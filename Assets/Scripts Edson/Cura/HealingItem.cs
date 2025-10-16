using UnityEngine;

public class HealingItem : MonoBehaviour
{
    [Header("Referencia al controlador HDRP del jugador")]
    public PostHDRPController postController;

    [Header("Configuración")]
    public KeyCode BotonAPresionar = KeyCode.E;

    private bool playerNearby = false;

    private void Update()
    {
        // Detectar si el jugador está cerca y presiona la tecla
        if (playerNearby && Input.GetKeyDown(BotonAPresionar))
        {
            if (postController != null)
            {
                postController.ApplyHealing(); // Aplica la curación rápida
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
