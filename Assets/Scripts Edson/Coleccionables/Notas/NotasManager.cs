using UnityEngine;
using System;

public class NotasManager : MonoBehaviour
{
    public static NotasManager Instance;

    [Header("Notas desbloqueadas")]
    public bool[] notasDesbloqueadas;

    // Evento que se dispara cuando una nota se desbloquea
    public event Action<int> OnNotaDesbloqueada;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DesbloquearNota(int id)
    {
        if (!notasDesbloqueadas[id])
        {
            notasDesbloqueadas[id] = true;

            // Avisar al menú
            OnNotaDesbloqueada?.Invoke(id);
        }
    }

    public bool NotaEstaDesbloqueada(int id)
    {
        return notasDesbloqueadas[id];
    }
}