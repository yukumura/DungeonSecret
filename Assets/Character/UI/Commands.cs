using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commands : MonoBehaviour
{

    [SerializeField]
    public static bool isOpen = false;
    [SerializeField]
    AudioClip audioMenu;
    [SerializeField]
    GameObject commands;

    private bool canTriggerAgain = true;

    private void Awake()
    {
        commands.SetActive(false);
    }

    public void Trigger()
    {
        if (canTriggerAgain)
        {
            canTriggerAgain = false;

            if (isOpen)
            {
                //transform.LeanMoveLocal(new Vector2(transform.localPosition.x - 1900, transform.localPosition.y), 1f).setEaseInOutBack();
                isOpen = false;
                commands.transform.LeanScale(Vector3.zero, .3f).setOnComplete(OnComplete);
                StartCoroutine(CanTriggerAgain());
            }
            else
            {
                //transform.LeanMoveLocal(new Vector2(transform.localPosition.x + 1900, transform.localPosition.y), 1f).setEaseInOutBack();
                commands.SetActive(true);
                isOpen = true;
                commands.transform.LeanScale(Vector3.one, .3f);
                StartCoroutine(CanTriggerAgain());
            }

            SFXManager.Instance.Audio.PlayOneShot(audioMenu);
        }
    }

    IEnumerator CanTriggerAgain()
    {
        yield return new WaitForSeconds(.5f);
        canTriggerAgain = true;
    }

    void OnComplete()
    {
        commands.SetActive(false);
    }
}
