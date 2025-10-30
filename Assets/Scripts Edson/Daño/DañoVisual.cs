using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ImagenDaño
{
    public Image imagen;
    [Range(0f, 1f)] public float alfaMaximo = 0.8f; // Alfa máximo que puede alcanzar esta imagen
    [Range(0f, 1f)] public float umbralVida = 0.75f; // Vida proporcional para empezar a activarse (0 a 1)
    [Range(0.1f, 10f)] public float duracionTransicion = 1f;
}

public class DañoVisual : MonoBehaviour
{
    [Header("Referencia a estadísticas del jugador")]
    public EstadisticasJugador stats;

    [Header("Imágenes de efecto de daño")]
    public ImagenDaño[] imagenesDaño;

    void Start()
    {
        // Inicializar todas las imágenes con alfa 0
        foreach (var ic in imagenesDaño)
        {
            if (ic.imagen != null)
            {
                Color c = ic.imagen.color;
                c.a = 0f;
                ic.imagen.color = c;
            }
        }
    }

    void Update()
    {
        // Actualizar visual cada frame
        UpdateDañoVisual();
    }

    // Método público que se puede llamar desde EstadisticasJugador
    public void UpdateDañoVisual()
    {
        if (stats == null) return;

        float vidaProporcional = (float)stats.vidaActual / stats.vidaMax;

        foreach (var ic in imagenesDaño)
        {
            if (ic.imagen == null) continue;

            if (vidaProporcional <= ic.umbralVida)
            {
                float factor = (ic.umbralVida - vidaProporcional) / ic.umbralVida;
                float alfaObjetivo = Mathf.Clamp01(factor * ic.alfaMaximo);

                // Aplicar transición suave con LeanTween
                LeanTween.cancel(ic.imagen.gameObject);
                Color colorActual = ic.imagen.color;
                LeanTween.value(ic.imagen.gameObject, colorActual.a, alfaObjetivo, ic.duracionTransicion)
                    .setOnUpdate((float val) =>
                    {
                        Color c = ic.imagen.color;
                        c.a = val;
                        ic.imagen.color = c;
                    });
            }
            else
            {
                // Mantener alfa 0 si la vida está por encima del umbral
                LeanTween.cancel(ic.imagen.gameObject);
                Color c = ic.imagen.color;
                c.a = 0f;
                ic.imagen.color = c;
            }
        }
    }
}