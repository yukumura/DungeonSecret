using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool isEnglish = true;

    ManagePlayerUI playerUI;
    PlayerController playerController;


    void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        Instance = this;

        playerUI = GameObject.FindGameObjectWithTag(Helpers.PlayerTag).GetComponent<ManagePlayerUI>();
        playerController = GameObject.FindGameObjectWithTag(Helpers.PlayerTag).GetComponent<PlayerController>();
    }

    public void SetCharacterThoughts(string message, float time)
    {
        playerUI.SetMessage(message, time);
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void ClearReference()
    {
        playerUI.ClearReference();
    }

    public void ShowIconInGame(string action)
    {
        playerUI.ShowIconInGame(action);
    }

    public void HideIconInGame()
    {
        playerUI.HideIconInGame();
    }

    public PlayerController GetPlayer()
    {
        return playerController;
    }

    public ManagePlayerUI GetPlayerUI()
    {
        return playerUI;
    }
    
}
