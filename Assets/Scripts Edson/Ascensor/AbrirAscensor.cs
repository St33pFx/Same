using UnityEngine;

public class AbrirAscensor : MonoBehaviour
{
    [Header("Configuración del trigger")]
    public string playerTag = "Player"; // Tag del jugador
    public KeyCode activationKey = KeyCode.E; // Tecla para activar
    public ElevatorDoorController doorController; // Referencia al script de las puertas

    private bool playerInside = false; // Detecta si el jugador está dentro del área

    void OnTriggerEnter(Collider other)
    {
        // Si entra el jugador al área
        if (other.CompareTag(playerTag))
        {
            playerInside = true;
            Debug.Log("Jugador dentro del ascensor. Presiona E para activar.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Si el jugador sale del área
        if (other.CompareTag(playerTag))
        {
            playerInside = false;
            Debug.Log("Jugador salió del area.");
        }
    }

    void Update()
    {
        // Si el jugador está dentro y presiona E
        if (playerInside && Input.GetKeyDown(activationKey))
        {
            Debug.Log("Tecla E presionada dentro del ascensor.");
            if (doorController != null)
            {
                doorController.ActivarPuertas(); // Llama al script de las puertas
            }
            else
            {
                Debug.LogWarning("No hay un ElevatorDoorController asignado en el inspector.");
            }
        }
    }
}
