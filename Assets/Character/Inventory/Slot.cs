using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField]
    private Image iconItem;
    [SerializeField]
    private bool isEmpty = true;


    void Awake()
    {
    }

    public bool IsEmpty { get { return isEmpty; } }

    public void SetItem(Sprite item)
    {
        iconItem.sprite = item;
        iconItem.enabled = true;
        isEmpty = false;        
    }

    public void RemoveItem()
    {
        iconItem.enabled = false;
        iconItem.sprite = null;
        isEmpty = true;
    }
}
