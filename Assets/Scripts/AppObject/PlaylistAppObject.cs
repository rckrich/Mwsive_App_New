using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaylistAppObject : AppObject
{
    public bool isEnabled;
    public TextMeshProUGUI playlistText;

    private List<MwsiveTrack> mwsiveTrackList = new List<MwsiveTrack>();

    public override void Initialize(params object[] list)
    {
        playlistText.text = (string)list[0];
        foreach (MwsiveTrack mwsiveTrack in (List<MwsiveTrack>)list[1])
        {
            mwsiveTrackList.Add(mwsiveTrack);
        }
    }

    public void OnSelectedPlaylistClick()
    {
        InvokeEvent<OnSelectedPlaylistClick>(new OnSelectedPlaylistClick(isEnabled));
        
    }
}
