using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotifyGetSeveralTracksViewModel : MonoBehaviour
{
    public TrackDetailsPanel trackDetailsPanel;
    public GameObject severalTrackHolderPrefab;
    public Transform instanceParent;

    public string[] trackidsExample;

    public void OnClick_GetSeveralTracks()
    {
        SpotifyConnectionManager.instance.GetSeveralTracks(trackidsExample, Callback_OnCLick_GetSeveralTracks);
    }

    private void Callback_OnCLick_GetSeveralTracks(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        ResetScrollView();

        SeveralTrackRoot severalTrackRoot = (SeveralTrackRoot)_value[1];

        foreach (Track track in severalTrackRoot.tracks)
        {
            SpotifyConnectionDemoSeveralTrackHolder instance = GameObject.Instantiate(severalTrackHolderPrefab, instanceParent).GetComponent<SpotifyConnectionDemoSeveralTrackHolder>();
            instance.Initialize(track);
        }
    }

    private void ResetScrollView()
    {
        for (int i = 0; i < instanceParent.childCount; i++)
        {
            Destroy(instanceParent.GetChild(i).gameObject);
        }
    }
}
