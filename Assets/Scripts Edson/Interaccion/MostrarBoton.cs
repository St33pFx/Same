using UnityEngine;

public class MostrarBoton : MonoBehaviour
{
    [Header("Tecla de interacción")]
    public KeyCode teclaInteraccion = KeyCode.E;

    private GameObject iconoUI;
    private bool jugadorDentro = false;

    private void Start()
    {
        // Obtiene la referencia desde el UIManager
        iconoUI = UIManager.Instance != null ? UIManager.Instance.botonInteraccion : null;

        if (iconoUI == null)
            Debug.LogWarning("MostrarBoton: No se encontró el botón de interacción desde UIManager.");
    }

    private void Update()
    {
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