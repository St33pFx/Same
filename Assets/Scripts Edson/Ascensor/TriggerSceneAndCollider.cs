using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TriggerSceneAndCollider : MonoBehaviour
{
    [Header("Nombre de la escena a cargar")]
    public string sceneName; // Nombre de la escena a cargar

    [Header("Collider a activar")]
    public Collider colliderToActivate; // Collider que se activará

    [Header("Tiempo antes de cambiar de escena (segundos)")]
    public float delayBeforeSceneChange = 2f; // Tiempo de espera antes de cambiar de escena

    private bool playerInTrigger = false;
    private bool sceneChangeStarted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            Debug.Log("Jugador dentro del trigger");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            Debug.Log("Jugador salió del trigger");
        }
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E) && !sceneChangeStarted)
        {
            // Activar el collider si se ha asignado
            if (colliderToActivate != null)
            {
                colliderToActivate.enabled = true;
                Debug.Log("Collider activado");
            }

            // Iniciar cambio de escena después del delay
            if (!string.IsNullOrEmpty(sceneName))
            {
                sceneChangeStarted = true;
                StartCoroutine(ChangeSceneAfterDelay());
            }
        }
    }

    private IEnumerator ChangeSceneAfterDelay()
    {
        Debug.Log("Esperando " + delayBeforeSceneChange + " segundos antes de cambiar de escena...");
        yield return new WaitForSeconds(delayBeforeSceneChange);
        SceneManager.LoadScene(sceneName);
        Debug.Log("Cambiando a la escena: " + sceneName);
    }
}