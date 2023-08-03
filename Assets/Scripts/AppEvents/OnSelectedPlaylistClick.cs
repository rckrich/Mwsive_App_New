using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSelectedPlaylistClick : AppEvent
{
    // Start is called before the first frame update
    private bool enabled;
    public OnSelectedPlaylistClick(bool _enabled) : base(_enabled)
    {
        enabled = _enabled;
    }

    public bool Enabled() { return enabled; }
}
