using UnityEngine;

public class FPSCameraBobbingAndSway : MonoBehaviour
{
    [Header("References")]
    public Rigidbody playerRb;                 // arrastra el Rigidbody del player
    public Transform cameraTransform;          // arrastra tu Main Camera (si dejas vac�o usa this.transform)
    public Transform weaponPivot;              // opcional: pivote del arma para sway posicional extra

    [Header("Head Bob (walk/run)")]
    [Tooltip("Amplitud lateral del bob (X).")]
    public float bobAmplitudeX = 0.03f;
    [Tooltip("Amplitud vertical del bob (Y).")]
    public float bobAmplitudeY = 0.05f;
    [Tooltip("Frecuencia base caminando.")]
    public float walkFrequency = 1.8f;
    [Tooltip("Frecuencia corriendo.")]
    public float runFrequency = 2.8f;
    [Tooltip("Velocidad a partir de la cual inicia el bob.")]
    public float minSpeedToBob = 0.1f;
    [Tooltip("Factor para suavizar el bob aplicado.")]
    public float bobSmooth = 12f;
    [Tooltip("Multiplicador de frecuencia seg�n la velocidad.")]
    public float speedToFreq = 0.35f;

    [Header("Ground Check (opcional)")]
    public bool useGroundCheck = true;
    public LayerMask groundMask = ~0;
    public float groundCheckDistance = 0.2f;
    public Vector3 groundCheckOffset = new Vector3(0, 0.05f, 0);

    [Header("Mouse Sway (rotaci�n de c�mara/arma)")]
    [Tooltip("Grados de rotaci�n por unidad de mouse.")]
    public float swayDegrees = 2.0f;
    [Tooltip("L�mite m�ximo de rotaci�n por sway.")]
    public float maxSwayAngle = 6.0f;
    [Tooltip("Qu� tan r�pido sigue el objetivo de sway.")]
    public float swayFollowSpeed = 10f;
    [Tooltip("Qu� tan r�pido vuelve al centro al no mover mouse.")]
    public float swayReturnSpeed = 8f;

    [Header("Positional Sway (arma)")]
    [Tooltip("Desplazamiento posicional del arma por movimiento de mouse.")]
    public float posSwayAmount = 0.02f;
    [Tooltip("Suavizado del sway posicional del arma.")]
    public float posSwayReturn = 10f;
    [Tooltip("L�mite del desplazamiento posicional.")]
    public float posSwayClamp = 0.06f;

    [Header("Run Toggle (opcional)")]
    public KeyCode runKey = KeyCode.LeftShift;
    public float runSpeedThreshold = 3.5f;

    // internals
    Vector3 camLocalBasePos;
    Quaternion camLocalBaseRot;
    Vector3 weapLocalBasePos;
    Quaternion weapLocalBaseRot;

    float bobTime;
    Vector3 bobCurrentOffset;
    Vector3 bobVel; // para SmoothDamp si lo quisieras, aqu� usamos Lerp

    void Reset()
    {
        cameraTransform = GetComponentInChildren<Camera>() ? GetComponentInChildren<Camera>().transform : null;
        if (!playerRb) playerRb = GetComponentInParent<Rigidbody>();
    }

    void Awake()
    {
        if (!cameraTransform) cameraTransform = transform;
        camLocalBasePos = cameraTransform.localPosition;
        camLocalBaseRot = cameraTransform.localRotation;

        if (weaponPivot)
        {
            weapLocalBasePos = weaponPivot.localPosition;
            weapLocalBaseRot = weaponPivot.localRotation;
        }
    }

