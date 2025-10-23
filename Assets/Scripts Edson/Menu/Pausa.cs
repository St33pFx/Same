using UnityEngine;
using UnityEngine.EventSystems;

public class Pausa : MonoBehaviour
{
    [Header("UI")]
    public GameObject objetoMenuPausa;
    public bool pausa = false;

    [Header("Referencia al script de cámara")]
    public MonoBehaviour scriptMovimientoCamara; // Asigna tu script de cámara en el Inspector

    private void Update()
    {
        // Detectar Escape incluso si Time.timeScale = 0
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pausa) AbrirMenu();
            else CerrarMenu();
        }
    }

    private void AbrirMenu()
    {
        pausa = true;
        objetoMenuPausa.SetActive(true);

        // Pausar el juego
        Time.timeScale = 0f;

        // Cursor visible y desbloqueado
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Desactivar el script de movimiento de cámara
        if (scriptMovimientoCamara != null)
            scriptMovimientoCamara.enabled = false;

        // Pausar música
        //if (AudioManager.Instance != null)
        //{
        //    AudioManager.Instance.PausarMusica();
        //}

        // Seleccionar automáticamente el primer botón del menú (opcional)
        EventSystem.current?.SetSelectedGameObject(objetoMenuPausa);
    }

    public void CerrarMenu()
    {
        pausa = false;
        objetoMenuPausa.SetActive(false);

        // Reanudar el juego
        Time.timeScale = 1f;

        // Cursor oculto y bloqueado
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Reactivar el script de movimiento de cámara
        if (scriptMovimientoCamara != null)
            scriptMovimientoCamara.enabled = true;

        // Reanudar música
        //if (AudioManager.Instance != null)
        //{
        //    AudioManager.Instance.ReanudarMusica();
        //}

        // Deseleccionar botón actual para evitar problemas de UI
        EventSystem.current?.SetSelectedGameObject(null);
    }
}