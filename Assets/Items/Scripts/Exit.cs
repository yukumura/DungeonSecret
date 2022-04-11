using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Actionable
{
    public override void Action()
    {
        StartCoroutine(FinishGame());
    }

    IEnumerator FinishGame()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.FinishGame();
        gameObject.SetActive(false);
    }
}
