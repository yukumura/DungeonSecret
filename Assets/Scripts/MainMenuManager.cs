using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    Button creditsButton;
    [SerializeField]
    Button creditsBackButton;
    [SerializeField]
    GameObject creditsPanel;

    private void Awake()
    {
        creditsPanel.SetActive(false);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(Helpers.GameSceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Credits()
    {
        creditsPanel.SetActive(true);
        creditsBackButton.Select();
    }

    public void Back()
    {
        creditsPanel.SetActive(false);
        creditsButton.Select();
    }
}
