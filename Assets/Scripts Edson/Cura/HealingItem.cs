using UnityEngine;

public class HealingItem : MonoBehaviour
{
    [Header("Referencia al controlador de fade")]
    public MonolithCanvasFade fadeController;

    [Header("Configuración")]
    public KeyCode BotonAPresionar = KeyCode.E;

    private bool playerNearby = false;

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(BotonAPresionar))
        {
            if (fadeController != null)
            {
                fadeController.ApplyHealing(); // Inicia la curación independiente
            }

            // Destruir el objeto curativo después de usarlo
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("Cura"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("Cura"))
        {
            playerNearby = false;
        }
    }
}
