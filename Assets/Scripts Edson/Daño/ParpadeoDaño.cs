using UnityEngine;

public class ParpadeoDaño : MonoBehaviour
{
    [Header("Referencia al Animator")]
    public Animator animator;

    [Header("Nombre del parámetro de trigger en Animator")]
    public string triggerNombre = "Parpadeo";

    [Header("Referencia a estadísticas del jugador")]
    public EstadisticasJugador stats;

    [Header("Vida crítica para mantener parpadeo")]
    public int vidaCritica = 20;

    /// <summary>
    /// Llamar este método para activar el parpadeo al recibir daño
    /// </summary>
    public void ActivarParpadeo()
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerNombre);
        }
    }

    void Update()
    {
        if (stats != null)
        {
            if (stats.vidaActual <= vidaCritica)
            {
                // Mantener animación activa si la vida está en nivel crítico
                animator.SetBool("VidaCritica", true);
            }
            else
            {
                animator.SetBool("VidaCritica", false);
            }
        }
    }
}
