using UnityEngine;

public class MonolitoCordura : MonoBehaviour
{
    [Header("Referencia a Cordura")]
    public Cordura cordura;

    [Header("Referencia a CorduraVisual")]
    public CorduraVisual visualCordura;

    [Header("Configuración")]
    public float perdidaPorSegundo = 5f;

    [HideInInspector] public bool jugadorDentro = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && cordura != null)
        {
            cordura.PerderCordura(perdidaPorSegundo * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
            if (visualCordura != null)
                visualCordura.JugadorDentroMonolito(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
            if (visualCordura != null)
                visualCordura.JugadorDentroMonolito(false);
        }
    }
}