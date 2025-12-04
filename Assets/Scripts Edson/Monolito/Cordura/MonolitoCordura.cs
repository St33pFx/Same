using UnityEngine;

public class MonolitoCordura : MonoBehaviour
{
    [Header("Referencia a Cordura")]
    public Cordura cordura;

    [Header("Referencia a CorduraVisual")]
    public CorduraVisual visualCordura;

    [Header("Configuración")]
    public float perdidaPorSegundo = 5f;

    private void Awake()
    {
        // Buscar Cordura automáticamente si no está asignada
        if (cordura == null)
            cordura = FindObjectOfType<Cordura>();

        // Buscar CorduraVisual automáticamente si no está asignada
        if (visualCordura == null)
        {
            visualCordura = FindObjectOfType<CorduraVisual>();
            if (visualCordura == null)
            {
                Debug.LogWarning("No se encontró CorduraVisual en la escena. Las imágenes no se mostrarán.");
            }
            else
            {
                visualCordura.cordura = cordura;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (cordura != null)
        {
            cordura.PerderCordura(perdidaPorSegundo * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (visualCordura != null)
            visualCordura.JugadorDentroMonolito(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (visualCordura != null)
            visualCordura.JugadorDentroMonolito(false);
    }
}