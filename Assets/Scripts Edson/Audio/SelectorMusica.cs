using UnityEngine;

public class SelectorMusica : MonoBehaviour
{
    public enum TipoMusica
    {
        MusicaJuego,
        MusicaMuerte,
        MusicaMenu,
        SafeZone,
        MusicaVictoria   // ← AÑADIDO
    }

    [Header("Configuración")]
    public TipoMusica tipoMusica;

    [Tooltip("Reproducir al iniciar la escena")]
    public bool reproducirAlIniciar = true;

    private MusicaFondo musicaFondo;

    private void Awake()
    {
        musicaFondo = FindAnyObjectByType<MusicaFondo>();
        if (musicaFondo == null)
        {
            Debug.LogError("[SelectorMusica] No se encontró el componente MusicaFondo.");
        }
    }

    private void Start()
    {
        musicaFondo.DetenerTodaLaMusica();

        if (reproducirAlIniciar && musicaFondo != null)
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

            case TipoMusica.MusicaVictoria:                 // ← AÑADIDO
                musicaFondo.ReproducirMusicaVictoriaAleatoria();
                break;

            default:
                Debug.LogWarning("[SelectorMusica] Tipo de música no reconocido.");
                break;
        }
    }
}
