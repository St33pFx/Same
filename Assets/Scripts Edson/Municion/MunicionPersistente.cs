using UnityEngine;

public class MunicionPersistente : MonoBehaviour
{
    public static MunicionPersistente Instance;

    public int municionActual = 0;

    private void Awake()
    {
        // Singleton: asegura que solo exista uno
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AgregarMunicion(int cantidad)
    {
        municionActual += cantidad;
    }

    public void UsarMunicion(int cantidad)
    {
        municionActual = Mathf.Max(0, municionActual - cantidad);
    }
}
