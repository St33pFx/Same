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

    private void Start()
    {
        // Sincronizar con munición persistente al inicio
        if (MunicionPersistente.Instance != null)
            municionActual = MunicionPersistente.Instance.municionActual;

        UIManager.Instance?.MostrarDisparo();
    }

    public void AgregarMunicion(int cantidad)
    {
        if (MunicionPersistente.Instance != null)
        {
            MunicionPersistente.Instance.AgregarMunicion(cantidad);
            municionActual = MunicionPersistente.Instance.municionActual;
        }

        UIManager.Instance?.MostrarDisparo();
    }

    public void Curarse(int cantidad)
    {
        vidaActual = Mathf.Min(vidaActual + cantidad, vidaMax);

        if (dañoVisual != null)
            dañoVisual.UpdateDañoVisual();
    }

    public void TomarDamage(int cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Max(vidaActual, 0);

        if (dañoVisual != null)
            dañoVisual.UpdateDañoVisual();

        if (parpadeoDaño != null)
            parpadeoDaño.ActivarParpadeo();

        if (vidaActual <= 0)
            Muerte();
    }

    private void Muerte()
    {
        Debug.Log("Jugador ha muerto");
    }
}