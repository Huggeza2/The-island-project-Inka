using UnityEngine;

public class TestItem2 : Item
{
    public override void Interact()
    {
        Debug.Log($"Interacted with TestItem2 script via the object: {name}");
    }
}