using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDiskAppEvent : AppEvent
{
    public int totalDisk;
    public string type;


    public ChangeDiskAppEvent(int _totalDisk, string _type = null):base (_totalDisk, _type)
    {
        totalDisk = _totalDisk;
        type = _type;
    }

}
