using UnityEngine;

public class CarryableMonolith : MonoBehaviour
{
    [Header("Referencia al jugador")]
    public Transform ManoDelJugador; // Punto donde se sujeta el monolito

    [Header("Configuración")]
    public float RangoDeAlcance = 2f; // Distancia máxima para recoger
    public KeyCode BotonAPresionar = KeyCode.E;

    [Header("Punto de soltado")]
    public Transform PuntoASoltar; // Donde se colocará el monolito al soltar

    private bool EsCargado = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.useGravity = true;
        rb.isKinematic = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(BotonAPresionar))
        {
            float distance = Vector3.Distance(transform.position, ManoDelJugador.position);
            if (!EsCargado && distance <= RangoDeAlcance)
            {
                PickUp();
            }
            else if (EsCargado)
            {
                Drop();
            }
        }

        // Mantener el monolito en la posición de la mano mientras se transporta
        if (EsCargado)
        {
            transform.position = ManoDelJugador.position;
        }
    }

    void PickUp()
    {
        EsCargado = true;
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    void Drop()
    {
        EsCargado = false;
        rb.isKinematic = false;
        rb.useGravity = true;

        // Si hay un dropPoint asignado, mover el monolito allí
        if (PuntoASoltar != null)
        {
            transform.position = PuntoASoltar.position;
            transform.rotation = PuntoASoltar.rotation;
        }
    }
}
