using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PillarManager : MonoBehaviour
{
    [SerializeField]
    Pillar[] pillars;
    [SerializeField]
    WindowGlass windowGlass;
    bool enigmaSolved = false;

    private void CheckCombination()
    {
        foreach (Pillar pillar in pillars)
        {

            if (!pillar.rightPosition)
                return;
        }

        enigmaSolved = true;
        Array.ForEach(pillars, x => x.Use());
        windowGlass.Disappear();
    }

    private void Update()
    {
        if (!enigmaSolved)
            CheckCombination();
    }
}
