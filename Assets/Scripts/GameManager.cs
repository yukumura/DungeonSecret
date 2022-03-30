using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    public bool finishGame = false;
    [SerializeField]
    public bool startGameIntro = true;

    ManagePlayerUI playerUI;
    PlayerController playerController;
    [SerializeField]
    Commands commandsController;


    void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        Instance = this;

        playerUI = GameObject.FindGameObjectWithTag(Helpers.PlayerTag).GetComponent<ManagePlayerUI>();
        playerController = GameObject.FindGameObjectWithTag(Helpers.PlayerTag).GetComponent<PlayerController>();
        commandsController = GameObject.FindGameObjectWithTag(Helpers.CommandsTag).GetComponent<Commands>();
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

    public void FinishGame()
    {

        Time.timeScale = 0;
        finishGame = true;
    }

    public void TriggerCommands()
    {
        commandsController.Trigger();
    }
    
}
