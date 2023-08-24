using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorAppEvent : AppEvent
{
    public Color color;
    public Color color2;

    public ChangeColorAppEvent(Color _color, Color _color2) : base(_color, _color2)
    {
        color = _color;
        color2 = _color2;
    }


}
