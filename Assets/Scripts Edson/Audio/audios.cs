using UnityEngine;

[System.Serializable]

public class audio
{
    public string nombre;

    public AudioClip clip;

    [Range(0f,1f)]
    public float volumen;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

}
