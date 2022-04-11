using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Item
{
    [Header("Pickup Settings")]
    [SerializeField]
    protected string itemName;
    public string ItemName { get { return itemName; } }

    [SerializeField]
    protected Sprite iconInInventory;
    public Sprite IconInInventory { get { return iconInInventory; } }

    public virtual void Pick()
    {
        Inventory.Instance.AddItemInInventory(iconInInventory, itemName);
        Destroy(gameObject);
        GameManager.Instance.ClearReference();
        GameManager.Instance.SetCharacterThoughts(message, timeToFadeThoughts);
    }

    public override void Trigger()
    {
        Pick();
        base.Trigger();
    }

    //public override void ShowIcon()
    //{
    //    GameManager.Instance.ShowIconInGame(Helpers.pickupIcon);
    //}
}
