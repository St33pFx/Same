using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CambioDeEscena : MonoBehaviour
{
    [SerializeField] private string escena;
    [SerializeField] private Image panelImagen;
    [SerializeField] private float duracionFade = 1f;

    private void Awake()
    {
        if (panelImagen != null)
        {
            panelImagen.gameObject.SetActive(false);
            Color c = panelImagen.color;
            c.a = 0f;
            panelImagen.color = c;
        }
    }

    public void Escena()
    {
        if (panelImagen != null)
        {
            panelImagen.gameObject.SetActive(true);

            LeanTween.value(panelImagen.gameObject, 0f, 1f, duracionFade)
                .setIgnoreTimeScale(true) // <- Permite que el fade funcione aun con timeScale = 0
                .setOnUpdate((float val) =>
                {
                    Color c = panelImagen.color;
                    c.a = val;
                    panelImagen.color = c;
                })
                .setOnComplete(() =>
                {
                    // 🔥 Justo AQUÍ restauramos el deltaTime antes de cambiar de escena
                    Time.timeScale = 1f;

                    SceneManager.LoadScene(escena);
                });
        }
        else
        {
            // También restauramos timeScale si no hay fade
            Time.timeScale = 1f;
            SceneManager.LoadScene(escena);
        }
    }

    public void Salir()
    {
        Application.Quit();
    }
}