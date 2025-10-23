using UnityEngine;

public class Puerta : MonoBehaviour
{
    public Transform pivot;
    public float anguloAbierto = 90f;
    public float velocidadAbierto = 2f;
    public float distanciaInteraccion = 3f;
    public Camera playerCamera;

    private bool estaAbierta = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    private void Start()
    {
        if (pivot == null)
            pivot = transform;


        closedRot = pivot.rotation;
        openRot = pivot.rotation * Quaternion.Euler(0f, anguloAbierto, 0f);

        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(ray, out RaycastHit hit, distanciaInteraccion))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    IntentarAbrir();
                }
            }
        }

        // Rotación suave
        Quaternion targetRot = estaAbierta ? openRot : closedRot;
        pivot.rotation = Quaternion.Slerp(pivot.rotation, targetRot, Time.deltaTime * velocidadAbierto);
    }

    void IntentarAbrir()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.keys > 0)
            {
                GameManager.instance.IntentarUsarLlave(1); // consume 1 llave
                estaAbierta = !estaAbierta;
                Debug.Log("Puerta abierta con una llave");
            }
            else
            {
                Debug.Log("Ups, no tienes llaves ");
            }
        }
        else
        {
            Debug.LogWarning("No hay GameManager en escena.");
        }
    }
}
