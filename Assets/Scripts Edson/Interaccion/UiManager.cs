using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Referencias de UI")]
    public GameObject botonInteraccion; // Asigna aquí tu icono de "Presiona E"

    private void Awake()
    {
        // Singleton: asegura que solo exista un UIManager en escena
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Opcional: mantener el UIManager entre escenas
        // DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (botonInteraccion != null)
            botonInteraccion.SetActive(false);
        else
            Debug.LogWarning("UIManager: No se asignó el botón de interacción en el inspector.");
    }
}
