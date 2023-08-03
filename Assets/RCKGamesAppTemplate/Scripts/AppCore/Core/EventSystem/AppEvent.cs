using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppEvent
{

    private object[] list;

    public AppEvent(params object[] list)
    {
        this.list = list;
    }

    public object[] GetParameters()
    {
        return list;
    }

}
