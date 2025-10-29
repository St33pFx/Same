using UnityEngine;

public class MonolithFadeZone : MonoBehaviour
{
    [Header("Referencia al controlador HDRP")]
    public PostHDRPController postController;

    [Header("Jugador")]
    public string playerTag = "Player"; // Etiqueta del jugador

    private bool playerInside = false;

    private void Awake()
    {
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogError("Debes tener un collider en este GameObject.");
            return;
        }

        col.isTrigger = true; // Asegurarse que es trigger
    }

    private void OnTriggerEnter(Collider other)
    {
        if (postController == null || postController.IsHealing) return;

        if (other.CompareTag(playerTag))
        {
            playerInside = true;
            postController.StartFadeIn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (postController == null) return;

        if (other.CompareTag(playerTag))
        {
            playerInside = false;
            postController.StartFadeOutWithDelay();
        }
    }

    public void ResetFadeState()
    {
        playerInside = false;
    }
}