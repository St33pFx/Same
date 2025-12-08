using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Listas de Audio")]
    public audio[] sonido;
    public audio[] musica;
    public audio[] musicaMuerte;
    public audio[] musicaMenu;
    public audio[] safeZone;
    public audio[] musicaVictoria;   // ← AÑADIDO
    public audio[] ruidosZombie;
    public audio[] dañoZombies;
    public audio[] muerteZombies;

    [Header("Volúmenes")]
    [Range(0f, 1f)] public float volumenEfectos = 1f;
    [Range(0f, 1f)] public float volumenMusica = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        // Inicializar todas las listas automáticamente
        InicializarAudios(sonido, volumenEfectos);
        InicializarAudios(musica, volumenMusica);
        InicializarAudios(musicaMuerte, volumenMusica);
        InicializarAudios(musicaMenu, volumenMusica);
        InicializarAudios(safeZone, volumenMusica);
        InicializarAudios(musicaVictoria, volumenMusica);   // ← AÑADIDO
        InicializarAudios(ruidosZombie, volumenEfectos);
        InicializarAudios(dañoZombies, volumenEfectos);
        InicializarAudios(muerteZombies, volumenEfectos);
    }

    private void InicializarAudios(audio[] lista, float volumenBase)
    {
        if (lista == null) return;

        foreach (audio a in lista)
        {
            a.source = gameObject.AddComponent<AudioSource>();
            a.source.clip = a.clip;
            a.source.volume = a.volumen * volumenBase;
            a.source.loop = a.loop;
        }
    }

    public void ActualizarVolumenMusica(float nuevoVolumen)
    {
        volumenMusica = nuevoVolumen;
        ActualizarListaVolumen(musica, nuevoVolumen);
        ActualizarListaVolumen(musicaMuerte, nuevoVolumen);
        ActualizarListaVolumen(musicaMenu, nuevoVolumen);
        ActualizarListaVolumen(safeZone, nuevoVolumen);
        ActualizarListaVolumen(musicaVictoria, nuevoVolumen);   // ← AÑADIDO
    }

    public void ActualizarVolumenEfectos(float nuevoVolumen)
    {
        volumenEfectos = nuevoVolumen;
        ActualizarListaVolumen(sonido, nuevoVolumen);
        ActualizarListaVolumen(ruidosZombie, nuevoVolumen);
        ActualizarListaVolumen(dañoZombies, nuevoVolumen);
        ActualizarListaVolumen(muerteZombies, nuevoVolumen);
    }

    private void ActualizarListaVolumen(audio[] lista, float volumenBase)
    {
        if (lista == null) return;

        foreach (audio a in lista)
        {
            if (a.source != null)
                a.source.volume = a.volumen * volumenBase;
        }
    }

    public void Play(string nombreAudio)
    {
        if (BuscarYReproducir(sonido, nombreAudio)) return;
        if (BuscarYReproducir(musica, nombreAudio)) return;
        if (BuscarYReproducir(musicaMuerte, nombreAudio)) return;
        if (BuscarYReproducir(musicaMenu, nombreAudio)) return;
        if (BuscarYReproducir(safeZone, nombreAudio)) return;
        if (BuscarYReproducir(musicaVictoria, nombreAudio)) return;   // ← AÑADIDO
        if (BuscarYReproducir(ruidosZombie, nombreAudio)) return;
        if (BuscarYReproducir(dañoZombies, nombreAudio)) return;
        if (BuscarYReproducir(muerteZombies, nombreAudio)) return;

        Debug.LogWarning($"[AudioManager] No encontré ningún audio llamado '{nombreAudio}'");
    }

    public void Stop(string nombreAudio)
    {
        if (BuscarYDetener(sonido, nombreAudio)) return;
        if (BuscarYDetener(musica, nombreAudio)) return;
        if (BuscarYDetener(musicaMuerte, nombreAudio)) return;
        if (BuscarYDetener(musicaMenu, nombreAudio)) return;
        if (BuscarYDetener(safeZone, nombreAudio)) return;
        if (BuscarYDetener(musicaVictoria, nombreAudio)) return;   // ← AÑADIDO
        if (BuscarYDetener(ruidosZombie, nombreAudio)) return;
        if (BuscarYDetener(dañoZombies, nombreAudio)) return;
        if (BuscarYDetener(muerteZombies, nombreAudio)) return;

        Debug.LogWarning($"[AudioManager] No encontré ningún audio llamado '{nombreAudio}'");
    }

    public void PlayUnaVez(string nombreAudio)
    {
        var todasLasListas = sonido
            .Concat(ruidosZombie)
            .Concat(dañoZombies)
            .Concat(muerteZombies);

        foreach (audio s in todasLasListas)
        {
            if (s.nombre == nombreAudio)
            {
                s.source.PlayOneShot(s.clip, s.volumen * volumenEfectos);
                return;
            }
        }

        Debug.LogWarning($"[AudioManager] No se encontró el audio: {nombreAudio}");
    }

    private bool BuscarYReproducir(audio[] lista, string nombre)
    {
        if (lista == null) return false;

        foreach (audio a in lista)
        {
            if (a.nombre == nombre)
            {
                a.source.Play();
                return true;
            }
        }
        return false;
    }

    private bool BuscarYDetener(audio[] lista, string nombre)
    {
        if (lista == null) return false;

        foreach (audio a in lista)
        {
            if (a.nombre == nombre)
            {
                a.source.Stop();
                return true;
            }
        }
        return false;
    }
}

