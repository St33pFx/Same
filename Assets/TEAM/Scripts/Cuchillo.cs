using UnityEngine;

public class Cuchillo : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Prefab del cuchillo que se va a crear")]
    public GameObject prefabCuchillo;

    [Tooltip("Punto donde aparecerá el cuchillo")]
    public Transform puntoDeInstancia;

    [Header("Configuración")]
    [Tooltip("Nombre exacto de la animación que debe detectar")]
    public string nombreAnimacion = "estocada";

    private GameObject cuchilloActual;
    private Animator animatorCuchillo;
    private bool animacionTerminada = true;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F) && animacionTerminada)
        {
            SoundManager.Instance.PlayKnife();
            CrearCuchillo();
        }

        if (cuchilloActual != null && animatorCuchillo != null)
        {
            AnimatorStateInfo info = animatorCuchillo.GetCurrentAnimatorStateInfo(0);

            if (info.IsName(nombreAnimacion) && info.normalizedTime >= 1.0f)
            {
                Destroy(cuchilloActual);
                cuchilloActual = null;
                animatorCuchillo = null;
                animacionTerminada = true;
            }
        }
    }

    void CrearCuchillo()
    {
        if (prefabCuchillo == null || puntoDeInstancia == null)
        {
            Debug.LogWarning("Falta asignar el prefab o el punto de instancia en el inspector.");
            return;
        }

        cuchilloActual = Instantiate(prefabCuchillo, puntoDeInstancia.position, puntoDeInstancia.rotation, puntoDeInstancia);
        animatorCuchillo = cuchilloActual.GetComponent<Animator>();

        if (animatorCuchillo == null)
        {
            Debug.LogWarning("El prefab del cuchillo no tiene un Animator.");
        }

        animacionTerminada = false;
    }
}
