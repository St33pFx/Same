using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    [Header("Intensidad")]
    public float recoilAngle = 2f;        // cuánto levanta la mira
    public float recoilKickback = 0.05f;  // cuánto se va hacia atrás (Z-)
    public float snappiness = 10f;        // qué tan rápido pega
    public float returnSpeed = 8f;        // qué tan rápido vuelve

    private Vector3 originalPos;
    private Quaternion originalRot;

    private Vector3 currentPos;
    private Vector3 targetPos;
    private Quaternion currentRot = Quaternion.identity;
    private Quaternion targetRot = Quaternion.identity;

    void Start()
    {
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
    }

    void Update()
    {
        // que el target vuelva poco a poco a 0
        targetPos = Vector3.Lerp(targetPos, Vector3.zero, returnSpeed * Time.deltaTime);
        targetRot = Quaternion.Slerp(targetRot, Quaternion.identity, returnSpeed * Time.deltaTime);

        // movimiento suave hacia el target
        currentPos = Vector3.Lerp(currentPos, targetPos, snappiness * Time.deltaTime);
        currentRot = Quaternion.Slerp(currentRot, targetRot, snappiness * Time.deltaTime);

        transform.localPosition = originalPos + currentPos;
        transform.localRotation = originalRot * currentRot;
    }

    public void AddRecoil()
    {
        // rotación hacia arriba + un poquito random a los lados
        float side = Random.Range(-recoilAngle * 0.5f, recoilAngle * 0.5f);
        targetRot *= Quaternion.Euler(-recoilAngle, side, 0f);

        // empujoncito hacia atrás
        targetPos += new Vector3(0f, 0f, -recoilKickback);
    }
}
