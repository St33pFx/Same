using UnityEngine;

public class MusicaFondo : MonoBehaviour
{
    private audio musicaActual;
    private AudioSource sourceActual;
    private int indiceAnterior = -1;

    private AudioManager manager => AudioManager.instance;

    private audio[] ObtenerLista(string tipo)
    {
        if (manager == null) return null;

        return tipo switch
        {
            "musica" => manager.musica,
            "musicaMuerte" => manager.musicaMuerte,
            "musicaMenu" => manager.musicaMenu,
            "safeZone" => manager.safeZone,
            "musicaVictoria" => manager.musicaVictoria,   // ← AÑADIDO
            _ => null
        };
    }

    private void DetenerMusicaActual()
    {
        if (sourceActual != null && sourceActual.isPlaying)
            sourceActual.Stop();
    }

    private void ReproducirAudio(audio[] lista)
    {
        if (lista == null || lista.Length == 0)
        {
            Debug.LogWarning("[MusicaFondo] Lista vacía.");
            return;
        }

        int indice = Random.Range(0, lista.Length);

        // Evitar repetición consecutiva
        if (lista.Length > 1)
        {
            while (indice == indiceAnterior)
                indice = Random.Range(0, lista.Length);
        }
        indiceAnterior = indice;

        audio nuevaMusica = lista[indice];

        // Detener la pista actual
        DetenerMusicaActual();

        // Asignar y reproducir
        musicaActual = nuevaMusica;
        sourceActual = musicaActual.source;
        sourceActual.Play();

        Debug.Log($"[MusicaFondo] Reproduciendo: {musicaActual.nombre}");
    }

    public void ReproducirMusicaAleatoria() => ReproducirAudio(ObtenerLista("musica"));
    public void ReproducirMusicaMuerteAleatoria() => ReproducirAudio(ObtenerLista("musicaMuerte"));
    public void ReproducirMusicaMenuAleatoria() => ReproducirAudio(ObtenerLista("musicaMenu"));
    public void ReproducirMusicaSafezoneAleatoria() => ReproducirAudio(ObtenerLista("safeZone"));

    public void ReproducirMusicaVictoriaAleatoria() =>      // ← AÑADIDO
        ReproducirAudio(ObtenerLista("musicaVictoria"));

    public void DetenerTodaLaMusica()
    {
        if (manager == null)
        {
            Debug.LogWarning("[MusicaFondo] AudioManager no encontrado.");
            return;
        }

        audio[][] listas = {
            manager.musica,
            manager.musicaMuerte,
            manager.musicaMenu,
            manager.safeZone,
            manager.musicaVictoria      // ← AÑADIDO
        };

        foreach (var lista in listas)
        {
            if (lista == null) continue;
            foreach (var pista in lista)
                pista.source.Stop();
        }

        musicaActual = null;
        sourceActual = null;
        indiceAnterior = -1;
    }
}

