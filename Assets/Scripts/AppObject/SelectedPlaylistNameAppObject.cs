using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedPlaylistNameAppObject : AppObject
{
   public string playlistName;

    public void SelectedPlaylistNameAppEvent()
    {
        InvokeEvent<SelectedPlaylistNameAppEvent>(new SelectedPlaylistNameAppEvent(playlistName));
    }
}
