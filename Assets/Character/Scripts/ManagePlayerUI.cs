using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagePlayerUI : MonoBehaviour
{
    [SerializeField]
    Image prefabUI;
    [SerializeField]
    Image uiUse;
    [SerializeField]
    Transform character;
    [SerializeField]
    Vector3 offset = new Vector3(0, 1.5f, 0);

    // Start is called before the first frame update
    void Start()
    {
        uiUse = Instantiate(prefabUI, FindObjectOfType<Canvas>().transform).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        uiUse.transform.position = Camera.main.WorldToScreenPoint(character.position + offset);
    }
}
