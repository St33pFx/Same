using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MenuNotas : MonoBehaviour
{
    [Header("Botones de notas")]
    public Button[] botonesNotas;

    [Header("Contenido de notas")]
    public string[] textosNotas;

    [Header("UI del contenido")]
    public TextMeshProUGUI textoNotaUI;

    [Header("Símbolo de nota bloqueada")]
    public string simboloBloqueado = "???";

    private void OnEnable()
    {
        // Iniciar la espera para asegurar que NotasManager esté listo
        StartCoroutine(EsperarNotasManager());
    }

    private void OnDisable()
    {
        // Desuscribirse del evento para evitar referencias fantasma
        if (NotasManager.Instance != null)
            NotasManager.Instance.OnNotaDesbloqueada -= ActivarBotonDeNota;
    }

    private IEnumerator EsperarNotasManager()
    {
        // Esperar hasta que exista el singleton
        while (NotasManager.Instance == null)
            yield return null;

        // Suscribirse al evento de notas desbloqueadas
        NotasManager.Instance.OnNotaDesbloqueada += ActivarBotonDeNota;

        // Inicializar los botones con el estado actual
        InicializarBotones();
    }

    private void InicializarBotones()
    {
        for (int i = 0; i < botonesNotas.Length; i++)
        {
            Button boton = botonesNotas[i];
            if (boton == null) continue;

            boton.onClick.RemoveAllListeners();

            // Solo activar interacción si la nota ya está desbloqueada
            bool desbloqueada = NotasManager.Instance.NotaEstaDesbloqueada(i);
            boton.interactable = desbloqueada;

            TextMeshProUGUI botonTexto = boton.GetComponentInChildren<TextMeshProUGUI>();
            if (botonTexto != null)
            {
                botonTexto.text = desbloqueada ? textosNotas[i] : simboloBloqueado;
            }

            if (desbloqueada)
            {
                int id = i; // Capturar la variable correctamente
                boton.onClick.AddListener(() => MostrarNota(id));
            }
        }
    }

    private void ActivarBotonDeNota(int id)
    {
        if (id < 0 || id >= botonesNotas.Length) return;

        Button boton = botonesNotas[id];
        if (boton == null) return;

        boton.interactable = true;

        // Cambiar el texto de signo de interrogación al contenido real
        TextMeshProUGUI botonTexto = boton.GetComponentInChildren<TextMeshProUGUI>();
        if (botonTexto != null && id < textosNotas.Length)
            botonTexto.text = textosNotas[id];

        // Limpiar listeners anteriores y asignar el click
        boton.onClick.RemoveAllListeners();
        boton.onClick.AddListener(() => MostrarNota(id));
    }

    public void MostrarNota(int id)
    {
        if (textoNotaUI == null) return;
        if (id < 0 || id >= textosNotas.Length) return;

        textoNotaUI.text = textosNotas[id];
    }
}