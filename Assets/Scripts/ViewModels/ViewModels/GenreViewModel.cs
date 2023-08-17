using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenreViewModel : ViewModel
{
    [Header("Instance Referecnes")]
    public GameObject trackHolderPrefab;
    public Transform instanceParent;
    public string artists;

    void Start()
    {
        
    }

    public void GetSeveralTracks(string[] _genreID)
    {
        SpotifyConnectionManager.instance.GetSeveralTracks(_genreID, Callback_GetSeveralTracks);
    }

    public void Callback_GetSeveralTracks(object[] _value)
    {
        SeveralTrackRoot severalTrackRoot = (SeveralTrackRoot)_value[1];

        foreach (Track track in severalTrackRoot.tracks)
        {
            if (track != null)
            {
                TrackHolder instance = GameObject.Instantiate(trackHolderPrefab, instanceParent).GetComponent<TrackHolder>();
                artists = "";
                foreach (Artist artist in track.artists) { artists += artist.name + ", "; }
                instance.Initialize(track.name, artists, track.id, track.artists[0].id, track.uri, track.preview_url, track.external_urls);
                if (track.album.images != null && track.album.images.Count > 0)
                    instance.SetImage(track.album.images[0].url);
                if (track.preview_url == null)
                {
                    instance.PreviewUrlGrey();
                }
            }

        }
    }
}
