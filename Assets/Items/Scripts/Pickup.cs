using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Item
{
    [Header("Pickup Settings")]
    [SerializeField]
    protected string itemName;
    public string ItemName { get { return itemName; } }

    [Header("Thoughts Settings")]
    [SerializeField]
    string pickupMessage;
    [SerializeField]
    float timeToFadeThoughts; 


    [SerializeField]
    protected Sprite iconInInventory;
    public Sprite IconInInventory { get { return iconInInventory; } }

    public void Pick()
    {
        Inventory.Instance.AddItemInInventory(iconInInventory, itemName);
        GameManager.Instance.ClearReference();
        GameManager.Instance.SetCharacterThoughts(pickupMessage, timeToFadeThoughts);
        Destroy(gameObject);
    }

    public override void Action()
    {
        Pick();
    }

    public override void ShowIcon()
    {
        GameManager.Instance.ShowIconInGame(Helpers.pickupIcon);
    }
}
