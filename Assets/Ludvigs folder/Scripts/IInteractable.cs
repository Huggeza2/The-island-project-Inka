using UnityEngine;

public interface IInteractable
{
    /// <summary>
    /// Called when the player interacts with this item.
    /// </summary>
    void Interact(); // Every interactable object must implement this.
}
