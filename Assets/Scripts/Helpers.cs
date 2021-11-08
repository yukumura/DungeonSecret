using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45.0f, 0));
    public static Vector3 ToIso(this Vector3 vector) => _isoMatrix.MultiplyPoint3x4(vector);
}
