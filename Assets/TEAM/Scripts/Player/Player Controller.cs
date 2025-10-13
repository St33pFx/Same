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

    private Vector3 playerMovementInput;
    private Vector3 playerMouseInput;
    private float xRotation = 0f;


    void Start()
    {
        rigidB = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Update()
    {
        playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        playerMouseInput = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f);

        MovePlayer();
        MoveCameraPlayer();
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(playerMovementInput) * speedMovemnt;
        rigidB.linearVelocity = new Vector3(MoveVector.x, rigidB.linearVelocity.y,  MoveVector.z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speedMovemnt = 6f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speedMovemnt = 3f;
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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            staminaPlayer -= 5f * Time.deltaTime;
            if (staminaPlayer <= 0f)
            {
                staminaPlayer = 0f;
                speedMovemnt = 3f;
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

}
