using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeEscena : MonoBehaviour
{
    [Header("Escena a cargar")]

    [SerializeField]
    private string escena;

    // Cambiar a la escena indicada
    public void Escena()
    {
        // Asegurarse de reanudar el tiempo
        Time.timeScale = 1;

        // Cargar la escena
        SceneManager.LoadScene(escena);
    }

    // Salir del juego
    public void Salir()
    {
        Application.Quit();
    }
}
