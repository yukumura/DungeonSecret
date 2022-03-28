using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grilles : Actionable
{
    Animator animator;
    [SerializeField]
    [TextArea]
    string finishMessage;
    [SerializeField]
    GameObject finishGO;
    // Start is called before the first frame update

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public override void Action()
    {
        animator.Play(Helpers.GrillesOpenAnimation);
    }


    public void ActivateEnd()
    {
        GameManager.Instance.GetPlayerUI().SetMessage(finishMessage, timeToFadeThoughts);
        GetComponent<BoxCollider>().enabled = false;
        finishGO.SetActive(true);
    }
}
