using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{

    [SerializeField]
    public bool isOpen = false;
    [SerializeField]
    AudioClip audioMenu;
    [SerializeField]
    GameObject panel;
    [SerializeField]
    Image commands;
    [SerializeField]
    Button gameButton;

    private bool canTriggerAgain = true;

    private void Awake()
    {
        panel = GameObject.FindGameObjectWithTag(Helpers.CommandsTag);
        panel.SetActive(false);
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
                panel.transform.LeanScale(Vector3.zero, .3f).setOnComplete(OnComplete);
                StartCoroutine(CanTriggerAgain());
            }
            else
            {
                //transform.LeanMoveLocal(new Vector2(transform.localPosition.x + 1900, transform.localPosition.y), 1f).setEaseInOutBack();
                panel.SetActive(true);
                isOpen = true;
                panel.transform.LeanScale(Vector3.one, .3f);
                StartCoroutine(CanTriggerAgain());
                commands.gameObject.SetActive(false);                
                gameButton.Select();
            }

            SFXManager.Instance.Audio.PlayOneShot(audioMenu);
            GameManager.Instance.PauseGame();
        }
    }

    IEnumerator CanTriggerAgain()
    {
        yield return new WaitForSeconds(.5f);
        canTriggerAgain = true;
    }

    void OnComplete()
    {
        panel.SetActive(false);
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene(Helpers.MainMenuName);
    }

    public void TriggerCommands()
    {
        commands.gameObject.SetActive(!commands.gameObject.activeSelf);

        if (commands.gameObject.activeSelf)
            commands.gameObject.GetComponent<Button>().Select();
        else
        {
            gameButton.Select();
        }
    }

}
