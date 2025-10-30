using UnityEngine;

public class MostrarBoton : MonoBehaviour
{
    [Header("Objeto de UI a mostrar")]
    public GameObject iconoUI;

    [Header("Tecla de interacción")]
    public KeyCode teclaInteraccion = KeyCode.E;

    private bool jugadorDentro = false;

    private void Start()
    {
        if (iconoUI != null)
            iconoUI.SetActive(false); // Ocultar al inicio
    }

    private void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(teclaInteraccion))
        {
            if (iconoUI != null)
            {
                iconoUI.SetActive(false); // Desactivar al presionar E
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && iconoUI != null)
        {
            jugadorDentro = true;
            iconoUI.SetActive(true); // Mostrar icono al entrar
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && iconoUI != null)
        {
            jugadorDentro = false;
            iconoUI.SetActive(false); // Ocultar icono al salir
        }
    }
}