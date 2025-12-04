using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ImagenDaño
{
    public Image imagen;
    [Range(0f, 1f)] public float alfaMaximo = 1f;
    [Range(0f, 1f)] public float umbralVida = 0.75f;
    [Range(0.1f, 10f)] public float duracionTransicion = .1f;
}

public class DañoVisual : MonoBehaviour
{
    public EstadisticasJugador stats;
    public ImagenDaño[] imagenesDaño;

    private int vidaAnterior;

    private void Awake()
    {
        if (stats == null)
        {
            stats = FindObjectOfType<EstadisticasJugador>();
        }

        // Cargar imágenes desde un manager
        Image[] imgs = UIEffectsManager.instance.GetImagenesDaño();
        for (int i = 0; i < imagenesDaño.Length && i < imgs.Length; i++)
            imagenesDaño[i].imagen = imgs[i];
    }

    void Start()
    {
        vidaAnterior = stats.vidaActual;

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
        // Solo actualiza si la vida cambió
        if (stats.vidaActual != vidaAnterior)
        {
            vidaAnterior = stats.vidaActual;
            UpdateDañoVisual();
        }
    }

    public void UpdateDañoVisual()
    {
        float vidaProporcional = (float)stats.vidaActual / stats.vidaMax;

        foreach (var ic in imagenesDaño)
        {
            if (ic.imagen == null) continue;

            float alfaObjetivo = 0f;

            if (vidaProporcional <= ic.umbralVida)
            {
                float factor = (ic.umbralVida - vidaProporcional) / ic.umbralVida;
                alfaObjetivo = Mathf.Clamp01(factor * ic.alfaMaximo);
            }

            LeanTween.cancel(ic.imagen.gameObject);

            Color colorActual = ic.imagen.color;

            LeanTween.value(
                ic.imagen.gameObject,
                colorActual.a,
                alfaObjetivo,
                ic.duracionTransicion
            )
            .setOnUpdate((float val) =>
            {
                Color c = ic.imagen.color;
                c.a = val;
                ic.imagen.color = c;
            });
        }
    }
}