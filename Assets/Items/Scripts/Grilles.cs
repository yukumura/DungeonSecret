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
    [SerializeField]
    AudioClip audioFormula;
    // Start is called before the first frame update

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public override void Action()
    {
        SFXManager.Instance.Audio.PlayOneShot(audioFormula);
        animator.Play(Helpers.GrillesOpenAnimation);
        base.Action();
    }


    public void ActivateEnd()
    {
        GameManager.Instance.GetPlayerUI().SetMessage(finishMessage, timeToFadeThoughts);
        GetComponent<BoxCollider>().enabled = false;
        finishGO.SetActive(true);
    }
}
