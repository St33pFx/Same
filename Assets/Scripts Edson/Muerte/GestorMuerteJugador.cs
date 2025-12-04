using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestorMuerteJugador : MonoBehaviour
{
    [Header("Configuración del Fade")]
    [SerializeField] private Image fadeImage;
    [Range(0.2f, 5f)] public float velocidadFade = 1.5f;

    [Header("Configuración de escena")]
    public string escenaDestino = "GameOver";

    private EstadisticasJugador stats;
    private bool jugadorMurio = false;

    private PlayerController playerController;
    private Shooter playerShooter;

    private void Start()
    {
        // Buscar estadísticas del jugador
        stats = FindObjectOfType<EstadisticasJugador>();

        // Buscar imagen del fade por tag si no está asignada
        if (fadeImage == null)
        {
            fadeImage = GameObject.FindGameObjectWithTag("Fade")?.GetComponent<Image>();
        }

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
        else
        {
            Debug.LogWarning("⚠️ No se encontró una imagen con tag 'Fade' para el fade de muerte.");
        }

        // Buscar PlayerController y Shooter
        PlayerController pc = FindObjectOfType<PlayerController>();
        if (pc != null) playerController = pc;

        Shooter sh = pc != null ? pc.GetComponent<Shooter>() : null;
        if (sh != null) playerShooter = sh;
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
        // Desactivar controles del jugador
        if (playerController != null) playerController.enabled = false;
        if (playerShooter != null) playerShooter.enabled = false;

        if (fadeImage != null)
        {
            // Activar fade con LeanTween ignorando Time.timeScale
            LeanTween.value(fadeImage.gameObject, 0f, 1f, velocidadFade)
                .setIgnoreTimeScale(true)
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
            SceneManager.LoadScene(escenaDestino);
        }
    }
}