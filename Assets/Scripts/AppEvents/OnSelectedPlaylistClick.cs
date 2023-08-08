using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSelectedPlaylistClick : AppEvent
{
    
    private bool enabled;
    public OnSelectedPlaylistClick(bool _enabled) : base(_enabled)
    {
        enabled = _enabled;
    }

    public bool Enabled() { return enabled; }
}
