using UnityEngine;

public class CorduraHUD : MonoBehaviour
{
    public static CorduraHUD Instance;

    [Header("Referencias de UI")]
    public GameObject iconoCordura;
    public Cordura corduraJugador;

    [Header("Fade configuración")]
    [Range(0f, 2f)] public float fadeInTime = 0.3f;
    [Range(0f, 2f)] public float fadeOutTime = 0.5f;
    public float tiempoVisible = 2f;

    private CanvasGroup iconoCG;
    private bool iconoVisible = false;
    private float tiempoUltimaPerdida = 0f;

    private void Awake()
    {
        // Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Buscar Cordura automáticamente
        if (corduraJugador == null)
            corduraJugador = FindObjectOfType<Cordura>();
    }

    private void Start()
    {
        if (iconoCordura != null)
        {
            iconoCG = iconoCordura.GetComponent<CanvasGroup>();
            if (iconoCG == null) iconoCG = iconoCordura.AddComponent<CanvasGroup>();
            iconoCG.alpha = 0f;
        }
    }

    private void Update()
    {
        if (!iconoVisible) return;

        // Si la cordura es 0, mantener icono siempre visible
        if (corduraJugador != null && corduraJugador.corduraActual <= 0f)
        {
            SetAlpha(1f);
            return;
        }

        // Ocultar después de X segundos sin perder cordura
        if (Time.time - tiempoUltimaPerdida > tiempoVisible)
        {
            OcultarIcono();
        }
    }

    public void MostrarIcono()
    {
        tiempoUltimaPerdida = Time.time;

        if (!iconoVisible)
        {
            iconoVisible = true;
            LeanTween.alphaCanvas(iconoCG, 1f, fadeInTime);
        }
        else
        {
            iconoCG.alpha = 1f;
        }
    }

    private void OcultarIcono()
    {
        iconoVisible = false;
        LeanTween.alphaCanvas(iconoCG, 0f, fadeOutTime);
    }

    private void SetAlpha(float alpha)
    {
        if (iconoCG != null)
            iconoCG.alpha = alpha;
    }
}