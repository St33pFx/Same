using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MonolitoCambioEscena : MonoBehaviour
{
    [Header("Nombre de la escena a cargar")]
    public string sceneName; // Nombre de la escena a cargar

    [Header("Duración del fade (segundos)")]
    public float fadeDuration = 1.5f; // Tiempo que tarda el fade antes del cambio

    [Header("Imagen para el fade in")]
    public Image fadeImage; // Imagen del UI con alfa inicial en 0

    private bool playerInTrigger = false;
    private bool sceneChangeStarted = false;
    private GameObject player;

    void OnTriggerEnter(Collider other)
    {
        // Detectar si el jugador entra al área
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            player = other.gameObject; // Guardar referencia al jugador
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Detectar si el jugador sale del área
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            player = null;
        }
    }

    void Update()
    {
        // Iniciar el cambio de escena con fade al presionar E
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E) && !sceneChangeStarted)
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                sceneChangeStarted = true;

                // Desactivar el PlayerController si existe
                if (player != null)
                {
                    MonoBehaviour playerController = player.GetComponent<MonoBehaviour>();
                    foreach (var comp in player.GetComponents<MonoBehaviour>())
                    {
                        if (comp.GetType().Name == "PlayerController")
                        {
                            comp.enabled = false;
                            Debug.Log("PlayerController desactivado antes del cambio de escena.");
                            break;
                        }
                    }
                }

                StartCoroutine(FadeAndChangeScene());
            }
        }
    }

    IEnumerator FadeAndChangeScene()
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            float tiempo = 0f;

            // Aumentar el alfa suavemente hasta 1
            while (tiempo < fadeDuration)
            {
                tiempo += Time.deltaTime;
                color.a = Mathf.Lerp(0f, 1f, tiempo / fadeDuration);
                fadeImage.color = color;
                yield return null;
            }
        }

        // Cambiar la escena justo al terminar el fade
        SceneManager.LoadScene(sceneName);
    }
}
