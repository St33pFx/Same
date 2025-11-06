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
            iconoUI.SetActive(false);
    }

    private void Update()
    {
        // Si el jugador está dentro y presiona la tecla
        if (jugadorDentro && Input.GetKeyDown(teclaInteraccion))
        {
            OcultarIcono();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
            MostrarIcono();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
            OcultarIcono();
        }
    }

    // 🔹 Esto se ejecuta automáticamente si el objeto es destruido o desactivado
    private void OnDisable()
    {
        OcultarIcono();
        jugadorDentro = false;
    }

    private void MostrarIcono()
    {
        if (iconoUI != null && !iconoUI.activeSelf)
            iconoUI.SetActive(true);
    }

    private void OcultarIcono()
    {
        if (iconoUI != null && iconoUI.activeSelf)
            iconoUI.SetActive(false);
    }
}