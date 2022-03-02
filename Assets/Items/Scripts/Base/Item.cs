using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Thoughts Settings")]
    [SerializeField]
    [TextArea]
    protected string message;
    [SerializeField]
    protected float timeToFadeThoughts; 

    void Awake()
    {
    }


    public virtual void Trigger()
    {
    }

    public virtual void ShowIcon()
    {

    }
}
