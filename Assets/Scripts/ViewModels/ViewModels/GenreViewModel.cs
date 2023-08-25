using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenreViewModel : ViewModel
{
    [Header("Instance Referecnes")]
    public GameObject trackHolderPrefab, surfButton;
    public Transform instanceParent;
    public string artists;
    public TextMeshProUGUI name;

    private SeveralTrackRoot severalTrackRoot;

    void Start()
    {
        
    }

    public void GetSeveralTracks(string[] _genreID, string _name)
    {
        SpotifyConnectionManager.instance.GetSeveralTracks(_genreID, Callback_GetSeveralTracks);
        name.text = _name;
    }

    public void Callback_GetSeveralTracks(object[] _value)
    {
        severalTrackRoot = (SeveralTrackRoot)_value[1];

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


    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
    }

    public void OnClick_SurfButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("surf");
        NewScreenManager.instance.GetCurrentView().GetComponentInChildren<SurfManager>().DynamicPrefabSpawnerSeveralTracks(severalTrackRoot.tracks);
    }
}
