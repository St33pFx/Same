using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Camera playerCamera;
    private EstadisticasJugador stats; // Referencia automática al jugador

    [Header("Parámetros")]
    public float bulletSpeed = 20f;

    void Start()
    {
        // Busca automáticamente el componente de estadísticas del jugador
        stats = FindObjectOfType<EstadisticasJugador>();

        if (stats == null)
            Debug.LogWarning("No se encontró EstadisticasJugador en la escena.");
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // Solo dispara si hay munición
            if (stats != null && stats.municionActual > 0)
            {
                Shoot();
                stats.municionActual--; // Resta una bala al disparar
            }
            else
            {
                Debug.Log("Sin munición");
                // Aquí podrías reproducir un sonido de clic vacío o animación
            }
        }
    }

    void Shoot()
    {
        // Lanza un rayo desde el centro de la cámara
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100f);
        }

        // Calcula la dirección del disparo
        Vector3 direction = (targetPoint - firePoint.position).normalized;

        // Instancia la bala y aplica velocidad
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = direction * bulletSpeed;

        // Destruye la bala después de un tiempo
        Destroy(bullet, 2f);
    }
}