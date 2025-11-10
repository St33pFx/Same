using UnityEngine;
using TMPro;

public class ScrollTexto : MonoBehaviour
{
    [Header("Referencia del texto (TextMeshProUGUI o RectTransform)")]
    public RectTransform texto;

    [Header("Posición inicial y final del movimiento")]
    public Vector3 posicionInicial;
    public Vector3 posicionFinal;

    [Header("Velocidad del desplazamiento")]
    public float velocidadScroll = 50f;

    [Header("Tiempo de espera al reiniciar")]
    public float tiempoReinicio = 0.5f;

    [Header("Duración del fade in/out")]
    [Range(0.1f, 5f)]
    public float duracionFade = 1f;

    private CanvasGroup canvasGroup;

    private void Start()
    {
        if (texto == null)
            texto = GetComponent<RectTransform>();

        // Asegurar que tenga un CanvasGroup
        canvasGroup = texto.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = texto.gameObject.AddComponent<CanvasGroup>();

        texto.anchoredPosition = posicionInicial;
        canvasGroup.alpha = 0;

        StartCoroutine(MoverTextoLoop());
    }

    private System.Collections.IEnumerator MoverTextoLoop()
    {
        while (true)
        {
            // Fade in
            LeanTween.alphaCanvas(canvasGroup, 1f, duracionFade);

            // Mover hacia la posición final
            while (Vector3.Distance(texto.anchoredPosition, posicionFinal) > 0.1f)
            {
                texto.anchoredPosition = Vector3.MoveTowards(texto.anchoredPosition, posicionFinal, velocidadScroll * Time.deltaTime);
                yield return null;
            }

            // Fade out
            LeanTween.alphaCanvas(canvasGroup, 0f, duracionFade);
            yield return new WaitForSeconds(duracionFade + tiempoReinicio);

            // Reiniciar posición
            texto.anchoredPosition = posicionInicial;
        }
    }
}
