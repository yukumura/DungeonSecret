using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource Audio;

    public static SFXManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); return;
        }

        Instance = this;
        Audio = GetComponent<AudioSource>();
        DontDestroyOnLoad(this);
    }
}