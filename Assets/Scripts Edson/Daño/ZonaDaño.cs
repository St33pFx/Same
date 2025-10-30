using UnityEngine;

public class ZonaDaño : MonoBehaviour
{
    [Header("Cantidad de daño")]
    public int daño = 10;

    private bool dañoAplicado = false;

    private void OnTriggerEnter(Collider other)
    {
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
        if (other.CompareTag("Player"))
        {
            dañoAplicado = false; // Resetear al salir para poder aplicar otra vez
        }
    }
}
