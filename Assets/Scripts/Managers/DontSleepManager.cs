using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontSleepManager : MonoBehaviour
{
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

}
