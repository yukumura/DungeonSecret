using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField]
    private Image itemIcon;
    [SerializeField]
    private string itemName;
    [SerializeField]
    private bool isEmpty = true;


    void Awake()
    {
        //Get component Image from child
        itemIcon = transform.Find(Helpers.UIItemIconName).GetComponent<Image>();
    }

    public bool IsEmpty { get { return isEmpty; } }

    public string ItemName { get { return itemName; } }

    public void SetItem(Sprite item, string name)
    {
        itemIcon.sprite = item;
        itemIcon.enabled = true;
        itemName = name;
        isEmpty = false;        
    }

    public void RemoveItem()
    {
        itemIcon.enabled = false;
        itemIcon.sprite = null;
        itemName = string.Empty;
        isEmpty = true;
    }
}
