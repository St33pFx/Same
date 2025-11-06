using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LeerNota : MonoBehaviour
{
    [Header("Imágenes de la UI")]
    public Image fondo;
    public Image imagenNota;

    [Header("Texto de la nota")]
    public TextMeshProUGUI textoNota;

    [Header("Configuraciones de Transición")]
    [Range(0f, 3f)] public float tiempoFadeIn = 1.2f;
    [Range(0f, 3f)] public float tiempoFadeOut = 0.8f;
    [Range(0f, 1f)] public float alphaMaxFondo = 0.6f;
    [Range(0f, 1f)] public float alphaMaxNota = 1f;

    [Header("Configuraciones Generales")]
    public string playerTag = "Player";
    [Range(0f, 3f)] public float tiempoBloqueoInput = 1f; // tiempo sin poder presionar E

    [Header("Referencias externas")]
    public MonoBehaviour playerController; // arrastra aquí el script del PlayerController
    public MonoBehaviour cameraBobbing;    // arrastra aquí el script FPSCameraBobbingAndSway

    private bool jugadorDentro = false;
    private bool notaActiva = false;
    private bool puedePresionarE = true;
    private bool bloqueando = false;

    void Start()
    {
        if (fondo != null) SetAlpha(fondo, 0f);
        if (imagenNota != null) SetAlpha(imagenNota, 0f);
        if (textoNota != null) textoNota.alpha = 0f;
    }

    void Update()
    {
        if (jugadorDentro && puedePresionarE && Input.GetKeyDown(KeyCode.E))
        {
            notaActiva = !notaActiva;
            FadeNota(notaActiva);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
            jugadorDentro = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            jugadorDentro = false;

            if (notaActiva)
            {
                notaActiva = false;
                FadeNota(false);
            }
        }
    }

    void FadeNota(bool mostrar)
    {
        if (!bloqueando)
            StartCoroutine(BloquearInputTemporal());

        float alphaFinalFondo = mostrar ? alphaMaxFondo : 0f;
        float alphaFinalNota = mostrar ? alphaMaxNota : 0f;
        float duracion = mostrar ? tiempoFadeIn : tiempoFadeOut;

        if (mostrar)
        {
            // Desactivar control del jugador y cámara
            if (playerController != null) playerController.enabled = false;
            if (cameraBobbing != null) cameraBobbing.enabled = false;
        }

        if (fondo != null)
        {
            LeanTween.cancel(fondo.gameObject);
            LeanTween.value(fondo.gameObject, fondo.color.a, alphaFinalFondo, duracion)
                .setOnUpdate((float a) => SetAlpha(fondo, a))
                .setEaseInOutQuad()
                .setOnComplete(() =>
                {
                    if (!mostrar)
                    {
                        // Reactivar control cuando termina el fade out
                        if (playerController != null) playerController.enabled = true;
                        if (cameraBobbing != null) cameraBobbing.enabled = true;
                    }
                });
        }

        if (imagenNota != null)
        {
            LeanTween.cancel(imagenNota.gameObject);
            LeanTween.value(imagenNota.gameObject, imagenNota.color.a, alphaFinalNota, duracion)
                .setOnUpdate((float a) => SetAlpha(imagenNota, a))
                .setEaseInOutQuad();
        }

        if (textoNota != null)
        {
            LeanTween.cancel(textoNota.gameObject);
            LeanTween.value(textoNota.gameObject, textoNota.alpha, mostrar ? 1f : 0f, duracion)
                .setEaseInOutQuad()
                .setOnUpdate((float a) => textoNota.alpha = a);
        }
    }

    void SetAlpha(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }

    IEnumerator BloquearInputTemporal()
    {
        bloqueando = true;
        puedePresionarE = false;
        yield return new WaitForSeconds(tiempoBloqueoInput);
        puedePresionarE = true;
        bloqueando = false;
    }
}