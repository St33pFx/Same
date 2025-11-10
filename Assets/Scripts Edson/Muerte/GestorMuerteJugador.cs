using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestorMuerteJugador : MonoBehaviour
{
    [Header("Configuración del Fade")]
    [SerializeField] private Image fadeImage; // Imagen negra en el Canvas
    [Range(0.2f, 5f)] public float velocidadFade = 1.5f; // Duración del fade in

    [Header("Configuración de escena")]
    public string escenaDestino = "GameOver"; // Nombre de la escena a cargar

    private EstadisticasJugador stats;
    private bool jugadorMurio = false;

    private void Start()
    {
        // Buscar componente de vida del jugador automáticamente
        stats = FindObjectOfType<EstadisticasJugador>();

        if (fadeImage == null)
        {
            fadeImage = GameObject.FindGameObjectWithTag("Fade")?.GetComponent<Image>();
        }

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0;
            fadeImage.color = c;
        }
    }

    private void Update()
    {
        if (stats != null && stats.vidaActual <= 0 && !jugadorMurio)
        {
            jugadorMurio = true;
            IniciarSecuenciaMuerte();
        }
    }

    private void IniciarSecuenciaMuerte()
    {
        if (fadeImage != null)
        {
            // Fade in usando LeanTween
            LeanTween.value(fadeImage.gameObject, 0f, 1f, velocidadFade)
                .setOnUpdate((float valor) =>
                {
                    Color c = fadeImage.color;
                    c.a = valor;
                    fadeImage.color = c;
                })
                .setOnComplete(() =>
                {
                    SceneManager.LoadScene(escenaDestino);
                });
        }
        else
        {
            // Si no hay imagen, carga directo
            SceneManager.LoadScene(escenaDestino);
        }
    }
}
