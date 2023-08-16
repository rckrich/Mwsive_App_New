using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopArtistViewModel : ViewModel
{
    public GameObject artistPrefab;
    public Transform artistScrollContent;
    public int count = 1;
    void Start()
    {
        MwsiveConnectionManager.instance.GetRecommendedArtists(Callback_GetRecommendedArtists);
    }

    private void Callback_GetRecommendedArtists(object[] _list)
    {
        MwsiveRecommendedArtistsRoot mwsiveRecommendedArtistsRoot = (MwsiveRecommendedArtistsRoot)_list[1];

        string[] artists_ids = new string[mwsiveRecommendedArtistsRoot.artists.Count];

        for (int i = 0; i < mwsiveRecommendedArtistsRoot.artists.Count; i++)
        {

            artists_ids[i] = mwsiveRecommendedArtistsRoot.artists[i].spotify_id;
        }
        SpotifyConnectionManager.instance.GetSeveralArtists(artists_ids, Callback_GetSeveralArtists);
    }

    private void Callback_GetSeveralArtists(object[] _list)
    {
        SeveralArtistRoot severalArtistRoot = (SeveralArtistRoot)_list[1];

        foreach (Artist artist in severalArtistRoot.artists)
        {
            GameObject artistInstance = GameObject.Instantiate(artistPrefab, artistScrollContent);
            TopArtistAppObject artists = artistInstance.GetComponent<TopArtistAppObject>();
            artists.Initialize(artist);
        }
    }

    public void OnClick_BackButton()
    {
        NewScreenManager.instance.ChangeToMainView(ViewID.ExploreViewModel);
    }
}
