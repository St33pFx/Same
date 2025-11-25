using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuNotas : MonoBehaviour
{
    [Header("Botones de notas")]
    public Button[] botonesNotas;

    [Header("Contenido de notas")]
    public string[] textosNotas;

    [Header("UI del contenido")]
    public TextMeshProUGUI textoNotaUI;

    private void OnEnable()
    {
        // Suscribirse al evento de notas desbloqueadas
        NotasManager.Instance.OnNotaDesbloqueada += ActivarBotonDeNota;

        // Configurar los botones SOLO con la info actual
        InicializarBotones();
    }

    private void OnDisable()
    {
        if (NotasManager.Instance != null)
            NotasManager.Instance.OnNotaDesbloqueada -= ActivarBotonDeNota;
    }

    private void InicializarBotones()
    {
        for (int i = 0; i < botonesNotas.Length; i++)
        {
            Button boton = botonesNotas[i];
            if (boton == null) continue;

            boton.onClick.RemoveAllListeners();

            // Si la nota NO está desbloqueada → desactivar interacción
            bool desbloqueada = NotasManager.Instance.NotaEstaDesbloqueada(i);
            boton.interactable = desbloqueada;

            if (desbloqueada)
            {
                int id = i;
                boton.onClick.AddListener(() => MostrarNota(id));
            }
        }
    }

    private void ActivarBotonDeNota(int id)
    {
        if (id < 0 || id >= botonesNotas.Length) return;

        Button boton = botonesNotas[id];
        boton.interactable = true;

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