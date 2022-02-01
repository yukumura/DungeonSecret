using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        Instance = this;
    }

    [SerializeField]
    List<Slot> slots;

    [SerializeField]
    GameObject slotPrefab;

    public Slot CheckIfItemExistInInventory(string itemName)
    {
        return slots.Where(x => x.ItemName == itemName).FirstOrDefault();
    }

    public void AddItemInInventory(Sprite item, string name)
    {       
        GameObject obj = Instantiate(slotPrefab);
        Slot slot = obj.GetComponent<Slot>();
        slot.SetItem(item, name);
        slots.Add(slot);
        obj.transform.SetParent(transform, false);
    }
}
