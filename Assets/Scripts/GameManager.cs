using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    ManagePlayerUI characterToughts;

    void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        Instance = this;

        characterToughts = GameObject.FindGameObjectWithTag(Helpers.PlayerTag).GetComponent<ManagePlayerUI>();
    }

    public void SetCharacterThoughts(string message)
    {
        characterToughts.SetMessage(message);
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
