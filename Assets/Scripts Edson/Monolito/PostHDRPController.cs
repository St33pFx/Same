using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System.Collections;

public class PostHDRPController : MonoBehaviour
{
    [Header("Referencia al Volume HDRP")]
    public Volume volume;

    private Vignette vignette;
    private ColorAdjustments colorAdjustments;

    [Header("Configuración del Fade")]
    public float VignetteInicio = 0f;
    public float VignetteFinal = 0.45f;
    public float SaturacionInicio = 0f;
    public float SaturacionFinal = -80f;
    public float DuracionFadeIn = 2f;

    [Header("Fade de salida")]
    public float DuracionFadeOut = 2f;
    public float DelayFadeOut = 3f;

    [Header("Curación")]
    public float CuracionDuracion = 2f; // Tiempo que tarda la curación en fade out

    private float timer = 0f;
    private bool isFadingIn = false;
    private bool isFadingOut = false;

    public bool IsHealing { get; private set; } = false;

    void Start()
    {
        if (volume != null && volume.profile != null)
        {
            volume.profile.TryGet(out vignette);
            volume.profile.TryGet(out colorAdjustments);

            if (vignette != null) vignette.intensity.value = VignetteInicio;
            if (colorAdjustments != null) colorAdjustments.saturation.value = SaturacionInicio;
        }
    }

    void Update()
    {
        // Fade de entrada
        if (isFadingIn)
        {
            timer += Time.deltaTime;
            float t = timer / DuracionFadeIn;

            if (vignette != null)
                vignette.intensity.value = Mathf.Lerp(VignetteInicio, VignetteFinal, t);

            if (colorAdjustments != null)
                colorAdjustments.saturation.value = Mathf.Lerp(SaturacionInicio, SaturacionFinal, t);

            if (timer >= DuracionFadeIn)
                isFadingIn = false;
        }

        // Fade de salida
        if (isFadingOut)
        {
            timer += Time.deltaTime;
            float t = timer / DuracionFadeOut;

            if (vignette != null)
                vignette.intensity.value = Mathf.Lerp(VignetteFinal, VignetteInicio, t);

            if (colorAdjustments != null)
                colorAdjustments.saturation.value = Mathf.Lerp(SaturacionFinal, SaturacionInicio, t);

            if (timer >= DuracionFadeOut)
                isFadingOut = false;
        }
    }

    public void StartFadeIn()
    {
        timer = 0f;
        isFadingIn = true;
        isFadingOut = false;
    }

    public void StartFadeOutWithDelay()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutDelayed());
    }

    private IEnumerator FadeOutDelayed()
    {
        yield return new WaitForSeconds(DelayFadeOut);

        timer = 0f;
        isFadingIn = false;
        isFadingOut = true;
    }

    // Método para aplicar curación con fade out
    public void ApplyHealing()
    {
        // Detener cualquier fade activo
        StopAllCoroutines();

        // Reiniciar estados de fade
        isFadingIn = false;
        isFadingOut = false;

        // Establecer los valores actuales al inicio de la curación
        if (vignette != null) vignette.intensity.value = vignette.intensity.value; // opcional, aseguramos consistencia
        if (colorAdjustments != null) colorAdjustments.saturation.value = colorAdjustments.saturation.value;

        // Iniciar corrutina de curación fade out
        StartCoroutine(HealingFadeOut());
    }

    private IEnumerator HealingFadeOut()
    {
        IsHealing = true; // Inicia el estado de curación
        float elapsed = 0f;

        float startVignette = vignette != null ? vignette.intensity.value : 0f;
        float startSaturation = colorAdjustments != null ? colorAdjustments.saturation.value : 0f;

        while (elapsed < CuracionDuracion)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / CuracionDuracion;

            if (vignette != null)
                vignette.intensity.value = Mathf.Lerp(startVignette, VignetteInicio, t);

            if (colorAdjustments != null)
                colorAdjustments.saturation.value = Mathf.Lerp(startSaturation, SaturacionInicio, t);

            yield return null;
        }

        if (vignette != null) vignette.intensity.value = VignetteInicio;
        if (colorAdjustments != null) colorAdjustments.saturation.value = SaturacionInicio;

        IsHealing = false; // Termina el estado de curación
    }
}