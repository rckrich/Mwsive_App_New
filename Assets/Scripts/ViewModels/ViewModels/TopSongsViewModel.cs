using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopSongsViewModel : ViewModel
{
    public GameObject trackPrefab;
    public Transform trackScrollContent;
    public int count;

    void Start()
    {
        MwsiveConnectionManager.instance.GetRecommendedTracks(Callback_GetRecommendedTracks);
    }

    private void Callback_GetRecommendedTracks(object[] _list)
    {
        MwsiveRecommendedTracksRoot mwsiveRecommendedTracksRoot = (MwsiveRecommendedTracksRoot)_list[1];

        string[] tracks_ids = new string[mwsiveRecommendedTracksRoot.tracks.Count];

        for (int i = 0; i < mwsiveRecommendedTracksRoot.tracks.Count; i++)
        {
            tracks_ids[i] = mwsiveRecommendedTracksRoot.tracks[i].mwsive_track.spotify_track_id;
        }

        SpotifyConnectionManager.instance.GetSeveralTracks(tracks_ids, Callback_GetSeveralTracks);
    }

    private void Callback_GetSeveralTracks(object[] _list)
    {

        SeveralTrackRoot severalTrackRoot = (SeveralTrackRoot)_list[1];

        foreach (Track track in severalTrackRoot.tracks)
        {
            GameObject trackInstance = GameObject.Instantiate(trackPrefab, trackScrollContent);
            trackInstance.GetComponent<TrackAppObject>().Initialize(track);
        }

    }

   
}
