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
    [SerializeField]
    [TextArea]
    string message;

    bool enigmaSolved = false;

    private void CheckCombination()
    {
        foreach (Pillar pillar in pillars)
        {

            if (!pillar.rightPosition)
                return;
        }

        EnigmaSolved();
    }

    private void EnigmaSolved()
    {
        enigmaSolved = true;
        //Array.ForEach(pillars, x => x.Use());
        foreach (var pillar in pillars)
        {
            pillar.Use();
        }
        windowGlass.Disappear();
        GameManager.Instance.SetCharacterThoughts(message, 3f);
    }

    private void Update()
    {
        if (!enigmaSolved)
            CheckCombination();
    }
}
