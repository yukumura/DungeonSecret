using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    public bool finishGame = false;
    [SerializeField]
    public bool pausedGame = false;
    [SerializeField]
    Image blackScreen;
    [SerializeField]
    TextMeshProUGUI finishGameTimer;
    [SerializeField]
    Button buttonBack;

    ManagePlayerUI playerUI;
    PlayerController playerController;
    [SerializeField]
    GameMenu gameMenuController;

    void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        Instance = this;

        playerUI = GameObject.FindGameObjectWithTag(Helpers.PlayerTag).GetComponent<ManagePlayerUI>();
        playerController = GameObject.FindGameObjectWithTag(Helpers.PlayerTag).GetComponent<PlayerController>();
        gameMenuController = GetComponent<GameMenu>();
        finishGame = false;
        pausedGame = false;
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
        StartCoroutine(FadeIn());
        finishGame = true;
    }

    public void PauseGame()
    {
        pausedGame = !pausedGame;
    }

    public void TriggerMenu()
    {
        gameMenuController.Trigger();
    }

    IEnumerator FadeIn(float fadeSpeed = .5f)
    {
        float fadeAmount;
        Color objectColor = blackScreen.color;
        while (blackScreen.color.a < 1)
        {
            fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackScreen.color = objectColor;
            yield return null;
        }

        StartCoroutine(Cooldown(3f));
    }

    IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        var ts = TimeSpan.FromSeconds(Time.timeSinceLevelLoad);
        finishGameTimer.text = string.Format("You have completed this introduction. \n \n Congratulations! \n \n Your completion time is {0:00}:{1:00}", ts.Minutes, ts.Seconds);
        buttonBack.gameObject.SetActive(true);
        buttonBack.Select();
    }
}
