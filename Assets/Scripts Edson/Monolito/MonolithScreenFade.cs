using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonolithCanvasFade : MonoBehaviour
{
    [Header("Referencia a la imagen del fade")]
    public Image fadeImage;

    [Header("Collider del área de fade")]
    public Collider fadeCollider; // Asignar desde el inspector
    public string playerTag = "Player";

    [Header("Configuración del fade")]
    public float maxAlpha = 0.45f;
    public float fadeInTime = 2f;
    public float fadeOutTime = 2f;

    [Header("Curación")]
    public float healingDuration = 2f;   // Tiempo independiente de curación

    private Coroutine fadeCoroutine;
    private bool playerInside = false;
    public bool IsHealing { get; private set; } = false;

    private void Awake()
    {
        if (fadeImage == null)
            Debug.LogError("Asigna fadeImage en el inspector.");
        if (fadeCollider == null)
            Debug.LogError("Asigna fadeCollider en el inspector.");
        else
            fadeCollider.isTrigger = true;

        SetAlpha(0f);
    }

    private void Update()
    {
        if (fadeCollider == null || fadeImage == null) return;
        if (IsHealing) return; // no iniciar fades normales mientras cura

        bool isInsideNow = false;
        Collider[] hits = Physics.OverlapBox(fadeCollider.bounds.center, fadeCollider.bounds.extents);
        foreach (var hit in hits)
        {
            if (hit.CompareTag(playerTag))
            {
                isInsideNow = true;
                break;
            }
        }

        if (isInsideNow && !playerInside)
        {
            playerInside = true;
            StartFadeIn();
        }
        else if (!isInsideNow && playerInside)
        {
            playerInside = false;
            StartFadeOut(fadeOutTime);
        }
    }

    // ---------- Fade normal ----------
    private void StartFadeIn()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeCanvas(fadeImage.color.a, maxAlpha, fadeInTime));
    }

    private void StartFadeOut(float duration)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeCanvas(fadeImage.color.a, 0f, duration));
    }

    private IEnumerator FadeCanvas(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            SetAlpha(Mathf.Lerp(from, to, t));
            yield return null;
        }
        SetAlpha(to);
    }

    private void SetAlpha(float a)
    {
        if (fadeImage == null) return;
        Color c = fadeImage.color;
        c.a = Mathf.Clamp01(a);
        fadeImage.color = c;
    }

    // ---------- Curación ----------
    public void ApplyHealing()
    {
        if (IsHealing) return;
        IsHealing = true;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        StartCoroutine(HealingFade());
    }

    private IEnumerator HealingFade()
    {
        // Inicia el fade out con el tiempo de curación
        yield return StartCoroutine(FadeCanvas(fadeImage.color.a, 0f, healingDuration));

        // Asegura que al terminar quede completamente transparente
        SetAlpha(0f);

        IsHealing = false;
        playerInside = false;
        fadeCoroutine = null;
    }
}