using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Camera playerCamera;
    private EstadisticasJugador stats;

    [Header("Parámetros")]
    public float bulletSpeed = 20f;

    void Start()
    {
        stats = FindObjectOfType<EstadisticasJugador>();

        if (stats == null)
            Debug.LogWarning("No se encontró EstadisticasJugador en la escena.");
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (stats != null && stats.municionActual > 0)
            {
                Shoot();

                // Restar munición de persistente
                MunicionPersistente.Instance?.UsarMunicion(1);

                // Sincronizar con stats local
                stats.municionActual = MunicionPersistente.Instance.municionActual;

                UIManager.Instance?.MostrarDisparo();
            }
            else
            {
                Debug.Log("Sin munición");
            }
        }
    }

    void Shoot()
    {


        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(100f);

        Vector3 direction = (targetPoint - firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = direction * bulletSpeed;

        Destroy(bullet, 2f);

        SoundManager.Instance.PlayShoot();
    }
}