using UnityEngine;

public class MenuOpciones : MonoBehaviour
{
    [Header("Objeto del menú de opciones")]
    public GameObject menuOpcionesObj;

    private bool opcionesAbiertas = false;

    void Start()
    {
        menuOpcionesObj.SetActive(false);
    }

    void Update()
    {
        if (opcionesAbiertas && Input.GetKeyDown(KeyCode.Escape))
        {
            CerrarOpciones();
        }
    }

    public void AbrirOpciones()
    {
        opcionesAbiertas = true;
        menuOpcionesObj.SetActive(true);
    }

    public void CerrarOpciones()
    {
        opcionesAbiertas = false;
        menuOpcionesObj.SetActive(false);
    }
}
