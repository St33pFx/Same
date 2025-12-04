using UnityEngine;

public class ProyectilEnemigo : MonoBehaviour
{
    [Header("Cantidad de daño")]
    public int daño = 10;

    private bool dañoAplicado = false;

    private void OnTriggerEnter(Collider other)
    {
        // Aplicar daño solo si es el jugador y aún no se aplicó
        if (!dañoAplicado && other.CompareTag("Player"))
        {
            EstadisticasJugador stats = other.GetComponent<EstadisticasJugador>();
            if (stats != null)
            {
                stats.TomarDamage(daño);
                dañoAplicado = true; // Marcar que el daño ya se aplicó
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Resetear para poder aplicar daño nuevamente al salir
        if (other.CompareTag("Player"))
        {
            dañoAplicado = false;
        }
    }
}
