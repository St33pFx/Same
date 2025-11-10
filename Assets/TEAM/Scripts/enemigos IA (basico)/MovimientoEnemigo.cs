using UnityEngine;
using UnityEngine.AI;

public class MovimientoEnemigo : MonoBehaviour
{
    private GameObject jugador;
    private NavMeshAgent agente;

    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (jugador != null)
        {
            agente.SetDestination(jugador.transform.position);
        }
    }
}
