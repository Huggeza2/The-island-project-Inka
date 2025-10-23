using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    #region Movement Settings
    [Header("Movement Settings")]

    [Tooltip("Movement speed of the player.")]
    public float moveSpeed = 5f;

    [Tooltip("Force applied upwards when the player jumps.")]
    public float jumpForce = 7f;

    [Tooltip("Extra distance below the collider to check for ground contact.")]
    public float groundCheckOffset = 0.1f;

    [Tooltip("Layers that count as 'ground' for grounding and jumping.")]
    public LayerMask groundMask;
    #endregion
    #region Look settings
    [Header("Look Settings")]

    [Tooltip("Reference to the player's POV camera.")]
    public Transform playerCamera;

    [Tooltip("Mouse sensitivity multiplier for camera look.")]
    public float mouseSensitivity = 2f;
    #endregion

    private float cameraPitch = 0f;          // Vertical camera rotation
    private Rigidbody rb;                    // Player Rigidbody reference
    private CapsuleCollider capsule;         // CapsuleCollider reference
    private Vector2 moveInput;               // WASD input vector
    private bool jumpRequested = false;      // Flag to trigger jump in FixedUpdate
    private bool isGrounded = false;         // True if touching ground

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        // Prevent physics from rotating the player
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleKeyboardInput();
        HandleJumpInput();
        CheckGrounded();
    }

    void FixedUpdate()
    {
        MovePlayer();
        ApplyJump();
    }

    /// <summary>
    /// Rotates the player body horizontally and the camera vertically based on mouse input.
    /// </summary>
    private void HandleMouseLook()
    {
        Vector2 lookDelta = mouseSensitivity * Time.deltaTime * Mouse.current.delta.ReadValue();

        // Horizontal rotation (yaw)
        transform.Rotate(Vector3.up * lookDelta.x);

        // Vertical rotation (pitch)
        cameraPitch = Mathf.Clamp(cameraPitch - lookDelta.y, -80f, 80f);
        playerCamera.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);
    }

    /// <summary>
    /// Reads WASD input from the keyboard and stores it as a normalized movement vector.
    /// </summary>
    private void HandleKeyboardInput()
    {
        Vector2 input = Vector2.zero;
        var keyboard = Keyboard.current;
        if (keyboard.wKey.isPressed) input.y += 1f;
        if (keyboard.sKey.isPressed) input.y -= 1f;
        if (keyboard.dKey.isPressed) input.x += 1f;
        if (keyboard.aKey.isPressed) input.x -= 1f;
        moveInput = input.normalized;
    }

    /// <summary>
    /// Detects jump input (Spacebar) and sets a jump request if the player is grounded.
    /// </summary>
    private void HandleJumpInput()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
            jumpRequested = true;
    }

    /// <summary>
    /// Performs a capsule-based ground check slightly below the player's collider.
    /// </summary>
    private void CheckGrounded()
    {
        float halfHeight = (capsule.height * 0.5f) - capsule.radius;
        Vector3 top = transform.position + Vector3.up * halfHeight;
        Vector3 bottom = transform.position - Vector3.up * halfHeight;

        // Extend downward from bottom for accurate ground contact
        Vector3 bottomOffset = bottom + Vector3.down * groundCheckOffset;

        isGrounded = Physics.CheckCapsule(
            top, bottomOffset, capsule.radius * 0.95f, groundMask, QueryTriggerInteraction.Ignore);
    }

    /// <summary>
    /// Moves the player smoothly based on movement input, using Rigidbody physics.
    /// </summary>
    private void MovePlayer()
    {
        Vector3 movement = moveSpeed * Time.fixedDeltaTime *
                           (transform.right * moveInput.x + transform.forward * moveInput.y);
        rb.MovePosition(rb.position + movement);
    }

    /// <summary>
    /// Applies the jump force if a jump was requested in Update().
    /// </summary>
    private void ApplyJump()
    {
        if (jumpRequested)
        {
            // Reset Y velocity to avoid stacking jump momentum.
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            // Apply upward impulse.
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

            jumpRequested = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (capsule == null) return;
        float halfHeight = (capsule.height * 0.5f) - capsule.radius;
        Vector3 top = transform.position + Vector3.up * halfHeight;
        Vector3 bottom = transform.position - Vector3.up * halfHeight;
        Vector3 bottomOffset = bottom + Vector3.down * groundCheckOffset;

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(top, capsule.radius * 0.95f);
        Gizmos.DrawWireSphere(bottomOffset, capsule.radius * 0.95f);
    }
}
