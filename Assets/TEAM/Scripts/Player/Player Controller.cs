using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Rigidbody rigidB;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform handlePivot;

    // Booleanos pa el player
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private bool canJump = true;

    [Header("Player Settings")]
    [SerializeField] private float speedMovemnt = 3f;
    [SerializeField] private float gravity = -10f;
    [SerializeField] private float staminaPlayer = 20f;
    [SerializeField] private float jumpForce = 4.5f;

    [Header("Camera Settings")]
    [SerializeField] private float mouseSensitivity = 10f;
    [SerializeField] private float cameraFov = 60f;

    [Header("Footstep Settings")]
    [SerializeField] private float walkStepInterval = 0.5f; // tiempo entre pasos al caminar
    [SerializeField] private float runStepInterval = 0.3f;  // tiempo entre pasos al correr

    private Vector3 playerMovementInput;
    private Vector3 playerMouseInput;
    private float xRotation = 0f;

    private float footstepTimer = 0f;
    private bool isMoving = false;
    private bool isRunning = false;

    void Start()
    {
        rigidB = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Input de movimiento y mouse
        playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        playerMouseInput = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f);

        // Flags básicos
        isMoving = playerMovementInput.magnitude > 0.1f;
        isRunning = Input.GetKey(KeyCode.LeftShift) && staminaPlayer > 0f && isMoving;

        MovePlayer();
        MoveCameraPlayer();
        HandleFootsteps();
    }

    private void MovePlayer()
    {
        // Velocidad según si corre o camina
        float targetSpeed = isRunning ? 6f : 3f;
        speedMovemnt = targetSpeed;

        Vector3 moveVector = transform.TransformDirection(playerMovementInput) * speedMovemnt;
        rigidB.linearVelocity = new Vector3(moveVector.x, rigidB.linearVelocity.y, moveVector.z);

        // Salto básico
        if (Input.GetKeyDown(KeyCode.Space) && canJump && isGrounded)
        {
            rigidB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        StaminaConsume();
    }

    private void MoveCameraPlayer()
    {
        xRotation -= playerMouseInput.y * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(0f, playerMouseInput.x * mouseSensitivity, 0f);
        cameraTransform.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void StaminaConsume()
    {
        if (isRunning)
        {
            staminaPlayer -= 5f * Time.deltaTime;
            if (staminaPlayer <= 0f)
            {
                staminaPlayer = 0f;
            }
        }
        else
        {
            if (staminaPlayer < 20f)
            {
                staminaPlayer += 2f * Time.deltaTime;
                if (staminaPlayer > 20f)
                {
                    staminaPlayer = 20f;
                }
            }
        }
    }

    private void HandleFootsteps()
    {
        // Aquí luego puedes meter un chequeo de isGrounded real (raycast, etc.)
        if (!isMoving || !isGrounded)
        {
            footstepTimer = 0f;
            return;
        }

        footstepTimer += Time.deltaTime;

        float currentInterval = isRunning ? runStepInterval : walkStepInterval;

        if (footstepTimer >= currentInterval)
        {
            footstepTimer = 0f;

            // Evita errores si por alguna razón no hay SoundManager
            if (SoundManager.Instance == null) return;

            if (isRunning)
            {
                SoundManager.Instance.PlayRun();
            }
            else
            {
                SoundManager.Instance.PlayWalk();
            }
        }
    }
}
