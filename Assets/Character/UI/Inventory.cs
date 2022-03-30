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

    [SerializeField]
    public static bool isOpen = false;

    private bool canTriggerAgain = true;

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

    public void RemoveItemInInventory(string itemName)
    {
        Slot slot = CheckIfItemExistInInventory(itemName);

        if (slot)
        {
            slots.Remove(slot);
        }

        Destroy(slot.gameObject);
    }

    public void Trigger()
    {
        if (canTriggerAgain)
        {
            canTriggerAgain = false;

            if (isOpen)
            {
                transform.LeanMoveLocal(new Vector2(transform.localPosition.x - 200, transform.localPosition.y), .8f).setEaseInOutBack();
                isOpen = false;
                StartCoroutine(CanTriggerAgain());
            }
            else
            {
                transform.LeanMoveLocal(new Vector2(transform.localPosition.x + 200, transform.localPosition.y), .8f).setEaseInOutBack();
                isOpen = true;
                StartCoroutine(CanTriggerAgain());
            }
        }
    }

    IEnumerator CanTriggerAgain()
    {
        yield return new WaitForSeconds(.5f);
        canTriggerAgain = true;
    }
}
