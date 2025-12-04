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

    private void Awake()
    {
        // Buscar automáticamente el componente EstadisticasJugador si no está asignado
        if (stats == null)
            stats = FindObjectOfType<EstadisticasJugador>();

        // Buscar automáticamente el Animator en hijos si no se asignó uno
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (animator == null)
            Debug.LogWarning("ParpadeoDaño: No se encontró un Animator en este objeto o en sus hijos.");
    }

    // Llamar este método para activar el parpadeo al recibir daño
    public void ActivarParpadeo()
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerNombre);
        }
    }

    void Update()
    {
        if (stats != null && animator != null)
        {
            if (stats.vidaActual <= vidaCritica)
            {
                animator.SetBool("VidaCritica", true);
            }
            else
            {
                animator.SetBool("VidaCritica", false);
            }
        }
    }
}