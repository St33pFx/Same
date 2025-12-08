using UnityEngine;

public class DestruirObjeto : MonoBehaviour
{
    [SerializeField] private float tiempo = 2f;

    [Header("Sistema de partículas opcional")]
    [SerializeField] private ParticleSystem particulas;

    private void Start()
    {
        // Si tiene partículas, iniciar la rutina
        if (particulas != null)
            StartCoroutine(DetenerParticulasAntes());

        // Destruir el objeto al final del tiempo
        Destroy(gameObject, tiempo);
    }

    private System.Collections.IEnumerator DetenerParticulasAntes()
    {
        // Espera hasta 1 segundo antes del tiempo total
        yield return new WaitForSeconds(tiempo - 1f);

        // Detiene las partículas
        particulas.Stop();
    }
}
