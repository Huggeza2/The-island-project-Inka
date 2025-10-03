using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Movement speed of the player in units per second.")]
    public float moveSpeed = 5f;

    [Header("Look Settings")]
    [Tooltip("Reference to the player's POV-camera.")]
    public Transform playerCamera;

    [Tooltip("Mouse sensitivity multiplier for looking around.")]
    public float mouseSensitivity = 2f;

    /// <summary>
    /// The pitch of the camera.
    /// </summary>
    private float cameraPitch = 0f;

    /// <summary>
    /// Reference to the RigidBody component of this object.
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// Normalized movement input vector derived from keyboard input.
    /// </summary>
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Cache Rigidbody reference.
        rb.freezeRotation = true;        // Prevent physics from tipping the player over.

        Cursor.lockState = CursorLockMode.Locked;  // Lock cursor to center.
        Cursor.visible = false;                    // Hide cursor.
    }

    void Update()
    {
        HandleMouseLook();
        HandleKeyboardInput();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    /// <summary>
    /// Handles rotating the player horizontally and the camera vertically based on mouse input.
    /// </summary>
    private void HandleMouseLook()
    {
        // Get mouse delta movement and apply sensitivity.
        Vector2 lookDelta = mouseSensitivity * Time.deltaTime * Mouse.current.delta.ReadValue();

        // Rotate the player object horizontally (yaw).
        transform.Rotate(Vector3.up * lookDelta.x);

        // Rotate the camera vertically (pitch), clamp to prevent flipping.
        cameraPitch = Mathf.Clamp(cameraPitch - lookDelta.y, -80f, 80f);
        playerCamera.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);
    }

    /// <summary>
    /// Reads keyboard input and calculates a normalized movement vector.
    /// </summary>
    private void HandleKeyboardInput()
    {
        Vector2 input = Vector2.zero;
        var keyboard = Keyboard.current;

        // Add/subtract based on pressed keys.
        if (keyboard.wKey.isPressed) input.y += 1f;
        if (keyboard.sKey.isPressed) input.y -= 1f;
        if (keyboard.dKey.isPressed) input.x += 1f;
        if (keyboard.aKey.isPressed) input.x -= 1f;

        // Normalize so diagonal movement isn't faster.
        moveInput = input.normalized;
    }

    /// <summary>
    /// Moves the player based on the calculated movement vector using smooth physics-based movement.
    /// </summary>
    private void MovePlayer()
    {
        // Calculate movement vector based on player orientation and input.
        Vector3 movement = moveSpeed * Time.fixedDeltaTime * (transform.right * moveInput.x + transform.forward * moveInput.y);

        // Move the Rigidbody to the new position.
        rb.MovePosition(rb.position + movement);
    }
}
