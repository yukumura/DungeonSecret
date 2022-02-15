using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightEffect : MonoBehaviour
{
    HDAdditionalLightData light;
    [SerializeField]
    float minimum = 2000f;
    [SerializeField]
    float maximum = 3500f;
    float t = 0.0f;
    [SerializeField]
    float speed;

    // Start is called before the first frame update
    void Awake()
    {
        light = GetComponent<HDAdditionalLightData>();
    }

    // Update is called once per frame
    void Update()
    {
        light.SetIntensity(Mathf.Lerp(minimum, maximum, t));
        t += speed * Time.deltaTime;

        if (t > 1.0f)
        {
            float temp = maximum;
            maximum = minimum;
            minimum = temp;
            t = 0.0f;
        }
    }
}
