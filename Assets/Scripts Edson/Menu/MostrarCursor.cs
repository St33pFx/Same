using UnityEngine;

public class MostrarCursor : MonoBehaviour
{
    void Start()
    {
        // Muestra el cursor
        Cursor.visible = true;

        // Desbloquea el cursor para que se pueda mover libremente
        Cursor.lockState = CursorLockMode.None;
    }
}
