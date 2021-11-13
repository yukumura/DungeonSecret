using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    [SerializeField]
    Transform startPoint;
    [SerializeField]
    Transform endPoint;
    [SerializeField]
    float speed = 1;

    Vector3 currentPos;

    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentPos = Vector3.Lerp(startPoint.position, endPoint.position, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
        transform.position = currentPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Helpers.PlayerTag))
        {
            other.transform.parent = transform;
            Camera.main.GetComponent<CinemachineBrain>().m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Helpers.PlayerTag))
        {
            other.transform.parent = null;
            Camera.main.GetComponent<CinemachineBrain>().m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;
        }
    }
}
