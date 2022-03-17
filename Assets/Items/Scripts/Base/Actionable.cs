using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actionable : Item
{
    [SerializeField]
    Pickup[] itemsRequired;

    [Header("Action Settings")]
    [SerializeField]
    [TextArea]
    string actionMessage;
    [SerializeField]
    [TextArea]
    string unableMessage;

    public bool CheckIfPlayerHasRequiredItems()
    {
        foreach (Pickup item in itemsRequired)
        {
            if (!Inventory.Instance.CheckIfItemExistInInventory(item.ItemName))
            {
                return false;
            }
        }

        return true;
    }
    public override void Trigger()
    {
        if (!isUsed)
        {
            if (CheckIfPlayerHasRequiredItems())
            {
                GameManager.Instance.GetPlayerUI().SetMessage(actionMessage, timeToFadeThoughts);
                RemoveRequiredItemsFromInventory();
                isUsed = true;
                Action();
                base.Trigger();
            }
            else
            {
                GameManager.Instance.GetPlayerUI().SetMessage(unableMessage, timeToFadeThoughts);
                PlayUnableAnimation();
            }
        }
            
        GameManager.Instance.ClearReference();
    }

    public override void ShowIcon()
    {
        if (!isUsed)
            GameManager.Instance.ShowIconInGame(Helpers.actionIcon);
    }

    private void RemoveRequiredItemsFromInventory()
    {
        foreach (Pickup item in itemsRequired)
        {
            Inventory.Instance.RemoveItemInInventory(item.ItemName);
        }
    }

    public virtual void Action()
    {
    }

    protected virtual void PlayUnableAnimation()
    {
        GameManager.Instance.GetPlayer().PlayItemAnimation(Helpers.ItemType.Generic);
    }
}
