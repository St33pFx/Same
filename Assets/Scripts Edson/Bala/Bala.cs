using UnityEngine;

public class Bala : MonoBehaviour
{
    [SerializeField] private string enemigo;
    [SerializeField] private int dano;

    [Tooltip("Tiempo en segundos antes de destruir el objeto")]
    public float tiempoDeVida = 5f;

    private void Start()
    {
        // Destruir la bala después de cierto tiempo
        Destroy(gameObject, tiempoDeVida);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(enemigo))
        {
            VidaEnemigo vida = collision.gameObject.GetComponent<VidaEnemigo>();
            if (vida != null)
            {
                vida.TomarDano(dano);
            }
        }

        // Destruir la bala siempre que choque con algo
        Destroy(gameObject);
    }
}
