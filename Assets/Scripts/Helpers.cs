using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public enum ItemType
    {
        //PickupFromGround,
        //PickupFromMiddle,
        //ActionableDoor,
        Generic,        
        //OpeningChest,        
        //CloseChest,        
    }

    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 135f, 0));
    private static Matrix4x4 _isoMatrix2 = Matrix4x4.Rotate(Quaternion.Euler(0, 45f, 0));
    public static Vector3 ToIso(this Vector3 vector) => _isoMatrix.MultiplyPoint3x4(vector);
    public static Vector3 ToIso2(this Vector3 vector) => _isoMatrix2.MultiplyPoint3x4(vector);

    public static string GameSceneName
    {
        get
        {
            return "High Details";
        }
    }

    public static string PlayerTag
    {
        get
        {
            return "Player";
        }
    }
    public static string CommandsTag
    {
        get
        {
            return "Commands";
        }
    }

    public static string InventoryTag
    {
        get
        {
            return "Inventory";
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

    public static string generalIcon
    {
        get
        {
            return "general";
        }
    }

    public static string JsonPath
    {
        get
        {
            return "./Assets/Items/Descriptions/";
        }
    }

    public static string English
    {
        get
        {
            return "English.json";
        }
    }

    public static string Italian
    {
        get
        {
            return "Italiano.json";
        }
    }

    public static string TreasureOpenAnimation
    {
        get
        {
            return "Treasure Open";
        }
    }

    public static string PlayerIsWalkingAnimation
    {
        get
        {
            return "isWalking";
        }
    }
    public static string PlayerIsRunningAnimation
    {
        get
        {
            return "isRunning";
        }
    }
    public static string TryToOpenTheDoorAnimation
    {
        get
        {
            return "TryToOpenTheDoor";
        }
    }
    public static string PickingItemsFromGroundAnimation
    {
        get
        {
            return "PickingItemsFromGround";
        }
    }
    public static string LookAroundAnimation
    {
        get
        {
            return "Looking";
        }
    }
    public static string PickingItemsFromMiddleAnimation
    {
        get
        {
            return "PickingItemsFromMiddle";
        }
    }
    public static string OpeningChestAnimation
    {
        get
        {
            return "OpeningChest";
        }
    }
    public static string GetUpAnimation
    {
        get
        {
            return "GetUp";
        }
    }
    public static string GrillesOpenAnimation
    {
        get
        {
            return "Grilles Open";
        }
    }
    public static string WindowGlassDisappearAnimation
    {
        get
        {
            return "WindowGlass Disappear";
        }
    }
}
