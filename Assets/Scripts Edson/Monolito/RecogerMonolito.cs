using UnityEngine;

public class RecogerMonolito : MonoBehaviour
{
    [Header("Referencia al jugador")]
    public Transform ManoDelJugador; // Donde se sujeta el monolito

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
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
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
            transform.position = ManoDelJugador.position;
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

        if (colliderParaDesactivar != null)
            colliderParaDesactivar.enabled = true;
    }
}