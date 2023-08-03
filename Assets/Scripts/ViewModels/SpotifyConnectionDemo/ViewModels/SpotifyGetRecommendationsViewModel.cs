using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotifyGetRecommendationsViewModel : MonoBehaviour
{
    public string[] seed_artists;
    public string[] seed_genres;
    public string[] seed_tracks;

    [Header("Instance References")]
    public GameObject trackHolderPrefab;
    public Transform instanceParent;
    public int objectsToNotDestroyIndex;

    public void OnClick_GetRecommendations()
    {
        if ((seed_artists.Length + seed_genres.Length + seed_tracks.Length) > 5) {
            Debug.Log("The seeds should be no more than 5 from either artists, genres or tracks");
            return;
        }

        SpotifyConnectionManager.instance.GetRecommendations(seed_artists, seed_genres, /*seed_tracks,*/ Callback_OnCLick_GetRecommendations);
    }

    private void Callback_OnCLick_GetRecommendations(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        RecommendationsRoot recommendationsRoot = (RecommendationsRoot)_value[1];

        InstanceTrackObjects(recommendationsRoot.tracks);
    }

    private void InstanceTrackObjects(List<Track> _tracks)
    {
        ResetScrollView();

        foreach (Track track in _tracks)
        {
            SpotifyConnectionDemoTrackHolder instance = GameObject.Instantiate(trackHolderPrefab, instanceParent).GetComponent<SpotifyConnectionDemoTrackHolder>();
            instance.Initialize(track.name, track.artists[0].name, track.id);

            if (track.album.images != null && track.album.images.Count > 0)
                instance.SetImage(track.album.images[0].url);
        }
    }

    private void ResetScrollView()
    {
        for (int i = (objectsToNotDestroyIndex); i < instanceParent.childCount; i++)
        {
            Destroy(instanceParent.GetChild(i).gameObject);
        }
    }
}
