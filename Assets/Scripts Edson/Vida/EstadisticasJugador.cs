using UnityEngine;

public class EstadisticasJugador : MonoBehaviour
{
    [Header("Estadísticas Jugador")]
    public int vidaMax = 100;
    public int vidaActual = 100;

    [Header("Munición")]
    public int municionActual = 0;

    [Header("Referencia a daño visual")]
    public DañoVisual dañoVisual;

    [Header("Referencia a parpadeo de daño")]
    public ParpadeoDaño parpadeoDaño;

    // Agregar munición
    public void AgregarMunicion(int cantidad)
    {
        municionActual += cantidad;
    }

    // Curación del jugador
    public void Curarse(int cantidad)
    {
        vidaActual = Mathf.Min(vidaActual + cantidad, vidaMax);

        // Actualizar efecto de daño visual al curarse
        if (dañoVisual != null)
            dañoVisual.UpdateDañoVisual();
    }

    // Tomar daño
    public void TomarDamage(int cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Max(vidaActual, 0);

        // Actualizar efecto de daño visual al recibir daño
        if (dañoVisual != null)
            dañoVisual.UpdateDañoVisual();

        // Activar parpadeo de daño
        if (parpadeoDaño != null)
            parpadeoDaño.ActivarParpadeo();

        if (vidaActual <= 0)
            Muerte();
    }

    private void Muerte()
    {
        Debug.Log("Jugador ha muerto gg");
        // Aquí puedes agregar animaciones, reinicio de escena, etc.
    }
}