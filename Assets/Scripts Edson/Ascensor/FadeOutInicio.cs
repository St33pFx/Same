using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOutInicio : MonoBehaviour
{
    public Image imagenFade; // Imagen a desvanecer
    public float duracion = 1.5f; // Tiempo del fade en segundos

    private void Start()
    {
        if (imagenFade != null)
            StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float tiempo = 0f;
        Color colorInicial = imagenFade.color;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, tiempo / duracion);
            imagenFade.color = new Color(colorInicial.r, colorInicial.g, colorInicial.b, alpha);
            yield return null;
        }

        imagenFade.color = new Color(colorInicial.r, colorInicial.g, colorInicial.b, 0f);
    }
}
