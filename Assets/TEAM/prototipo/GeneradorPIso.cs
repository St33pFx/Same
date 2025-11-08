using System.Collections;
using UnityEngine;

public class GeneradorPiso : MonoBehaviour
{
    #region para edson y yahir
    //funciona? si
    //es optimo? no creo por que cada vez que genera baja uno o dos fps
    //porque no lo optimizo mas? ni idea we apenas y pude hacer que funcione
    #endregion

    [Header("Configuración de generación")]
    [Tooltip("Lista de prefabs de pisos que pueden generarse")]
    public GameObject[] PreafabsPasillos;

    [Tooltip("Empty donde se generará el nuevo piso (si está vacío se usa este transform)")]
    public Transform puntoGeneracion;

    [Tooltip("Referencia al guizmo del jugador (si no se asigna, se buscará por tag 'Player')")]
    public GuizmoJugador jugador;

    [HideInInspector] public bool puedeGenerar = true;
    public bool jugadorAdentro = false;
    public bool jugadorAfuera = false;

    private static bool spawnGlobalBloqueado = false;

    [Tooltip("Cooldown global entre spawns (segundos) para evitar múltiples instancias inmediatas")]
    public float SpawnGlobalCooldown = 0.25f;

    [Tooltip("Tiempo que se desactivan los colliders de los spawners recién instanciados (segundos)")]
    public float NuevoRestrasoDeSpawner = 0.2f;

    private GameObject ultimoPrefabGenerado;

    public BoxCollider colliderGeneracion;

    private void Reset()
    {
        puntoGeneracion = transform;
    }

    private void Awake()
    {
        if (jugador == null)
            jugador = GuizmoJugador.EncontrarJugador();

        if (puntoGeneracion == null)
            puntoGeneracion = transform;

        var col = GetComponent<Collider>();
        col.isTrigger = true;

        if (colliderGeneracion == null)
        {
            colliderGeneracion = puntoGeneracion.gameObject.AddComponent<BoxCollider>();
            colliderGeneracion.isTrigger = true;
            colliderGeneracion.size = new Vector3(5f, 5f, 5f);
            Debug.Log("collider de bloqueo adquirido");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        jugadorAdentro = true;

        if (ultimoPrefabGenerado != null)
            return;

        if (!puedeGenerar) return;
        if (spawnGlobalBloqueado) return;

        if (HayBloqueoEnPuntoDeGeneracion())
        {
            Debug.Log("Generación bloqueada: Hay un objeto con tag 'BloqueoGenerador' en el área.");
            return;
        }

        spawnGlobalBloqueado = true;
        puedeGenerar = false;
        jugadorAfuera = false;

        GenerarNuevoPiso();

        StartCoroutine(LevantarBloqueoGlobal(SpawnGlobalCooldown));
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        jugadorAdentro = false;
        jugadorAfuera = true;

        if (jugador == null) jugador = GuizmoJugador.EncontrarJugador();
    }

    private IEnumerator LevantarBloqueoGlobal(float Retraso)
    {
        yield return new WaitForSeconds(Retraso);
        spawnGlobalBloqueado = false;
    }

    private void GenerarNuevoPiso()
    {
        if (PreafabsPasillos == null || PreafabsPasillos.Length == 0 || puntoGeneracion == null) return;

        int randomIndex = Random.Range(0, PreafabsPasillos.Length);
        GameObject nuevoSuelo = Instantiate(PreafabsPasillos[randomIndex], puntoGeneracion.position, puntoGeneracion.rotation);

        ultimoPrefabGenerado = nuevoSuelo;

        GuizmoJugador JugadorActual = jugador ?? GuizmoJugador.EncontrarJugador();
        GeneradorPiso[] nuevoSpawner = nuevoSuelo.GetComponentsInChildren<GeneradorPiso>();
        foreach (var spawner in nuevoSpawner)
        {
            spawner.jugador = JugadorActual;
            spawner.puedeGenerar = false;

            Collider c = spawner.GetComponent<Collider>();
            if (c != null)
            {
                c.enabled = false;
                spawner.StartCoroutine(spawner.ReabilitarColliderDespuesDeDelay(c, NuevoRestrasoDeSpawner));
            }

            spawner.jugadorAdentro = false;
            spawner.jugadorAfuera = false;
        }
    }

    private IEnumerator ReabilitarColliderDespuesDeDelay(Collider c, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (c != null) c.enabled = true;
    }

    private bool HayBloqueoEnPuntoDeGeneracion()
    {
        if (colliderGeneracion == null)
        {
            Debug.LogWarning("No se encontró BoxCollider en el punto de generación.");
            return false;
        }

        Vector3 center = colliderGeneracion.bounds.center;
        Vector3 halfExtents = colliderGeneracion.bounds.extents;
        Collider[] objetosDentro = Physics.OverlapBox(center, halfExtents);

        foreach (Collider col in objetosDentro)
        {
            if (col.CompareTag("BloqueoGenerador"))
            {
                return true;
            }
        }

        return false;
    }
}



