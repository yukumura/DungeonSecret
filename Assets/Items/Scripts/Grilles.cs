using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grilles : Actionable
{
    Animator animator;
    [SerializeField]
    [TextArea]
    string finishMessage;
    // Start is called before the first frame update

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public override void Action()
    {
        animator.Play(Helpers.GrillesOpenAnimation);
        StartCoroutine(WaitAndFinishGame());
    }

    IEnumerator WaitAndFinishGame()
    {
        yield return new WaitForSeconds(15f);
        gameObject.SetActive(false);
        GameManager.Instance.GetPlayerUI().SetMessage(finishMessage, timeToFadeThoughts);
    }
}
