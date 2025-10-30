using UnityEngine;

public class CuracionCordura : MonoBehaviour
{
    [Header("Referencia a la cordura")]
    public Cordura cordura;

    [Header("Configuración")]
    public float cantidadCuracion = 50f;
    public KeyCode BotonAPresionar = KeyCode.E;

    private bool jugadorDentro = false;

    void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(BotonAPresionar))
        {
            if (cordura != null)
            {
                cordura.CurarCordura(cantidadCuracion);
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorDentro = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorDentro = false;
    }
}