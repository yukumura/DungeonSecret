using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightEffect : MonoBehaviour
{
    HDAdditionalLightData lightData;
    [SerializeField]
    float minValue = 6f;
    [SerializeField]
    float maxValue = 7f;
    [SerializeField]
    float durationInSeconds = .4f;

    bool delayedStart = false;

    // Start is called before the first frame update
    void Awake()
    {
        lightData = GetComponent<HDAdditionalLightData>();
        float random = Random.Range(0f, 1f);
        StartCoroutine(Cooldown(random));
    }

    // Update is called once per frame
    void Update()
    {
        if (delayedStart)
            lightData.luxAtDistance = Mathf.Lerp(minValue, maxValue, Mathf.PingPong(Time.time / durationInSeconds, 1));

    }

    IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        delayedStart = true;
    }
}
