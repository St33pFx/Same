using UnityEngine;
using UnityEngine.AI;

public class VidaEnemigo : MonoBehaviour
{
    [SerializeField] private int vida = 10;

    [Header("Loot al morir")]
    [SerializeField] private GameObject prefabDrop;        // Drop principal
    [SerializeField] private GameObject prefabParticulas;  // Segundo drop (partículas)
    [SerializeField] private Transform puntoParticulas;    // Punto de spawn partículas

    [Range(0f, 1f)]
    [SerializeField] private float probabilidadDrop = 1f;

    [Header("Animación")]
    [SerializeField] private Animator animator;
    [SerializeField] private string boolMuerte = "IsDead";
    [SerializeField] private float tiempoAnimacionMuerte = 1.2f;

    [Header("Componentes")]
    [SerializeField] private NavMeshAgent agente;

    private bool yaMurio = false;

    public void TomarDano(int dano)
    {
        if (yaMurio) return;

        vida -= dano;

        if (vida <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        if (yaMurio) return;
        yaMurio = true;

        // Activar animación
        if (animator != null)
            animator.SetBool(boolMuerte, true);

        // Convertir el collider en trigger para evitar colisiones mientras muere
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;

        // Apagar el NavMeshAgent
        if (agente != null)
            agente.enabled = false;

        // Registrar muerte en contador
        EnemyCounter contador = FindAnyObjectByType<EnemyCounter>();
        if (contador != null)
            contador.RegistrarMuerte();

        StartCoroutine(ProcesoMuerte());
    }

    private System.Collections.IEnumerator ProcesoMuerte()
    {
        // Esperar la mitad del tiempo → dropear partículas
        yield return new WaitForSeconds(tiempoAnimacionMuerte * 0.5f);

        if (prefabParticulas != null && puntoParticulas != null)
        {
            Instantiate(prefabParticulas, puntoParticulas.position, puntoParticulas.rotation);
        }

        // Esperar la segunda mitad → loot principal y destruir
        yield return new WaitForSeconds(tiempoAnimacionMuerte * 0.5f);

        if (prefabDrop != null && Random.value <= probabilidadDrop)
        {
            Instantiate(prefabDrop, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}