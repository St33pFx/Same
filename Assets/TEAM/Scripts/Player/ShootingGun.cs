using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Camera playerCamera;
    private EstadisticasJugador stats;

    [Header("Parámetros")]
    public float velocidadBala = 20f;
    public float cadencia = 0.2f;

    private bool isShooting = false;
    private Coroutine shootCoroutine;

    void Start()
    {
        stats = GetComponent<EstadisticasJugador>();

        if (stats == null)
            Debug.LogWarning("No se encontró EstadisticasJugador en la escena.");
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isShooting)
        {
            isShooting = true;
            shootCoroutine = StartCoroutine(DisparoContinuo());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            isShooting = false;

            if (shootCoroutine != null)
                StopCoroutine(shootCoroutine);
        }
    }

    private IEnumerator DisparoContinuo()
    {
        while (isShooting)
        {
            if (stats != null && stats.municionActual > 0)
            {
                Disparar();

                // Restar munición
                MunicionPersistente.Instance?.UsarMunicion(1);
                stats.municionActual = MunicionPersistente.Instance.municionActual;

                UIManager.Instance?.MostrarDisparo();
            }
            else
            {
                Debug.Log("Sin munición");
                isShooting = false;
                break;
            }

            yield return new WaitForSeconds(cadencia);
        }
    }

    private void Disparar()
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
        rb.linearVelocity = direction * velocidadBala;

        Destroy(bullet, 2f);

        SoundManager.Instance.PlayShoot();
    }
}
