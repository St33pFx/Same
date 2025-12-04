using UnityEngine;

public class CuracionCordura : MonoBehaviour
{
    [Header("Referencia a la cordura")]
    public Cordura cordura;

    [Header("Configuración")]
    public float cantidadCuracion = 50f;
    public KeyCode BotonAPresionar = KeyCode.E;

    private bool jugadorDentro = false;

    private void Awake()
    {
        // Buscar automáticamente Cordura si no está asignado
        if (cordura == null)
        {
            cordura = FindObjectOfType<Cordura>();
            if (cordura == null)
                Debug.LogError("❌ No se encontró 'Cordura' en la escena.");
        }
    }

    void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(BotonAPresionar))
        {
            if (cordura != null)
            {
                cordura.CurarCordura(cantidadCuracion);
            }

            // Destruir el objeto curativo después de usarlo
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