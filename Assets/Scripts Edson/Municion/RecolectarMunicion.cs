using UnityEngine;

public class RecolectarMunicion : MonoBehaviour
{
    [Header("Configuración")]
    public KeyCode botonAPresionar = KeyCode.E;
    public int cantidadMunicion = 10;

    private EstadisticasJugador stats;
    private bool jugadorDentro = false;

    private void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(botonAPresionar))
        {
            if (stats != null)
            {
                stats.AgregarMunicion(cantidadMunicion);
                Debug.Log("Se agregaron " + cantidadMunicion + " balas. Total: " + stats.municionActual);
                SoundManager.Instance.PlayPickupAmmo();
            }

            // Destruir el objeto de munición después de recogerlo
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
            // Busca automáticamente el componente EstadisticasJugador en el jugador
            stats = other.GetComponent<EstadisticasJugador>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
            stats = null;
        }
    }
}
