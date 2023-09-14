using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogManager : MonoBehaviour
{
    private static DebugLogManager _instance;
    public static DebugLogManager instance { get
        {
            if (_instance == null) _instance = GameObject.FindObjectOfType<DebugLogManager>();
            return _instance;
        }
    }

    public bool canPrint = true;

    public void DebugLog(object _message)
    {
        if(canPrint)
            Debug.Log(_message);
    }

    public void DebugLogWarning(object _message)
    {
        if (canPrint)
            Debug.LogWarning(_message);
    }

    public void DebugLogError(object _message)
    {
        if (canPrint)
            Debug.LogError(_message);
    }
}
