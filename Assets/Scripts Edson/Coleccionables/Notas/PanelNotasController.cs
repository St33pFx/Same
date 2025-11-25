using UnityEngine;

public class PanelNotasController : MonoBehaviour
{
    [Header("Imágenes a mover")]
    [SerializeField] private RectTransform pauseImage;
    [SerializeField] private RectTransform notasImage;

    [Header("Posiciones (asignar en el inspector)")]
    [SerializeField] private Vector3 pausePosInicial;
    [SerializeField] private Vector3 pausePosFinal;
    [SerializeField] private Vector3 notasPosInicial;
    [SerializeField] private Vector3 notasPosFinal;

    [Header("Animación")]
    [Range(0.05f, 1.5f)]
    [SerializeField] private float duracion = 0.4f;
    [SerializeField] private LeanTweenType easing = LeanTweenType.easeInOutQuad;

    private void Awake()
    {
        // La imagen de notas comienza fuera de pantalla
        if (notasImage != null)
            notasImage.localPosition = notasPosInicial;

        // Se mantiene desactivada hasta que se abra por primera vez
        notasImage.gameObject.SetActive(false);
    }

    public void ActivarNotas()
    {
        notasImage.gameObject.SetActive(true);

        // Mover panel de pausa hacia la izquierda
        LeanTween.moveLocal(pauseImage.gameObject, pausePosFinal, duracion)
            .setEase(easing)
            .setIgnoreTimeScale(true);

        // Mover panel de notas hacia su posición visible
        LeanTween.moveLocal(notasImage.gameObject, notasPosFinal, duracion)
            .setEase(easing)
            .setIgnoreTimeScale(true);
    }

    public void DesactivarNotas()
    {
        // Mover panel de pausa a su posición original
        LeanTween.moveLocal(pauseImage.gameObject, pausePosInicial, duracion)
            .setEase(easing)
            .setIgnoreTimeScale(true);

        // Mover panel de notas fuera de pantalla
        LeanTween.moveLocal(notasImage.gameObject, notasPosInicial, duracion)
            .setEase(easing)
            .setIgnoreTimeScale(true)
            .setOnComplete(() =>
            {
                // Se desactiva al terminar animación
                notasImage.gameObject.SetActive(false);
            });
    }
}