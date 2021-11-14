using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actionable : Item
{
    [SerializeField]
    Pickup[] itemsRequired;
    [SerializeField]
    List<Slot> inventorySlot = new List<Slot>();
    [SerializeField]
    bool isUsed = false;
    public bool IsUsed { get { return isUsed; } }

    public bool CheckIfPlayerHasRequiredItems()
    {
        inventorySlot = new List<Slot>();
        List<Slot> slotsList = new List<Slot>();

        foreach (Pickup item in itemsRequired)
        {
            Slot slot = inventory.CheckIfItemExistInInventory(item.ItemName);
            if (slot != null)
                slotsList.Add(slot);
            else
                return false;
        }

        inventorySlot = slotsList;

        return true;
    }
    public void DoAction()
    {
        foreach (Slot slot in inventorySlot)
        {
            slot.RemoveItem();
        }

        isUsed = true;
    }
}
