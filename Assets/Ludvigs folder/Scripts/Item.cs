using UnityEngine;

public abstract class Item : MonoBehaviour, IInteractable
{

    /// <summary>
    /// Called when the player interacts with this item.
    /// </summary>
    public virtual void Interact()
    {
        Debug.Log($"Interacted with item: {name}");
    }
}