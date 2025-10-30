using UnityEngine;

public class ApagarLinterna : MonoBehaviour
{
    [Header("Objeto a alternar")]
    public GameObject objeto;

    [Header("Tecla para alternar")]
    public KeyCode teclaAlternar = KeyCode.Q;

    private void Update()
    {
        if (Input.GetKeyDown(teclaAlternar) && objeto != null)
        {
            objeto.SetActive(!objeto.activeSelf); // Cambia al estado contrario
        }
    }
}
