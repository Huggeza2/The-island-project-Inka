using UnityEngine;

public class TestItem1 : Item
{
    public override void Interact()
    {
        Debug.Log($"Interacted with TestItem1 script via the object: {name}");
    }
}