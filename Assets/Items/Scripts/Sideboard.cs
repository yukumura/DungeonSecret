using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sideboard : Actionable
{
    [SerializeField]
    GameObject corpseToHide;

    [SerializeField]
    GameObject skullToShow;

    [SerializeField]
    GameObject magicKey;

    [SerializeField]
    GameObject goldenSpoon;

    public override void Action()
    {
        corpseToHide.SetActive(false);
        skullToShow.SetActive(true);
        magicKey.SetActive(true);
        goldenSpoon.SetActive(true);
    }
}
