using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Readable : Item
{
    // Start is called before the first frame update
    public override void Trigger()
    {
        GameManager.Instance.SetCharacterThoughts(message, timeToFadeThoughts);
        GameManager.Instance.ClearReference();
        base.Trigger();
    }

    public override void ShowIcon()
    {
        GameManager.Instance.ShowIconInGame(Helpers.readIcon);
    }
}
