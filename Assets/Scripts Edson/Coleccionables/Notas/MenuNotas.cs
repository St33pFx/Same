using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MenuNotas : MonoBehaviour
{
    [Header("Botones de notas")]
    public Button[] botonesNotas;

    [Header("Títulos de notas (lo que ven los botones)")]
    public string[] titulosNotas;

    [Header("Contenido de notas (lo que aparece al abrirlas)")]
    public string[] textosNotas;

    [Header("UI del contenido")]
    public TextMeshProUGUI textoNotaUI;

    [Header("Símbolo de nota bloqueada")]
    public string simboloBloqueado = "???";

    private void OnEnable()
    {
        StartCoroutine(EsperarNotasManager());
    }

    private void OnDisable()
    {
        if (NotasManager.Instance != null)
            NotasManager.Instance.OnNotaDesbloqueada -= ActivarBotonDeNota;
    }

    private IEnumerator EsperarNotasManager()
    {
        while (NotasManager.Instance == null)
            yield return null;

        NotasManager.Instance.OnNotaDesbloqueada += ActivarBotonDeNota;
        InicializarBotones();
    }

    private void InicializarBotones()
    {
        for (int i = 0; i < botonesNotas.Length; i++)
        {
            Button boton = botonesNotas[i];
            if (boton == null) continue;

            boton.onClick.RemoveAllListeners();

            bool desbloqueada = NotasManager.Instance.NotaEstaDesbloqueada(i);
            boton.interactable = desbloqueada;

            TextMeshProUGUI botonTexto = boton.GetComponentInChildren<TextMeshProUGUI>();
            if (botonTexto != null)
            {
                botonTexto.text = desbloqueada
                    ? titulosNotas[i]
                    : simboloBloqueado;
            }

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
        if (boton == null) return;

        boton.interactable = true;

        TextMeshProUGUI botonTexto = boton.GetComponentInChildren<TextMeshProUGUI>();
        if (botonTexto != null)
            botonTexto.text = titulosNotas[id];

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