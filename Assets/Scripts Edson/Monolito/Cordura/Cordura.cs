using UnityEngine;

public class Cordura : MonoBehaviour
{
    [Header("Cordura")]
    public float corduraMaxima = 100f;
    public float corduraActual = 100f;

    void Start()
    {
        corduraActual = Mathf.Clamp(corduraActual, 0f, corduraMaxima);
    }

    public void PerderCordura(float cantidad)
    {
        corduraActual -= cantidad;
        corduraActual = Mathf.Clamp(corduraActual, 0f, corduraMaxima);
    }

    public void CurarCordura(float cantidad)
    {
        corduraActual += cantidad;
        corduraActual = Mathf.Clamp(corduraActual, 0f, corduraMaxima);
    }

    public float ObtenerPorcentaje()
    {
        return corduraActual / corduraMaxima;
    }
}