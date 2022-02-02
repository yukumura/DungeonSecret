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
        transform.localPosition = new Vector2(474.5f, 204.5f);
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

    public void Trigger()
    {
        if (canTriggerAgain)
        {
            Debug.Log("Trigger inventory");
            canTriggerAgain = false;

            if (isOpen)
            {
                transform.LeanMoveLocal(new Vector2(474.5f, 204.5f), .8f).setEaseInOutBack();
                isOpen = false;
                StartCoroutine(CanTriggerAgain());
            }
            else
            {
                transform.LeanMoveLocal(new Vector2(374.5f, 204.5f), .8f).setEaseInOutBack();
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
