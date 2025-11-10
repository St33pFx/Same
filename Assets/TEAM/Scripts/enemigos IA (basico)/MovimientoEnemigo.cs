using UnityEngine;
using UnityEngine.AI;

public class MovimientoEnemigo : MonoBehaviour
{
    private Transform jugador;
    private NavMeshAgent agente;

    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        agente = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (jugador != null)
        {
            agente.SetDestination(jugador.position);
        }
    }
}
