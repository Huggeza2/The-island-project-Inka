using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Tooltip("Reference to the player's camera.")]
    public Transform playerCamera;

    [Tooltip("Maximum distance the player can interact with objects.")]
    public float interactDistance = 3f;

    void Update()
    {
        // Check if the player pressed the E key this frame.
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            // Create a ray starting at the camera and pointing forward.
            Ray ray = new(playerCamera.position, playerCamera.forward);

            // Perform a raycast to detect interactable objects.
            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            {
                // Try to get any component that implements IInteractable.
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                // If found, call its Interact method.
                interactable?.Interact();
            }
        }
    }
}
