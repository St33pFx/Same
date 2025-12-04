using UnityEngine;
using UnityEngine.EventSystems;

public class Pausa : MonoBehaviour
{
    [Header("UI")]
    public GameObject objetoMenuPausa;
    public bool pausa = false;

    private PlayerController scriptMovimiento; // Script de movimiento del jugador

    private void Start()
    {
        // Buscar automáticamente el script de movimiento en la escena
        scriptMovimiento = FindObjectOfType<PlayerController>();

        if (scriptMovimiento == null)
            Debug.LogWarning("No se encontró PlayerController en la escena.");
    }

    private void Update()
    {
        // Solo abre el menú con Esc
        if (Input.GetKeyDown(KeyCode.Escape) && !pausa)
        {
            AbrirMenu();
        }
    }

    private void AbrirMenu()
    {
        pausa = true;
        objetoMenuPausa.SetActive(true);

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (scriptMovimiento != null)
        {
            // Desactivar movimiento
            scriptMovimiento.enabled = false;

            // Desactivar disparo
            Shooter shooter = scriptMovimiento.GetComponent<Shooter>();
            if (shooter != null)
                shooter.enabled = false;
        }

        EventSystem.current?.SetSelectedGameObject(objetoMenuPausa);
    }

    public void CerrarMenu()
    {
        pausa = false;
        objetoMenuPausa.SetActive(false);

        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (scriptMovimiento != null)
        {
            // Reactivar movimiento
            scriptMovimiento.enabled = true;

            // Reactivar disparo
            Shooter shooter = scriptMovimiento.GetComponent<Shooter>();
            if (shooter != null)
                shooter.enabled = true;
        }

        EventSystem.current?.SetSelectedGameObject(null);
    }
}