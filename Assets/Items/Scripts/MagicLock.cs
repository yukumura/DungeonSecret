using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLock : Actionable
{
    [SerializeField]
    GameObject magicBook;
    [SerializeField]
    float timeToUnlockBook = 3f;
    public override void Action()
    {
        StartCoroutine(EnableBook(timeToUnlockBook));
        gameObject.SetActive(false);
    }

    IEnumerator EnableBook(float time)
    {
        yield return new WaitForSeconds(time);
        magicBook.GetComponent<SphereCollider>().enabled = true;
    }
}