    void Update()
    {
        if (!playerRb) return;

        // ===== HEAD BOB =====
        float speed = new Vector2(playerRb.linearVelocity.x, playerRb.linearVelocity.z).magnitude;
        bool grounded = !useGroundCheck || IsGrounded();

        Vector3 targetBob = Vector3.zero;
        if (grounded && speed > minSpeedToBob)
        {
            float isRunning = (Input.GetKey(runKey) || speed > runSpeedThreshold) ? 1f : 0f;
            float freq = Mathf.Lerp(walkFrequency, runFrequency, isRunning);
            // aumenta frecuencia con la velocidad
            freq += speed * speedToFreq;

            bobTime += Time.deltaTime * freq;

            // patr�n: X coseno, Y seno absoluto (para impacto del paso)
            float x = Mathf.Cos(bobTime) * bobAmplitudeX;
            float y = Mathf.Abs(Mathf.Sin(bobTime)) * bobAmplitudeY;

            targetBob = new Vector3(x, y, 0f);
        }
        else
        {
            // cuando est� quieto o en el aire, resetea lentamente
            bobTime = Mathf.Lerp(bobTime, 0f, Time.deltaTime * 5f);
        }

        bobCurrentOffset = Vector3.Lerp(bobCurrentOffset, targetBob, Time.deltaTime * bobSmooth);
        cameraTransform.localPosition = camLocalBasePos + bobCurrentOffset;

        // ===== MOUSE SWAY (rotaci�n) =====
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // Rotaci�n objetivo en grados (pitch(+arriba), yaw(+derecha), roll)
        float yaw = Mathf.Clamp(-mx * swayDegrees, -maxSwayAngle, maxSwayAngle);
        float pitch = Mathf.Clamp(my * swayDegrees, -maxSwayAngle, maxSwayAngle);
        float roll = Mathf.Clamp(-mx * (swayDegrees * 0.6f), -maxSwayAngle, maxSwayAngle);

        Quaternion targetSwayRot = camLocalBaseRot * Quaternion.Euler(pitch, yaw, roll);

        // Seguir r�pido cuando hay input; si no, regresar al centro un poco m�s lento
        float rotLerpSpeed = (Mathf.Abs(mx) + Mathf.Abs(my)) > 0.001f ? swayFollowSpeed : swayReturnSpeed;
        cameraTransform.localRotation = Quaternion.Lerp(cameraTransform.localRotation, targetSwayRot, Time.deltaTime * rotLerpSpeed);

        // ===== POS SWAY (arma opcional) =====
        if (weaponPivot)
        {
            // movimiento contrario (sutil) al mouse
            Vector3 targetPosOffset = new Vector3(-mx, -my, 0f) * posSwayAmount;
            targetPosOffset.x = Mathf.Clamp(targetPosOffset.x, -posSwayClamp, posSwayClamp);
            targetPosOffset.y = Mathf.Clamp(targetPosOffset.y, -posSwayClamp, posSwayClamp);

            Vector3 desired = weapLocalBasePos + targetPosOffset + bobCurrentOffset * 0.25f; // un poco de bob en arma
            weaponPivot.localPosition = Vector3.Lerp(weaponPivot.localPosition, desired, Time.deltaTime * posSwayReturn);

            // rotaci�n muy sutil del arma acorde al sway de la c�mara
            Quaternion weapTargetRot = weapLocalBaseRot * Quaternion.Euler(pitch * 0.5f, yaw * 0.5f, roll * 0.8f);
            weaponPivot.localRotation = Quaternion.Lerp(weaponPivot.localRotation, weapTargetRot, Time.deltaTime * (posSwayReturn * 0.7f));
        }
    }

    bool IsGrounded()
    {
        Vector3 origin = playerRb.worldCenterOfMass + groundCheckOffset;
        return Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundMask, QueryTriggerInteraction.Ignore);
    }

    // Gizmos para ver el ray de ground
    void OnDrawGizmosSelected()
    {
        if (!playerRb) return;
        Gizmos.color = Color.cyan;
        Vector3 origin = playerRb.worldCenterOfMass + groundCheckOffset;
        Gizmos.DrawLine(origin, origin + Vector3.down * groundCheckDistance);
    }
}
