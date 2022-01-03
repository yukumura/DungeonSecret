using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Item
{
    [SerializeField]
    protected string itemName;
    public string ItemName { get { return itemName; } }
    

    [SerializeField]
    protected Sprite iconInInventory;
    public Sprite IconInInventory { get { return iconInInventory; } }

    public void Pick()
    {
        Slot slot = inventory.GetFirstAvailableSlot();
        if (slot != null)
        {
            slot.SetItem(iconInInventory, itemName);
            ClearReference();
            GameManager.Instance.SetCharacterThoughts("Questo mi potrà servire più tardi");
        }
    }

    //to do: check if it's better use trigger enter instead character controller OnControllerColliderHit
    private void OnTriggerEnter(Collider other)
    {
    }
}
