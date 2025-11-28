using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Opciones de música al entrar a la escena")]
    public bool detenerMusicaAnterior = true;
    public bool reproducirAmbienteAlEntrar = true;

    private void Start()
    {
        // Asegurarnos de que haya un SoundManager
        if (SoundManager.Instance == null)
        {
            Debug.LogWarning("[GameInitializer] No hay SoundManager en la escena ni persistente. " +
                             "Asegúrate de que se creó en el menú o en el primer nivel.");
            return;
        }

        if (detenerMusicaAnterior)
        {
            SoundManager.Instance.StopMusic();
        }

        if (reproducirAmbienteAlEntrar)
        {
            SoundManager.Instance.PlayAmbientLoop();
        }
    }
}
