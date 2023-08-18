using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GenreAppObject : AppObject
{
    public bool isEnabled;
    public TextMeshProUGUI genreText;
    public Image genreCoverImage;

    private List<MwsiveTrack> mwsiveTrackList = new List<MwsiveTrack>();
    private List<string> genreID = new List<string>();

    public override void Initialize(params object[] list)
    {
        genreText.text = (string)list[0];
        foreach (MwsiveTrack mwsiveTrack in (List<MwsiveTrack>)list[1])
        {
            mwsiveTrackList.Add(mwsiveTrack);
        }
    }

    public void OnSelectedPlaylistClick()
    {
        InvokeEvent<OnSelectedPlaylistClick>(new OnSelectedPlaylistClick(isEnabled));

    }

    public void OnClick_Genre()
    {
        for (int i = 0; i < mwsiveTrackList.Count; i++)
        {
            genreID.Add(mwsiveTrackList[i].spotify_track_id);
        }

        NewScreenManager.instance.ChangeToSpawnedView("genre");
        NewScreenManager.instance.GetCurrentView().GetComponent<GenreViewModel>().GetSeveralTracks(genreID.ToArray(), genreText.text);
    }
}
