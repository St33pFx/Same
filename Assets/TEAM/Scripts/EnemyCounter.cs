using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCounter : MonoBehaviour
{
    [Header("Configuración")]
    public int objetivoMuertes = 5;           // Cuántos enemigos debes matar
    public string escenaGameOver = "Game Over"; // Nombre de la escena a cargar

    private int muertesActuales = 0;

    // Llamar este método cuando un enemigo muera
    public void RegistrarMuerte()
    {
        muertesActuales++;

        Debug.Log("Enemigos eliminados: " + muertesActuales);

        if (muertesActuales >= objetivoMuertes)
        {
            SceneManager.LoadScene(escenaGameOver);
        }
    }
}
