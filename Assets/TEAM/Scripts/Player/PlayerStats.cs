using Unity.Mathematics;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Estadisticas Jugador")]
    [SerializeField] private int vidaMax = 100;
    [SerializeField] private int vidaActuali = 100;

    [Header("Municion")]
    [SerializeField] private int municionActual = 0;

    public void AgregarMunicion(int municionCantidad)
    {
        municionActual += municionCantidad;
    }

    public void Curarse(int curacionCantidad)
    {
        vidaActuali = Mathf.Min(vidaActuali + curacionCantidad, vidaMax);
    }

    public void tomarDamage(int cantidadDamage)
    {
        vidaActuali -= cantidadDamage;
        SoundManager.Instance.PlayPlayerHit();
        if (vidaActuali <= 0)
        {
            Muerte();
        }
        
    }

    public void Muerte()
    {
        Debug.Log("Jugador ha muerto gg");
        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlayDeath();
    }
}
