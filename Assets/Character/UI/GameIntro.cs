using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameIntro : MonoBehaviour
{
    [SerializeField]
    Image blackScreen;
    [SerializeField]
    TextMeshProUGUI text;
    [SerializeField]
    string[] story;
    [SerializeField]
    float timeBetweenText;
    Queue<string> textToShow = new Queue<string>();
    bool canChangeText = true;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (string item in story)
        {
            textToShow.Enqueue(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canChangeText)
        {
            canChangeText = false;
            StartCoroutine(Cooldown(timeBetweenText));
            if (textToShow.Count > 0)
                text.text = textToShow.Dequeue();
            else
            {
                canChangeText = false;
                StartCoroutine(FadeOut());
            }
        }

    }

    IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canChangeText = true;
    }

    IEnumerator FadeOut(float fadeSpeed = 0.5f)
    {
        text.text = "";
        float fadeAmount;
        Color objectColor = blackScreen.color;
        while (blackScreen.color.a > 0)
        {
            fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackScreen.color = objectColor;
            yield return null;
        }

        GameManager.Instance.startGameIntro = false;
        gameObject.SetActive(false);
    }
}
