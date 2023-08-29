using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaylistAppObject : AppObject
{
    public bool isEnabled;
    public TextMeshProUGUI playlistText;
    public Image playlistCoverImage;

    private List<MwsiveTrack> mwsiveTrackList = new List<MwsiveTrack>();
    private List<string> trackID = new List<string>();
    private RecommendedPlaylist playlist;
    public override void Initialize(params object[] list)
    {

        playlist = (RecommendedPlaylist)list[0];
        

        if (playlist.name != null)
        {
            if (playlist.name.Length > 27)
            {
                string _text2 = "";
                for (int i = 0; i < 27; i++)
                {
                    _text2 = _text2 + playlist.name[i];
                }
                _text2 = _text2 + "...";
                playlistText.text = _text2;
            }
            else
            {
                playlistText.text = playlist.name;
            }
        }

        if(playlist.image_url != null)
            ImageManager.instance.GetImage(playlist.image_url, playlistCoverImage, (RectTransform)this.transform);

        foreach (MwsiveTrack mwsiveTrack in (List<MwsiveTrack>)list[1])
        {
            mwsiveTrackList.Add(mwsiveTrack);
        }
    }

    public void OnSelectedPlaylistClick()
    {
        InvokeEvent<OnSelectedPlaylistClick>(new OnSelectedPlaylistClick(isEnabled));
    }

    public void OnClick_Playlist()
    {
        for(int i = 0; i < mwsiveTrackList.Count; i++)
        {
            trackID.Add(mwsiveTrackList[i].spotify_track_id);
        }

        NewScreenManager.instance.ChangeToSpawnedView("playlist");
        NewScreenManager.instance.GetCurrentView().GetComponent<PlaylistViewModel>().listenOnSpotify.SetActive(false);
        NewScreenManager.instance.GetCurrentView().GetComponent<PlaylistViewModel>().GetSeveralTracks(trackID.ToArray());
        trackID.Clear();
    }
}
