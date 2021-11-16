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
    [SerializeField]
    int secondsToFade = 3;

    Image uiUse;
    [SerializeField]
    TextMeshProUGUI text;
    Transform character;

    // Start is called before the first frame update
    void Awake()
    {
        //uiUse = Instantiate(prefabUI, FindObjectOfType<Canvas>().transform).GetComponent<Image>();
        //text = uiUse.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        text = Instantiate(prefabUI, FindObjectOfType<Canvas>().transform).GetComponent<TextMeshProUGUI>();
        character = GameObject.FindGameObjectWithTag(Helpers.PlayerTag).GetComponent<Transform>();
        text.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        text.transform.position = Camera.main.WorldToScreenPoint(character.position + offset);
    }

    public void SetMessage(string message)
    {
        text.gameObject.SetActive(true);
        text.text = message;
        StartCoroutine(HideMessage());
    }

    IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(secondsToFade);
        text.gameObject.SetActive(false);
    }
}
