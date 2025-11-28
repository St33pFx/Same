using UnityEngine;

public class SelectorMusica : MonoBehaviour
{
    public enum TipoMusica
    {
        MusicaJuego,
        MusicaMuerte,
        MusicaMenu,
        SafeZone
    }

    [Header("Configuración")]
    public TipoMusica tipoMusica;

    [Tooltip("Reproducir al iniciar la escena")]
    public bool reproducirAlIniciar = true;

    private MusicaFondo musicaFondo;

    //  Ya no usamos Awake para buscar el MusicaFondo

    private void Start()
    {
        //  Aquí ya se ejecutaron todos los Awake, incluido el de MusicaFondo
        musicaFondo = MusicaFondo.Instance;

        if (musicaFondo == null)
        {
            Debug.LogError("[SelectorMusica] No se encontró MusicaFondo. Asegúrate de tener uno en la primera escena.");
            return;
        }

        // Primero apagamos lo que haya sonando (por ejemplo, música del menú)
        musicaFondo.DetenerTodaLaMusica();

        if (reproducirAlIniciar)
        {
            ReproducirMusicaSeleccionada();
        }
    }

    [ContextMenu("Reproducir Música Seleccionada")]
    public void ReproducirMusicaSeleccionada()
    {
        if (musicaFondo == null) return;

        switch (tipoMusica)
        {
            case TipoMusica.MusicaJuego:
                musicaFondo.ReproducirMusicaAleatoria();
                break;

            case TipoMusica.MusicaMuerte:
                musicaFondo.ReproducirMusicaMuerteAleatoria();
                break;

            case TipoMusica.MusicaMenu:
                musicaFondo.ReproducirMusicaMenuAleatoria();
                break;

            case TipoMusica.SafeZone:
                musicaFondo.ReproducirMusicaSafezoneAleatoria();
                break;

            default:
                Debug.LogWarning("[SelectorMusica] Tipo de música no reconocido.");
                break;
        }
    }
}
