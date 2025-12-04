using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TriggerSceneAndCollider : MonoBehaviour
{
    [Header("Nombre de la escena a cargar")]
    public string sceneName;

    [Header("Collider a activar")]
    public Collider colliderToActivate;

    [Header("Duración del fade (segundos)")]
    public float fadeDuration = 1.5f;

    [Header("Imagen para el fade in")]
    public Image fadeImage;

    private bool playerInTrigger = false;
    private bool sceneChangeStarted = false;

    private void Awake()
    {
        // Inicializar alfa de la imagen en 0
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTrigger = false;
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E) && !sceneChangeStarted)
        {
            sceneChangeStarted = true;

            // Activar el collider asignado
            if (colliderToActivate != null)
                colliderToActivate.enabled = true;

            // Iniciar fade con LeanTween
            if (fadeImage != null)
            {
                fadeImage.gameObject.SetActive(true);

                LeanTween.value(fadeImage.gameObject, fadeImage.color.a, 1f, fadeDuration)
                    .setIgnoreTimeScale(true) // funciona aunque Time.timeScale = 0
                    .setOnUpdate((float val) =>
                    {
                        Color c = fadeImage.color;
                        c.a = val;
                        fadeImage.color = c;
                    })
                    .setOnComplete(() =>
                    {
                        SceneManager.LoadScene(sceneName);
                    });
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}