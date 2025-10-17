using UnityEngine;
using System.Collections;

public class DestruccionPiso : MonoBehaviour
{
    [Tooltip("Referencia al guizmo del jugador (si no asignas, se buscará por tag 'Player')")]
    public GuizmoJugador jugador;

    [Tooltip("Tiempo de espera antes de destruir el piso si el jugador está fuera de rango")]
    public float retrasoDestruccion = 2f;

    [Tooltip("Intervalo de chequeo mientras el jugador está lejos")]
    public float intervaloRevision = 1f;

    private GeneradorPiso[] spawners;

    private void Start()
    {
        if (jugador == null)
            jugador = GuizmoJugador.EncontrarJugador();

        spawners = GetComponentsInChildren<GeneradorPiso>();
        foreach (var spawner in spawners)
        {
            if (spawner != null)
                spawner.jugador = jugador;
        }

        StartCoroutine(CheckLoop());
    }

    private IEnumerator CheckLoop()
    {
        while (true)
        {
            bool playerClose = false;

            if (jugador == null)
                jugador = GuizmoJugador.EncontrarJugador();

            if (jugador != null)
            {
                foreach (var spawner in spawners)
                {
                    if (spawner == null) continue;

                    if (jugador.EstaEnRango(spawner.transform.position))
                    {
                        playerClose = true;
                        spawner.puedeGenerar = true;
                    }
                }
            }

            if (!playerClose)
            {
                yield return new WaitForSeconds(retrasoDestruccion);

                // Recomprueba
                if (!AnySpawnerInRange())
                {
                    Destroy(gameObject);
                    yield break;
                }
            }

            yield return new WaitForSeconds(intervaloRevision);
        }
    }

    private bool AnySpawnerInRange()
    {
        if (jugador == null) return false;

        foreach (var spawner in spawners)
        {
            if (spawner == null) continue;
            if (jugador.EstaEnRango(spawner.transform.position))
                return true;
        }
        return false;
    }
}

