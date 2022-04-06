using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Helpers;

public class Item : MonoBehaviour
{
    [Header("Thoughts Settings")]
    [SerializeField]
    [TextArea]
    protected string message;
    [SerializeField]
    protected float timeToFadeThoughts;

    [Header("Item Type")]
    public ItemType type;
    [SerializeField]
    protected bool isUsed = false;
    [SerializeField]
    bool isReusable = true;

    [Header("Audio")]
    [SerializeField]
    AudioClip interactAudio;

    protected Coroutine reusable;

    public bool CanPlayerMoves;
    public bool IsUsed
    {
        get
        {
            return isUsed;
        }
        set
        {
            isUsed = value;
        }
    }

    void Awake()
    {
    }


    public virtual void Trigger()
    {
        //GameManager.Instance.GetPlayer().PlayItemAnimation(type);
        
        if (interactAudio != null)
            SFXManager.Instance.Audio.PlayOneShot(interactAudio);

        isUsed = true;
        reusable = StartCoroutine(SetReusable(timeToFadeThoughts + 1));
    }

    public virtual void ShowIcon()
    {
        if (!isUsed)
            GameManager.Instance.ShowIconInGame(readIcon);
    }

    IEnumerator SetReusable(float time)
    {
        yield return new WaitForSeconds(time);
        isUsed = false;
    }
}
