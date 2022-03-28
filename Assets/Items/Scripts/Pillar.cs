using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : Actionable
{
    public bool rightPosition = false;
    [SerializeField]
    private int currentNumber = 1;
    [SerializeField]
    private int rightNumber;
    [SerializeField]
    private float time;


    public override void Action()
    {
        currentNumber++;
        if (currentNumber > 4)
            currentNumber = 1;

        StartCoroutine(RotateMe(Vector3.up * 90, time));
        isUsed = false;

        rightPosition = currentNumber == rightNumber;
    }

    IEnumerator RotateMe(Vector3 byAngles, float inTime)
    {
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }
    }

    public void Use()
    {
        isUsed = true;
    }
}
