using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : Actionable
{
    [SerializeField]
    GameObject goldenSpoon;

    Animator animator;
    int openHash;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        openHash = Animator.StringToHash(Helpers.TreasureOpenAnimation);

    }

    public override void Action()
    {
        animator.Play(openHash);
    }

    public void ActivateGoldenSpoon()
    {
        goldenSpoon.GetComponent<SphereCollider>().enabled = true;
    }

    protected override void PlayUnableAnimation()
    {
        GameManager.Instance.GetPlayer().PlayItemAnimation(Helpers.ItemType.CloseChest);
    }
}
