using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBook : Pickup
{
    [SerializeField]
    GameObject endCollider;

    public override void Pick()
    {
        base.Pick();
        endCollider.SetActive(true);
    }
}
