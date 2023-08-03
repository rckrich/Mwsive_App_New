using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedPlaylistNameAppEvent : AppEvent
{
    public string playlistName;

    public SelectedPlaylistNameAppEvent(string _playlistName) : base(_playlistName)
    {
        playlistName = _playlistName;
    }
}
