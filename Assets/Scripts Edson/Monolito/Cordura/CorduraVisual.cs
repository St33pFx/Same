using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ImagenCordura
{
    public Image imagen;
    [Range(0f, 1f)] public float alfaObjetivo = 0.8f;
    [Range(0.1f, 10f)] public float duracionTransicion = 2f;
    [Range(0f, 15f)] public float tiempoActivacion = 1f;

    [HideInInspector] public float temporizadorActual = 0f;
    [HideInInspector] public bool activada = false;
}

public class CorduraVisual : MonoBehaviour
{
    [Header("Referencia a Cordura")]
    public Cordura cordura;

    [Header("Vignette")]
    public ImagenCordura[] imagenesCordura;

    [Header("Tiempo de desvanecimiento al curarse")]
    public float tiempoDesvanecimiento = 1.5f;

    [HideInInspector] public bool jugadorDentroMonolito = false;
    private bool temporizadoresIniciados = false;

    void Start()
    {
        foreach (var ic in imagenesCordura)
        {
            if (ic.imagen != null)
            {
                Color c = ic.imagen.color;
                c.a = 0f;
                ic.imagen.color = c;

                ic.temporizadorActual = ic.tiempoActivacion;
                ic.activada = false;
            }
        }
    }

    void Update()
    {
        if (cordura == null) return;

        // Activar temporizadores solo cuando cordura = 0
        if (cordura.corduraActual <= 0f && !temporizadoresIniciados)
        {
            temporizadoresIniciados = true;
            foreach (var ic in imagenesCordura)
            {
                ic.temporizadorActual = ic.tiempoActivacion;
                ic.activada = false;
            }
        }

        // Si cordura > 0, detener temporizadores y desvanecer imágenes
        if (cordura.corduraActual > 0f && temporizadoresIniciados)
        {
            temporizadoresIniciados = false;
            foreach (var ic in imagenesCordura)
            {
                ic.temporizadorActual = ic.tiempoActivacion;
                ic.activada = false;
                ActivarImagen(ic, false, tiempoDesvanecimiento);
            }
        }

        // Ejecutar temporizadores escalonados solo si el jugador está dentro del monolito
        if (temporizadoresIniciados && jugadorDentroMonolito)
        {
            foreach (var ic in imagenesCordura)
            {
                if (!ic.activada)
                {
                    ic.temporizadorActual -= Time.deltaTime;
                    if (ic.temporizadorActual <= 0f)
                    {
                        ic.activada = true;
                        ActivarImagen(ic, true, ic.duracionTransicion);
                    }
                }
            }
        }
    }

    public void JugadorDentroMonolito(bool dentro)
    {
        jugadorDentroMonolito = dentro;
    }

    private void ActivarImagen(ImagenCordura ic, bool activar, float duracion)
    {
        if (ic.imagen == null) return;

        LeanTween.cancel(ic.imagen.gameObject);

        float alfaFinal = activar ? ic.alfaObjetivo : 0f;
        Color colorActual = ic.imagen.color;

        LeanTween.value(ic.imagen.gameObject, colorActual.a, alfaFinal, duracion)
            .setOnUpdate((float val) =>
            {
                Color c = ic.imagen.color;
                c.a = val;
                ic.imagen.color = c;
            });
    }
}