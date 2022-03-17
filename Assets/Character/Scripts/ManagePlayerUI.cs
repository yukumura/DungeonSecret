using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagePlayerUI : MonoBehaviour
{
    [Header("Player Thoughts Settings")]
    [SerializeField]
    TextMeshProUGUI prefabUI;
    [SerializeField]
    Vector3 offset = new Vector3(0, 2.25f, 0);
    [SerializeField]
    TextMeshProUGUI text;

    Image uiUse;
    Transform character;
    CanvasGroup canvasGroup;

    [Header("Player Icons Settings")]
    [SerializeField]
    GameObject iconInGame;
    [SerializeField]
    Sprite action;
    [SerializeField]
    Sprite read;
    [SerializeField]
    Sprite pickup;
    [SerializeField]
    float secondsToFade = .7f;

    Coroutine hideIconInGameRoutine;
    Coroutine hideMessageRoutine;
    GameObject instantiatedIconInGame;


    // Start is called before the first frame update
    void Awake()
    {
        character = GameObject.FindGameObjectWithTag(Helpers.PlayerTag).GetComponent<Transform>();
        text = Instantiate(prefabUI, FindObjectOfType<Canvas>().transform).GetComponent<TextMeshProUGUI>();
        text.gameObject.SetActive(false);
        instantiatedIconInGame = Instantiate(iconInGame, transform.position + offset, Quaternion.identity, character);
        instantiatedIconInGame.LeanScale(new Vector3(.9f, .9f, .9f), .8f).setEaseLinear().setLoopPingPong();
        instantiatedIconInGame.SetActive(false);
        canvasGroup = text.gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        text.transform.position = Camera.main.WorldToScreenPoint(character.position + offset);
    }

    public void SetMessage(string message, float time)
    {
        if(hideMessageRoutine != null)
        {
            StopCoroutine(hideMessageRoutine);
        }

        text.gameObject.SetActive(true);
        canvasGroup.LeanAlpha(1, 0.8f);
        text.text = message;
        hideMessageRoutine = StartCoroutine(HideMessage(time));
    }

    IEnumerator HideMessage(float time)
    {
        yield return new WaitForSeconds(time);
        canvasGroup.LeanAlpha(0, 0.8f).setOnComplete(OnComplete);
    }

    void OnComplete()
    {
        text.gameObject.SetActive(false);
    }

    public void ShowIconInGame(string iconName)
    {
        if (!instantiatedIconInGame.activeSelf)
        {
            instantiatedIconInGame.GetComponent<SpriteRenderer>().sprite = iconName == Helpers.actionIcon ? action : iconName == Helpers.pickupIcon ? pickup : read;
            instantiatedIconInGame.SetActive(true);
        }
    }

    public void HideIconInGame()
    {
        hideIconInGameRoutine = StartCoroutine(HideIconInGameRoutine());
    }

    protected IEnumerator HideIconInGameRoutine()
    {
        yield return new WaitForSeconds(secondsToFade);
        if (instantiatedIconInGame != null)
            instantiatedIconInGame.SetActive(false);
    }

    public void ClearReference()
    {
        if (hideIconInGameRoutine != null)
        {
            StopCoroutine(hideIconInGameRoutine);
        }

        instantiatedIconInGame.SetActive(false);
    }
}
