using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonolithCanvasFade : MonoBehaviour
{
    [Header("Referencia a la imagen del fade")]
    public Image fadeImage;

    [Header("Collider del monolito")]
    public Collider fadeCollider; // Asignar desde el inspector
    public string playerTag = "Player";

    [Header("Configuración del fade")]
    public float maxAlpha = 0.45f;
    public float fadeInTime = 2f;
    public float fadeOutTime = 2f;
    public float fadeOutDelay = 3f;

    [Header("Curación")]
    public float healingDuration = 2f;

    private Coroutine fadeCoroutine;
    private Coroutine healingCoroutine;
    private bool playerInside = false;
    public bool IsHealing { get; private set; } = false;

    private void Awake()
    {
        if (fadeCollider == null)
            Debug.LogError("Debes asignar el Collider del monolito en el Inspector.");

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    private void Update()
    {
        // Detectar si el jugador está dentro del collider asignado
        if (fadeCollider != null && !IsHealing)
        {
            bool inside = fadeCollider.bounds.Contains(PlayerPosition());
            if (inside && !playerInside)
            {
                playerInside = true;
                StartFadeIn();
            }
            else if (!inside && playerInside)
            {
                playerInside = false;
                StartFadeOutWithDelay();
            }
        }
    }

    private Vector3 PlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        return player != null ? player.transform.position : Vector3.zero;
    }

    #region Fade Normal
    private void StartFadeIn()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeCanvas(0f, maxAlpha, fadeInTime));
    }

    private void StartFadeOutWithDelay()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        yield return new WaitForSeconds(fadeOutDelay);
        fadeCoroutine = StartCoroutine(FadeCanvas(fadeImage.color.a, 0f, fadeOutTime));
        yield return fadeCoroutine;
        fadeCoroutine = null;
    }

    private IEnumerator FadeCanvas(float startAlpha, float targetAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            Color c = fadeImage.color;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeImage.color = c;
            yield return null;
        }

        Color finalColor = fadeImage.color;
        finalColor.a = targetAlpha;
        fadeImage.color = finalColor;
    }
    #endregion

    #region Curación
    // Curación independiente
    public void ApplyHealing()
    {
        // Detener cualquier fade de curación activo
        if (healingCoroutine != null)
            StopCoroutine(healingCoroutine);

        // Detener el fade normal mientras se aplica la curación
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        // Iniciar curación independiente usando su propio tiempo
        healingCoroutine = StartCoroutine(HealingFadeOut());
    }

    private IEnumerator HealingFadeOut()
    {
        IsHealing = true;
        float elapsed = 0f;
        float startAlpha = fadeImage.color.a;

        while (elapsed < healingDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / healingDuration; // Usamos el tiempo de curación

            Color c = fadeImage.color;
            c.a = Mathf.Lerp(startAlpha, 0f, t);
            fadeImage.color = c;

            yield return null;
        }

        Color finalColor = fadeImage.color;
        finalColor.a = 0f;
        fadeImage.color = finalColor;

        IsHealing = false;
        healingCoroutine = null;
    }
    #endregion
}