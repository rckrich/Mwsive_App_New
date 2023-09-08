using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDiskAppEvent : AppEvent
{
    private int totalDisk;

    public ChangeDiskAppEvent(int _totalDisk):base (_totalDisk)
    {
        totalDisk = _totalDisk;
    }

}
