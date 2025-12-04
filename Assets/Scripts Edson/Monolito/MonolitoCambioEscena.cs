using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MonolitoCambioEscena : MonoBehaviour
{
    [Header("Nombre de la escena a cargar")]
    public string sceneName;

    [Header("Duración del fade (segundos)")]
    public float fadeDuration = 1.5f;

    [Header("Imagen para el fade in")]
    public Image fadeImage;

    private bool playerInTrigger = false;
    private bool sceneChangeStarted = false;
    private GameObject player;

    private void Awake()
    {
        // Buscar automáticamente la imagen con el tag "Fade" si no está asignada
        if (fadeImage == null)
        {
            GameObject fadeObj = GameObject.FindGameObjectWithTag("Fade");
            if (fadeObj != null)
                fadeImage = fadeObj.GetComponent<Image>();
            else
                Debug.LogWarning("No se encontró un objeto con tag 'Fade' en la escena.");
        }

        // Inicializar alfa en 0 y activar la imagen
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            player = null;
        }
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E) && !sceneChangeStarted)
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                sceneChangeStarted = true;

                // Desactivar PlayerController y Shooter si existen
                if (player != null)
                {
                    var playerController = player.GetComponent<MonoBehaviour>();
                    var shooter = player.GetComponent<MonoBehaviour>();

                    foreach (var comp in player.GetComponents<MonoBehaviour>())
                    {
                        if (comp.GetType().Name == "PlayerController" || comp.GetType().Name == "Shooter")
                        {
                            comp.enabled = false;
                        }
                    }
                }

                // Ejecutar fade completo con LeanTween y TimeScale ignorado
                if (fadeImage != null)
                {
                    LeanTween.value(fadeImage.gameObject, fadeImage.color.a, 1f, fadeDuration)
                        .setIgnoreTimeScale(true) // Permite que funcione aunque Time.timeScale = 0
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
}