using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Helpers;

public class Item : MonoBehaviour
{
    [Header("Thoughts Settings")]
    [SerializeField]
    [TextArea]
    protected string message;
    [SerializeField]
    protected float timeToFadeThoughts;

    [Header("Item Type")]
    public ItemType type;
    [SerializeField]
    protected bool isUsed = false;
    public bool IsUsed
    {
        get
        {
            return isUsed;
        }
        set
        {
            isUsed = value;
        }
    }

    void Awake()
    {
    }


    public virtual void Trigger()
    {
        GameManager.Instance.GetPlayer().PlayItemAnimation(type);
    }

    public virtual void ShowIcon()
    {

    }
}
