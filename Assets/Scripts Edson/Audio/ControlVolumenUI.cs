using UnityEngine;
using UnityEngine.UI;

public class ControlVolumenUI : MonoBehaviour
{
    public Slider sliderMusica;
    public Slider sliderEfectos;

    private const string volumenMusicaKey = "VolumenMusica";
    private const string volumenEfectosKey = "VolumenEfectos";

    private void Start()
    {
        // Cargar valores guardados (o asignar 1 por defecto)
        float volumenMusicaGuardado = PlayerPrefs.GetFloat(volumenMusicaKey, 1f);
        float volumenEfectosGuardado = PlayerPrefs.GetFloat(volumenEfectosKey, 1f);

        sliderMusica.value = volumenMusicaGuardado;
        sliderEfectos.value = volumenEfectosGuardado;

        // Asignar los valores al AudioManager
        AudioManager.instance.ActualizarVolumenMusica(volumenMusicaGuardado);
        AudioManager.instance.ActualizarVolumenEfectos(volumenEfectosGuardado);

        // Vincular eventos
        sliderMusica.onValueChanged.AddListener(OnMusicaSliderChanged);
        sliderEfectos.onValueChanged.AddListener(OnEfectosSliderChanged);
    }

    private void OnMusicaSliderChanged(float valor)
    {
        AudioManager.instance.ActualizarVolumenMusica(valor);
        PlayerPrefs.SetFloat(volumenMusicaKey, valor);
    }

    private void OnEfectosSliderChanged(float valor)
    {
        AudioManager.instance.ActualizarVolumenEfectos(valor);
        PlayerPrefs.SetFloat(volumenEfectosKey, valor);
    }
}
