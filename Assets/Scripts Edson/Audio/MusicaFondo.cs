using UnityEngine;

public class MusicaFondo : MonoBehaviour
{
    public static MusicaFondo Instance { get; private set; }

    [Header("Audio Source principal")]
    [SerializeField] private AudioSource audioSource;

    [Header("Listas de música")]
    public AudioClip[] musicaJuego;
    public AudioClip[] musicaMuerte;
    public AudioClip[] musicaMenu;
    public AudioClip[] musicaSafeZone;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.playOnAwake = false; // IMPORTANTÍSIMO: que no suene solo
        }
    }

    public void DetenerTodaLaMusica()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    private void ReproducirClipAleatorio(AudioClip[] lista)
    {
        if (audioSource == null || lista == null || lista.Length == 0) return;

        int index = Random.Range(0, lista.Length);
        audioSource.clip = lista[index];
        audioSource.Play();
    }

    public void ReproducirMusicaAleatoria()
    {
        ReproducirClipAleatorio(musicaJuego);
    }

    public void ReproducirMusicaMuerteAleatoria()
    {
        ReproducirClipAleatorio(musicaMuerte);
    }

    public void ReproducirMusicaMenuAleatoria()
    {
        ReproducirClipAleatorio(musicaMenu);
    }

    public void ReproducirMusicaSafezoneAleatoria()
    {
        ReproducirClipAleatorio(musicaSafeZone);
    }
}
