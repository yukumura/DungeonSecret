using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    Slot[] slots;

    public Slot GetFirstAvailableSlot()
    {
        return slots.Where(x => x.IsEmpty).FirstOrDefault();
    }

    public Slot CheckIfItemExistInInventory(string itemName)
    {
        return slots.Where(x => x.ItemName == itemName).FirstOrDefault();
    }
}
