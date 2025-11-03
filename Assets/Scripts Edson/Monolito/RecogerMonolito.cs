using UnityEngine;

public class RecogerMonolito : MonoBehaviour
{
    [Header("Referencia al jugador")]
    public Transform ManoDelJugador; // Donde se sujeta el monolito

    [Header("Punto donde se soltará el objeto")]
    public Transform PuntoDeSoltar; // Lugar donde cae al soltarlo

    [Header("Configuración")]
    public float RangoDeAlcance = 2f; // Para recoger
    public KeyCode BotonAPresionar = KeyCode.E;

    [Header("Collider opcional a desactivar al cargar")]
    public Collider colliderParaDesactivar;

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

        if (EsCargado)
        {
            // Mantener el objeto en la mano
            transform.position = ManoDelJugador.position;
            transform.rotation = ManoDelJugador.rotation;
        }
    }

    void PickUp()
    {
        EsCargado = true;
        rb.isKinematic = true;
        rb.useGravity = false;

        if (colliderParaDesactivar != null)
            colliderParaDesactivar.enabled = false;
    }

    void Drop()
    {
        EsCargado = false;
        rb.isKinematic = false;
        rb.useGravity = true;

        // 🔹 Si se asignó un punto de soltar, usarlo
        if (PuntoDeSoltar != null)
        {
            transform.position = PuntoDeSoltar.position;
            transform.rotation = PuntoDeSoltar.rotation;
        }

        if (colliderParaDesactivar != null)
            colliderParaDesactivar.enabled = true;
    }
}
