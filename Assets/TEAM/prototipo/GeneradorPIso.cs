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

    [HideInInspector]
    public bool puedeGenerar = true;

    private bool jugadorAdentro = false;
    private bool jugadorAfuera = false;

    private static bool spawnGlobalBloqueado = false;
    [Tooltip("Cooldown global entre spawns (segundos) para evitar múltiples instancias inmediatas")]
    public float SpawnGlobalCooldown = 0.25f;

    [Tooltip("Tiempo que se desactivan los colliders de los spawners recién instanciados (segundos)")]
    public float NuevoRestrasoDeSpawner = 0.2f;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        jugadorAdentro = true;

        if (!puedeGenerar) return;
        if (spawnGlobalBloqueado) return;

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

        if (jugador != null && !jugador.EstaEnRango(puntoGeneracion.position))
        {
            puedeGenerar = true;
            jugadorAfuera = false;
        }
        else
        {
            StartCoroutine(EsperarGizmoEnSalirParaReactivar());
        }
    }

    private IEnumerator EsperarGizmoEnSalirParaReactivar()
    {
        while (true)
        {
            
            if (jugadorAdentro)
            {
                jugadorAfuera = false;
                yield break;
            }

            if (jugador == null) jugador = GuizmoJugador.EncontrarJugador();

            if (jugador != null && jugadorAfuera && !jugador.EstaEnRango(puntoGeneracion.position))
            {
                puedeGenerar = true;
                jugadorAfuera = false;
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
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
}

