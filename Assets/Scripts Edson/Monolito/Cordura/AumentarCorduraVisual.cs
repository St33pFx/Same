using UnityEngine;
using UnityEngine.UI;

public class AumentarCorduraVisual : MonoBehaviour
{
    [System.Serializable]
    public class ImagenCordura
    {
        public Image imagen;
        [Range(0f, 1f)] public float alfaObjetivo = 1f;
    }

    [Header("Imágenes con su alfa individual")]
    public ImagenCordura[] imagenesCordura;

    [Header("Duración del cambio de alfa")]
    public float duracion = 1.5f; // Tiempo total de transición

    [Header("Tecla de activación")]
    public KeyCode tecla = KeyCode.E;

    private bool jugadorDentro = false;
    private bool activando = false;
    private bool yaActivado = false; // Evita reactivar el efecto
    private float tiempoTranscurrido = 0f;
    private Color[] alfasIniciales;

    void Start()
    {
        // Guardar el color inicial de cada imagen
        alfasIniciales = new Color[imagenesCordura.Length];
        for (int i = 0; i < imagenesCordura.Length; i++)
        {
            if (imagenesCordura[i].imagen != null)
                alfasIniciales[i] = imagenesCordura[i].imagen.color;
        }
    }

    void Update()
    {
        // Solo se activa una vez, si el jugador está dentro y no se ha activado aún
        if (jugadorDentro && Input.GetKeyDown(tecla) && !yaActivado)
        {
            activando = true;
            tiempoTranscurrido = 0f;
            yaActivado = true; // Marca que ya se ejecutó
        }

        // Si el efecto está activo, hacer la interpolación de alfa
        if (activando)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = Mathf.Clamp01(tiempoTranscurrido / duracion);

            for (int i = 0; i < imagenesCordura.Length; i++)
            {
                if (imagenesCordura[i].imagen == null) continue;

                Color color = alfasIniciales[i];
                color.a = Mathf.Lerp(alfasIniciales[i].a, imagenesCordura[i].alfaObjetivo, t);
                imagenesCordura[i].imagen.color = color;
            }

            // Terminar transición
            if (t >= 1f)
            {
                activando = false;
            }
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