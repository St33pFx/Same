using UnityEngine;
using System.Collections;

public class DesvanecerObjeto : MonoBehaviour
{
    [Header("Configuración del desvanecimiento")]
    public float duracion = 1.5f; // Tiempo total del fade
    public bool destruirDespues = true; // Si se destruye o solo se desactiva
    public bool forzarTransparencia = true; // Asegura que el material soporte transparencia

    private Renderer[] renderers;
    private bool enProceso = false;
    private bool jugadorDentro = false;

    void Awake()
    {
        // Obtener todos los renderers del objeto y sus hijos
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        // Si el jugador está dentro y presiona E, iniciar el desvanecimiento
        if (jugadorDentro && Input.GetKeyDown(KeyCode.E))
        {
            IniciarDesvanecimiento();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detectar si el jugador entra al trigger
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Detectar si el jugador sale del trigger
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
        }
    }

    public void IniciarDesvanecimiento()
    {
        if (!enProceso)
            StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        enProceso = true;

        // Guardar materiales y colores originales
        Material[] materiales = new Material[renderers.Length];
        Color[] coloresIniciales = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            materiales[i] = renderers[i].material;
            coloresIniciales[i] = materiales[i].color;

            // Forzar modo transparente si es necesario
            if (forzarTransparencia)
                CambiarAModoTransparente(materiales[i]);
        }

        float tiempo = 0f;
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, tiempo / duracion);

            for (int i = 0; i < materiales.Length; i++)
            {
                Color c = coloresIniciales[i];
                c.a = alpha;
                materiales[i].color = c;
            }

            yield return null;
        }

        Finalizar();
    }

    private void Finalizar()
    {
        if (destruirDespues)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }

    // Cambia el material al modo transparente para permitir el fade
    private void CambiarAModoTransparente(Material mat)
    {
        if (mat == null) return;

        mat.SetFloat("_Mode", 3);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
    }
}
