using UnityEngine;

public class DeteccionNuevoPiso : MonoBehaviour
{
    [Header("Área de Detección")]
    [Tooltip("Tamaño del área cúbica que detecta nuevos pisos")]
    public float detectionRange = 5f;
    [Tooltip("Capa de los empties con etiqueta 'Nuevo Piso'")]
    public LayerMask detectionLayer;

    [Header("Control")]
    [Tooltip("Segundos a esperar después de un spawn antes de detectar de nuevo")]
    public float spawnCooldown = 0.25f;

    private float lastSpawnTime = -Mathf.Infinity;

    private void Update()
    {
        if (Time.time - lastSpawnTime < spawnCooldown) return;
        DetectFloorTriggers();
    }

    void DetectFloorTriggers()
    {
        Collider[] hits = Physics.OverlapBox(
            transform.position,
            Vector3.one * detectionRange * 0.5f,
            Quaternion.identity,
            detectionLayer
        );

        if (hits == null || hits.Length == 0) return;

        // Elegir el empty más cercano al jugador
        Collider closest = null;
        float minDistSqr = float.MaxValue;
        Vector3 myPos = transform.position;

        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("Nuevo Piso")) continue;

            float d = (hit.transform.position - myPos).sqrMagnitude;
            if (d < minDistSqr)
            {
                minDistSqr = d;
                closest = hit;
            }
        }

        if (closest == null) return;

        DetectorGenerador spawner = closest.GetComponent<DetectorGenerador>();
        if (spawner != null)
        {
            // TrySpawnFloor ahora devuelve true si spawn completado
            bool spawned = spawner.TrySpawnFloor();
            if (spawned)
            {
                lastSpawnTime = Time.time;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Vector3.one * detectionRange);
    }
}
