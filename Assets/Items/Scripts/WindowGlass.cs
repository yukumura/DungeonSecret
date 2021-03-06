using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowGlass : Readable
{
    [SerializeField]
    GameObject bookLock;
    Animator anim;
    [SerializeField]
    AudioClip audioDisappear;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Disappear()
    {
        SFXManager.Instance.Audio.PlayOneShot(audioDisappear);
        anim.Play(Helpers.WindowGlassDisappearAnimation);
    }

    public void TurnOff()
    {
        gameObject.SetActive(false);
        bookLock.GetComponent<SphereCollider>().enabled = true;
    }
}
