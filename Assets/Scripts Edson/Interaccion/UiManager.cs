using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Referencias de UI")]
    public GameObject botonInteraccion;
    public GameObject iconoArma;
    public TextMeshProUGUI textoDisparo;
    public TextMeshProUGUI textoX;

    [Header("Fade configuración")]
    [Range(0f, 2f)] public float fadeInTime = 0.3f;
    [Range(0f, 2f)] public float fadeOutTime = 0.5f;
    public float tiempoVisible = 2f;

    private CanvasGroup iconoArmaCG;
    private CanvasGroup textoDisparoCG;
    private CanvasGroup textoXCG;

    private bool iconoVisible = false;
    private float tiempoUltimaAccion = 0f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        iconoArmaCG = GetOrAddCanvasGroup(iconoArma);
        textoDisparoCG = GetOrAddCanvasGroup(textoDisparo);
        textoXCG = GetOrAddCanvasGroup(textoX);

        SetAlpha(0f);

        if (botonInteraccion != null) botonInteraccion.SetActive(false);
    }

    private CanvasGroup GetOrAddCanvasGroup(GameObject go)
    {
        if (go == null) return null;
        CanvasGroup cg = go.GetComponent<CanvasGroup>();
        if (cg == null) cg = go.AddComponent<CanvasGroup>();
        return cg;
    }

    private CanvasGroup GetOrAddCanvasGroup(TextMeshProUGUI tmp)
    {
        if (tmp == null) return null;
        CanvasGroup cg = tmp.GetComponent<CanvasGroup>();
        if (cg == null) cg = tmp.gameObject.AddComponent<CanvasGroup>();
        return cg;
    }

    private void Update()
    {
        if (!iconoVisible) return;

        if (Time.time - tiempoUltimaAccion > tiempoVisible)
        {
            LeanTween.alphaCanvas(iconoArmaCG, 0f, fadeOutTime);
            LeanTween.alphaCanvas(textoDisparoCG, 0f, fadeOutTime);
            LeanTween.alphaCanvas(textoXCG, 0f, fadeOutTime);
            iconoVisible = false;
        }
    }

    public void MostrarBotonInteraccion(bool mostrar)
    {
        if (botonInteraccion != null)
            botonInteraccion.SetActive(mostrar);
    }

    public void MostrarDisparo()
    {
        if (MunicionPersistente.Instance == null) return;

        if (textoDisparo != null)
            textoDisparo.text = MunicionPersistente.Instance.municionActual.ToString();

        tiempoUltimaAccion = Time.time;

        if (!iconoVisible)
        {
            iconoVisible = true;
            LeanTween.alphaCanvas(iconoArmaCG, 1f, fadeInTime);
            LeanTween.alphaCanvas(textoDisparoCG, 1f, fadeInTime);
            LeanTween.alphaCanvas(textoXCG, 1f, fadeInTime);
        }
        else
        {
            SetAlpha(1f);
        }
    }

    private void SetAlpha(float alpha)
    {
        if (iconoArmaCG != null) iconoArmaCG.alpha = alpha;
        if (textoDisparoCG != null) textoDisparoCG.alpha = alpha;
        if (textoXCG != null) textoXCG.alpha = alpha;
    }
}