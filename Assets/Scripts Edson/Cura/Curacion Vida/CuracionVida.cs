using UnityEngine;

public class CuracionVida : MonoBehaviour
{
    [Header("Referencia a estadísticas del jugador")]
    public EstadisticasJugador stats;

    [Header("Configuración")]
    public KeyCode botonAPresionar = KeyCode.E;
    public int cantidadCuracion = 50;

    private bool jugadorDentro = false;

    private void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(botonAPresionar))
        {
            if (stats != null)
            {
                stats.Curarse(cantidadCuracion);
            }

            // Destruir el objeto curativo después de usarlo
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
        }
    }
}