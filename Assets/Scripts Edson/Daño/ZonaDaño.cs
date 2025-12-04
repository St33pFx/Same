using UnityEngine;

public class ZonaDaño : MonoBehaviour
{
    [Header("Cantidad de daño")]
    public int daño = 10;

    [Header("Collider que activa el daño (asignar desde el inspector)")]
    public Collider triggerCollider; // Solo este collider hará que se aplique daño

    private bool dañoAplicado = false;

    private void OnTriggerEnter(Collider other)
    {
        // Solo aplicar daño si el collider que entró es el asignado
        if (!dañoAplicado && other == triggerCollider)
        {
            EstadisticasJugador stats = other.GetComponent<EstadisticasJugador>();
            if (stats != null)
            {
                stats.TomarDamage(daño);
                dañoAplicado = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Resetear el daño solo cuando el collider asignado salga
        if (other == triggerCollider)
        {
            dañoAplicado = false;
        }
    }
}
