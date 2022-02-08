using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 135f, 0));
    private static Matrix4x4 _isoMatrix2 = Matrix4x4.Rotate(Quaternion.Euler(0, 45f, 0));
    public static Vector3 ToIso(this Vector3 vector) => _isoMatrix.MultiplyPoint3x4(vector);
    public static Vector3 ToIso2(this Vector3 vector) => _isoMatrix2.MultiplyPoint3x4(vector);

    public static string PlayerTag
    {
        get
        {
            return "Player";
        }
    }

    public static string UIItemIconName
    {
        get
        {
            return "ItemIcon";
        }
    }

    public static string actionIcon
    {
        get
        {
            return "action";
        }
    }
    public static string readIcon
    {
        get
        {
            return "read";
        }
    }
    public static string pickupIcon
    {
        get
        {
            return "pickup";
        }
    }
}
