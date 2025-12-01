using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [Header("Valores de Retroceso")]
    public float kickbackAmount = 0.1f;     // Qué tanto se echa hacia atrás
    public float sideKickAmount = 0.05f;    // Movimiento hacia arriba/abajo
    public float snappiness = 10f;          // Qué tan rápido pega
    public float returnSpeed = 7f;          // Qué tan rápido vuelve

    private Vector3 originalPos;
    private Quaternion originalRot;

    private Vector3 targetPos;
    private Quaternion targetRot;

    private Vector3 currentPos;
    private Quaternion currentRot;

    void Start()
    {
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
        targetPos = Vector3.zero;
        targetRot = Quaternion.identity;
    }

    void Update()
    {
        // Volver al original con suavidad
        targetPos = Vector3.Lerp(targetPos, Vector3.zero, returnSpeed * Time.deltaTime);
        targetRot = Quaternion.Slerp(targetRot, Quaternion.identity, returnSpeed * Time.deltaTime);

        currentPos = Vector3.Lerp(currentPos, targetPos, snappiness * Time.deltaTime);
        currentRot = Quaternion.Slerp(currentRot, targetRot, snappiness * Time.deltaTime);

        transform.localPosition = originalPos + currentPos;
        transform.localRotation = originalRot * currentRot;
    }

    public void AddWeaponRecoil()
    {
        // Movimiento hacia atrás
        targetPos += new Vector3(0f, 0f, -kickbackAmount);

        // Rotación ligera (arma sube y baja)
        float side = Random.Range(-sideKickAmount, sideKickAmount);
        targetRot *= Quaternion.Euler(-sideKickAmount * 30f, side * 30f, 0f);
    }
}
