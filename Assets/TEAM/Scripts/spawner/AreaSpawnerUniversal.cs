using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AreaSpawnerUniversal : MonoBehaviour
{
    [System.Serializable]
    public class PrefabEntry
    {
        public string nameTag;
        public GameObject prefab;
        [Range(0f, 1f)] public float probabilidad = 0.5f;
        public float minCount = 0;
        public float maxCount = 5;
        public bool incluirGeneracionDentroDeOtrosObjetos = false;
        public bool tieneNavMesh = false;
        [HideInInspector] public int spawnedCount = 0;
    }

    [System.Serializable]
    public class SpawnArea
    {
        public Transform pivot;
        public Vector3 boxSize = Vector3.one;
        public bool usarpivoteRotacion = true;
        public bool permitirGeneracionDentroDeLayers = false;
        public int maxIntentosPorSpawn = 40;
    }

    [Header("OBJETOS")]
    public List<PrefabEntry> prefabs = new List<PrefabEntry>();

    [Header("GIZMOS")]
    public List<SpawnArea> spawnAreas = new List<SpawnArea>();

    [Header("LÍMITE GLOBAL")]
    public int maxSpawnGlobal = 50;

    [Header("CAPAS")]
    public LayerMask layerPermitidasEstarAdentro = ~0;
    public LayerMask layersIgnoradas = 0;

    [Header("SUELO")]
    public bool colocarPrefabsSobreSuperficie = true;
    public float alturaDeInicioRaycast = 6f;
    public float offsetVertical = 0.02f;

    public Transform EmptyComoCarpeta;

    [Header("DEBUG / PREVIEW")]
    public bool mostrarPreview = true;
    public int cantidadPreview = 40;
    public bool mostrarPosicionesGeneradas = true;

    private List<GameObject> spawned = new List<GameObject>();
    private List<Vector3> puntosDePreview = new List<Vector3>();

    [Header("INICIO AUTOMÁTICO")]
    public bool generarAlIniciar = true;

    [ContextMenu("SPAWN GLOBAL")]

    void Start()
    {
        if (generarAlIniciar)
        {
            Debug.Log("[Spawner] Generación automática al iniciar...");
            SpawnGlobal();
        }
    }
    [ContextMenu("SPAWN GLOBAL")]
    public void SpawnGlobal()
    {
        if (prefabs.Count == 0 || spawnAreas.Count == 0)
        {
            Debug.LogError("[Spawner] No hay Prefabs o SpawnAreas asignadas.");
            return;
        }

        ClearSpawned();
        foreach (var p in prefabs)
            p.spawnedCount = 0;

        int totalSpawned = 0;
        int intentosTotales = 0;
        int maxIntentosTotales = maxSpawnGlobal * 20;

        while (totalSpawned < maxSpawnGlobal && intentosTotales < maxIntentosTotales)
        {
            intentosTotales++;

            SpawnArea area = spawnAreas[Random.Range(0, spawnAreas.Count)];
            if (area == null || area.pivot == null)
                continue;

            PrefabEntry entry = SelectPrefabByWeight(area);
            if (entry == null)
                continue;

            int maxAllowed = Mathf.RoundToInt(entry.maxCount);
            if (entry.spawnedCount >= maxAllowed)
                continue;

            bool success = TrySpawnOne(entry, area, false);

            if (success)
            {
                entry.spawnedCount++;
                totalSpawned++;
            }
        }

        if (totalSpawned < maxSpawnGlobal)
        {
            Debug.LogWarning($"[Spawner] Solo se generaron {totalSpawned} de {maxSpawnGlobal}. No quedaron posiciones válidas.");
        }

        Debug.Log($"[Spawner] Total generado: {spawned.Count}");
    }


    [ContextMenu("CLEAR")]
    public void ClearSpawned()
    {
        for (int i = spawned.Count - 1; i >= 0; i--)
        {
            if (spawned[i] == null) continue;

#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(spawned[i]);
            else
                Destroy(spawned[i]);
#else
            Destroy(spawned[i]);
#endif
        }
        spawned.Clear();
    }

    bool TrySpawnOne(PrefabEntry entry, SpawnArea area, bool logDebug)
    {
        if (entry == null || entry.prefab == null)
        {
            Debug.LogError("[Spawner] PrefabEntry o Prefab NULL. Revisa el Inspector.");
            return false;
        }

        Bounds b = CalculatePrefabBounds(entry.prefab);
        Vector3 halfExtents = b.extents;

        for (int i = 0; i < area.maxIntentosPorSpawn; i++)
        {
            Vector3 local = new Vector3(
                Random.Range(-area.boxSize.x / 2, area.boxSize.x / 2),
                Random.Range(-area.boxSize.y / 2, area.boxSize.y / 2),
                Random.Range(-area.boxSize.z / 2, area.boxSize.z / 2)
            );

            Vector3 candidate = area.usarpivoteRotacion ?
                area.pivot.TransformPoint(local) :
                area.pivot.position + local;

            //---------- NAVMESH ----------
            if (entry.tieneNavMesh)
            {
                if (!NavMesh.SamplePosition(candidate, out NavMeshHit nh, 3f, NavMesh.AllAreas))
                    continue;

                candidate = nh.position;
            }
            //---------- SUELO ----------
            else if (colocarPrefabsSobreSuperficie)
            {
                if (Physics.Raycast(candidate + Vector3.up * alturaDeInicioRaycast,
                    Vector3.down, out RaycastHit hit,
                    alturaDeInicioRaycast * 2f, ~layersIgnoradas,
                    QueryTriggerInteraction.Ignore))
                {
                    candidate = hit.point + Vector3.up * (halfExtents.y + offsetVertical);
                }
                else if (!entry.incluirGeneracionDentroDeOtrosObjetos)
                {
                    continue;
                }
            }

            if (entry.incluirGeneracionDentroDeOtrosObjetos && !area.permitirGeneracionDentroDeLayers)
                continue;

            //---------- COLLISIONES ----------
            Collider[] overlaps = Physics.OverlapBox(
                candidate,
                halfExtents * 0.9f,
                Quaternion.identity,
                ~layersIgnoradas,
                QueryTriggerInteraction.Ignore
            );

            bool blocked = false;
            foreach (var c in overlaps)
            {
                if (c.isTrigger) continue;

                if (!entry.incluirGeneracionDentroDeOtrosObjetos)
                {
                    blocked = true;
                    break;
                }
            }

            if (blocked)
                continue;

            //---------- INSTANTIAR ----------
            GameObject go = Instantiate(entry.prefab, candidate, Quaternion.identity, EmptyComoCarpeta);
            go.SetActive(true);
            go.transform.localScale = entry.prefab.transform.localScale;
            go.name += "_SPAWNED";

            spawned.Add(go);

            if (logDebug)
            {
                Debug.Log($"[Spawner] ✅ Generado: {entry.prefab.name}");
                Debug.Log($"Posición: {candidate}");
                Debug.Log($"Área: {area.pivot.name}");
            }

            return true;
        }

        return false;
    }

    PrefabEntry SelectPrefabByWeight(SpawnArea area)
    {
        float total = 0f;

        foreach (var p in prefabs)
        {
            if (p.prefab == null) continue;

            if (p.incluirGeneracionDentroDeOtrosObjetos && !area.permitirGeneracionDentroDeLayers)
                continue;

            total += Mathf.Max(0.01f, p.probabilidad);
        }

        float r = Random.Range(0f, total);
        float acc = 0;

        foreach (var p in prefabs)
        {
            if (p.prefab == null) continue;

            if (p.incluirGeneracionDentroDeOtrosObjetos && !area.permitirGeneracionDentroDeLayers)
                continue;

            acc += Mathf.Max(0.01f, p.probabilidad);
            if (r <= acc)
                return p;
        }

        return null;
    }

    // ---------- BOUNDS SEGUROS ----------
    Bounds CalculatePrefabBounds(GameObject prefab)
    {
        if (prefab == null)
            return new Bounds(Vector3.zero, Vector3.one);

        Collider col = prefab.GetComponentInChildren<Collider>();
        if (col != null)
            return col.bounds;

        Renderer rend = prefab.GetComponentInChildren<Renderer>();
        if (rend != null)
            return rend.bounds;

        return new Bounds(Vector3.zero, Vector3.one);
    }

    // ===================== PREVIEW =====================

    void GeneratePreview()
    {
        puntosDePreview.Clear();
        if (!mostrarPreview) return;

        for (int i = 0; i < cantidadPreview; i++)
        {
            SpawnArea area = spawnAreas[Random.Range(0, spawnAreas.Count)];
            if (area?.pivot == null) continue;

            Vector3 local = new Vector3(
                Random.Range(-area.boxSize.x / 2, area.boxSize.x / 2),
                Random.Range(-area.boxSize.y / 2, area.boxSize.y / 2),
                Random.Range(-area.boxSize.z / 2, area.boxSize.z / 2)
            );

            Vector3 point = area.usarpivoteRotacion ?
                area.pivot.TransformPoint(local) :
                area.pivot.position + local;

            if (Physics.Raycast(point + Vector3.up * alturaDeInicioRaycast,
                Vector3.down, out RaycastHit hit,
                alturaDeInicioRaycast * 2f))
            {
                puntosDePreview.Add(hit.point);
            }
        }
    }

    // ===================== GIZMOS =====================

    void OnDrawGizmosSelected()
    {
        GeneratePreview();

        Gizmos.color = Color.yellow;
        foreach (var p in puntosDePreview)
            Gizmos.DrawSphere(p, 0.2f);

        if (mostrarPosicionesGeneradas)
        {
            Gizmos.color = Color.red;
            foreach (var go in spawned)
            {
                if (go == null) continue;
                Gizmos.DrawSphere(go.transform.position, 0.25f);
                Gizmos.DrawLine(transform.position, go.transform.position);
            }
        }
    }

    void OnDrawGizmos()
    {
        foreach (var area in spawnAreas)
        {
            if (area?.pivot == null) continue;

            Gizmos.color = area.permitirGeneracionDentroDeLayers ? Color.cyan : Color.green;

            Matrix4x4 m = area.usarpivoteRotacion ?
                Matrix4x4.TRS(area.pivot.position, area.pivot.rotation, Vector3.one) :
                Matrix4x4.TRS(area.pivot.position, Quaternion.identity, Vector3.one);

            Gizmos.matrix = m;
            Gizmos.DrawWireCube(Vector3.zero, area.boxSize);
        }

        Gizmos.matrix = Matrix4x4.identity;
    }

    // ===================== VALIDACIÓN =====================

    void OnValidate()
    {
        if (spawnAreas.Count == 0)
            Debug.LogWarning("[Spawner] No hay SpawnAreas.");

        for (int i = 0; i < prefabs.Count; i++)
        {
            if (prefabs[i].prefab == null)
                Debug.LogError($"[Spawner] Prefab NULL en índice {i}");

            if (prefabs[i].maxCount < prefabs[i].minCount)
                prefabs[i].maxCount = prefabs[i].minCount;
        }
    }
}
