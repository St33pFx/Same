using UnityEngine;
using UnityEngine.SceneManagement;

public class ContadorMonolitos : MonoBehaviour
{
    [Header("Cantidad de monolitos recogidos")]
    public float monolitos = 0f;

    public void SumarMonolito()
    {
        monolitos++;
        Debug.Log("Monolitos recogidos: " + monolitos);
    }

    private void Update()
    {
        if (monolitos >= 10f) SceneManager.LoadScene("Menu");
    }
}
