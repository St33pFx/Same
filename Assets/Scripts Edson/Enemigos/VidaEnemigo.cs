using UnityEngine;

public class VidaEnemigo : MonoBehaviour
{
    [SerializeField] private int vida = 10;

    [Header("Loot al morir")]
    [SerializeField] private GameObject prefabDrop; // El objeto que va a soltar

    [Range(0f, 1f)]
    [SerializeField] private float probabilidadDrop = 1f; // Probabilidad

    public void TomarDano(int dano)
    {
        vida -= dano;
        if (vida <= 0)
        {
            Morir();
        }
        SoundManager.Instance.PlayEnemyHit();
    }

    private void Morir()
    {
        EnemyCounter contador = FindAnyObjectByType<EnemyCounter>();
        if (contador != null)
        {
            contador.RegistrarMuerte();
        }
        Destroy(gameObject);

        if (prefabDrop != null && Random.value <= probabilidadDrop)
        {
            Debug.Log("Instanciando drop en posición: " + transform.position);
            Instantiate(prefabDrop, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
        else
        {
            Debug.Log("No se generó drop. PrefabDrop=" + prefabDrop + " Probabilidad=" + probabilidadDrop);
        }

        
    }
}
