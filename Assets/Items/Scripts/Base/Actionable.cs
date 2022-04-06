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
    [Header("Audio Settings")]
    [SerializeField]
    AudioClip audioUsage;

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
                base.Trigger();
                Action();
            }
            else
            {
                GameManager.Instance.GetPlayerUI().SetMessage(unableMessage, timeToFadeThoughts);
                //PlayUnableAnimation();
            }
        }
            
        GameManager.Instance.ClearReference();
    }

    //public override void ShowIcon()
    //{
    //    if (!isUsed)
    //        GameManager.Instance.ShowIconInGame(Helpers.actionIcon);
    //}

    private void RemoveRequiredItemsFromInventory()
    {
        foreach (Pickup item in itemsRequired)
        {
            Inventory.Instance.RemoveItemInInventory(item.ItemName);
        }
    }

    public virtual void Action()
    {
        if (reusable != null)
            StopCoroutine(reusable);

        isUsed = true;

        if (audioUsage != null)
            SFXManager.Instance.Audio.PlayOneShot(audioUsage);
    }

    protected virtual void PlayUnableAnimation()
    {
        //GameManager.Instance.GetPlayer().PlayItemAnimation(Helpers.ItemType.Generic);
    }
}
