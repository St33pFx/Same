using UnityEngine;
using UnityEngine.UI;

public class UIEffectsManager : MonoBehaviour
{
    public static UIEffectsManager instance;

    private void Awake()
    {
        // Singleton seguro
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // ================================
    //        IMÁGENES DE DAÑO
    // ================================
    [Header("Imágenes de DAÑO")]
    public Image[] imagenesDaño;

    public Image[] GetImagenesDaño()
    {
        return imagenesDaño;
    }

    // ================================
    //        IMÁGENES DE CORDURA
    // ================================
    [Header("Imágenes de CORDURA")]
    public Image[] imagenesCordura;

    public Image[] GetImagenesCordura()
    {
        return imagenesCordura;
    }

    // ================================
    //      (A futuro puedes agregar)
    //  • Imágenes de estamina
    //  • Efectos de flash
    //  • Íconos de HUD
    //  • Post-procesos visuales
    // ================================
}