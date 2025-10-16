using UnityEngine;

public class FadeTriggerZone : MonoBehaviour
{
    [Header("Referencia al controlador HDRP")]
    public PostHDRPController postController;

    private CapsuleCollider col;
    private bool playerInside = false; // Para detectar cambios de estado

    private void Start()
    {
        col = GetComponent<CapsuleCollider>();
        col.isTrigger = true; // Asegurarse de que sea un trigger
    }

    private void Update()
    {
        if (postController == null || col == null)
            return;

        // Obtener todos los colliders que estén dentro de la cápsula
        Collider[] hits = Physics.OverlapCapsule(
            transform.position + Vector3.up * (col.height / 2),   // Punto superior de la cápsula
            transform.position - Vector3.up * (col.height / 2),   // Punto inferior de la cápsula
            col.radius
        );

        bool isInside = false;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                isInside = true;
                break;
            }
        }

        // Detectar cambios de estado
        if (isInside && !playerInside)
        {
            playerInside = true;
            postController.StartFadeIn();
        }
        else if (!isInside && playerInside)
        {
            playerInside = false;
            postController.StartFadeOutWithDelay(); // Fade de salida con retraso
        }
    }
    
}