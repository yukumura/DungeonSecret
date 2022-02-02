using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagePlayerUI : MonoBehaviour
{
    //[SerializeField]
    //Image prefabUI;
    [SerializeField]
    TextMeshProUGUI prefabUI;
    [SerializeField]
    Vector3 offset = new Vector3(0, 2.25f, 0);

    Image uiUse;
    [SerializeField]
    TextMeshProUGUI text;
    Transform character;
    CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Awake()
    {
        text = Instantiate(prefabUI, FindObjectOfType<Canvas>().transform).GetComponent<TextMeshProUGUI>();
        character = GameObject.FindGameObjectWithTag(Helpers.PlayerTag).GetComponent<Transform>();
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
        text.gameObject.SetActive(true);
        canvasGroup.LeanAlpha(1, 0.8f);
        text.text = message;
        StartCoroutine(HideMessage(time));
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
}
