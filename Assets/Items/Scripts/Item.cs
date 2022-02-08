using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Settings")]
    
    [SerializeField]
    protected Inventory inventory;

    void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag(Helpers.PlayerTag).GetComponent<Inventory>();
    }


    public virtual void Action()
    {
    }

    public virtual void ShowIcon()
    {

    }
}
