using UnityEngine;

public class DesvanecerSuave : MonoBehaviour
{
    public float duracion = 1.5f; // Duración del desvanecimiento

    private bool iniciandoDesvanecimiento = false;
    private float tiempoTranscurrido = 0f;
    private Vector3 escalaInicial;
    private bool jugadorDentro = false; // Detecta si el jugador está dentro del trigger

    private ContadorMonolitos contador;

    void Start()
    {
        escalaInicial = transform.localScale;
        contador = FindAnyObjectByType<ContadorMonolitos>();

        if (contador == null)
        {
            Debug.LogWarning("No se encontró un ContadorMonolitos en la escena.");
        }
    }

    void Update()
    {
        // Solo inicia el desvanecimiento si el jugador está dentro y presiona E
        if (jugadorDentro && Input.GetKeyDown(KeyCode.E) && !iniciandoDesvanecimiento)
        {
            iniciandoDesvanecimiento = true;
        }

        // Si ya comenzó el desvanecimiento
        if (iniciandoDesvanecimiento)
        {
            tiempoTranscurrido += Time.deltaTime;

            // Reducir escala progresivamente
            float progreso = tiempoTranscurrido / duracion;
            transform.localScale = Vector3.Lerp(escalaInicial, Vector3.zero, progreso);

            // Cuando termina, destruir el objeto
            if (tiempoTranscurrido >= duracion)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Detectar si el jugador entra al área
        if (other.CompareTag("Player"))
            jugadorDentro = true;
    }

    void OnTriggerExit(Collider other)
    {
        // Detectar si el jugador sale del área
        if (other.CompareTag("Player"))
            jugadorDentro = false;
    }
}