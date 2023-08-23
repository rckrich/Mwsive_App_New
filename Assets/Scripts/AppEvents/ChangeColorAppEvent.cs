using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorAppEvent : AppEvent
{
    public Color color;

    public ChangeColorAppEvent(Color _color) : base(_color)
    {
        color = _color;
    }


}
