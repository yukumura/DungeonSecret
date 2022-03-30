using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commands : MonoBehaviour
{

    [SerializeField]
    public static bool isOpen = false;

    private bool canTriggerAgain = true;

    public void Trigger()
    {
        if (canTriggerAgain)
        {
            canTriggerAgain = false;

            if (isOpen)
            {
                transform.LeanMoveLocal(new Vector2(transform.localPosition.x - 1900, transform.localPosition.y), 1f).setEaseInOutBack();
                isOpen = false;
                StartCoroutine(CanTriggerAgain());
            }
            else
            {
                transform.LeanMoveLocal(new Vector2(transform.localPosition.x + 1900, transform.localPosition.y), 1f).setEaseInOutBack();
                isOpen = true;
                StartCoroutine(CanTriggerAgain());
            }
        }
    }

    IEnumerator CanTriggerAgain()
    {
        yield return new WaitForSeconds(.5f);
        canTriggerAgain = true;
    }
}
