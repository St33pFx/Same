using UnityEngine;

public class DañoPorCuchillo : MonoBehaviour
{
    [Header("Daño")]
    public int dano = 1; // Cantidad de daño que aplica

    private void OnCollisionEnter(Collision collision)
    {
        // Verificar que el objeto tenga tag "Enemigo"
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            VidaEnemigo vidaEnemigo = collision.gameObject.GetComponent<VidaEnemigo>();
            if (vidaEnemigo != null)
            {
                vidaEnemigo.TomarDano(dano);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigo"))
        {
            VidaEnemigo vidaEnemigo = other.GetComponent<VidaEnemigo>();
            if (vidaEnemigo != null)
            {
                vidaEnemigo.TomarDano(dano);
            }
        }
    }
}