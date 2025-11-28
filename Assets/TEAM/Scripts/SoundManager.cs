using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton sencillo PER ESCENA
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [Tooltip("Fuente para efectos de sonido (disparos, pasos, etc.)")]
    [SerializeField] private AudioSource sfxSource;
    [Tooltip("Fuente para música / ambiente (loop)")]
    [SerializeField] private AudioSource musicSource;

    [Header("Opciones")]
    [Tooltip("Reproducir ambiente automáticamente al empezar la escena")]
    [SerializeField] private bool playAmbientOnStart = true;

    [Header("Player SFX")]
    public AudioClip shootClip;          // Disparo
    public AudioClip walkClip;           // Pasos caminando
    public AudioClip runClip;            // Pasos corriendo (Shift)
    public AudioClip knife;
    public AudioClip reloadClip;         // Recarga
    public AudioClip playerHitClip;      // Cuando el jugador recibe daño
    public AudioClip pickupAmmoClip;     // Recoger munición/balas
    public AudioClip deathClip;          // Cuando mueres

    [Header("Enemy SFX")]
    public AudioClip enemyStepsClip;         // Pasos de enemigos acercándose
    public AudioClip enemyHitClip;           // Cuando tú golpeas al enemigo
    public AudioClip enemyApproachAlertClip; // Efecto de se están acercando

    [Header("Ambiente & UI")]
    public AudioClip ambientClip;        // Sonido ambiental de fondo (loop)
    public AudioClip menuClip;           // Música/sonido de menú (si quieres usarlo)

    private void Awake()
    {
        //  Aquí NO usamos DontDestroyOnLoad ni destruimos duplicados.
        // Cada escena de juego puede tener SU propio SoundManager.
        Instance = this;

        // Autorellenar sources si no los asignaste a mano
        if (sfxSource == null || musicSource == null)
        {
            var sources = GetComponentsInChildren<AudioSource>();
            if (sfxSource == null && sources.Length > 0) sfxSource = sources[0];
            if (musicSource == null && sources.Length > 1) musicSource = sources[1];
        }
    }

    private void Start()
    {
        // Si quieres que cuando entras a la escena suene el ambiente:
        if (playAmbientOnStart && ambientClip != null)
        {
            PlayAmbientLoop();
        }
    }

    #region Public Methods (para llamar desde otros scripts)

    // -------- EFECTOS DE JUGADOR --------
    public void PlayShoot() => PlaySFX(shootClip);
    public void PlayWalk() => PlaySFX(walkClip);
    public void PlayRun() => PlaySFX(runClip);
    public void PlayKnife() => PlaySFX(knife);
    public void PlayReload() => PlaySFX(reloadClip);
    public void PlayPlayerHit() => PlaySFX(playerHitClip);
    public void PlayPickupAmmo() => PlaySFX(pickupAmmoClip);
    public void PlayDeath() => PlaySFX(deathClip);

    // -------- EFECTOS DE ENEMIGOS --------
    public void PlayEnemySteps() => PlaySFX(enemyStepsClip);
    public void PlayEnemyHit() => PlaySFX(enemyHitClip);
    public void PlayEnemyApproachAlert() => PlaySFX(enemyApproachAlertClip);

    // -------- AMBIENTE / MENÚ --------
    public void PlayAmbientLoop()
    {
        PlayMusicLoop(ambientClip);
    }

    public void PlayMenuLoop()
    {
        PlayMusicLoop(menuClip);
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    #endregion

    #region Internal Helpers

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }

    private void PlayMusicLoop(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    #endregion
}
