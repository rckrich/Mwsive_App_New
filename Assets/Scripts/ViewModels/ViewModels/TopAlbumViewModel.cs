using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopAlbumViewModel : ViewModel
{
    public GameObject albumPrefab;
    public Transform albumScrollContent;
    public int count;
    public ScrollRect scrollRect;

    private float end = -0.01f;

    void Start()
    {
        MwsiveConnectionManager.instance.GetRecommendedAlbums(Callback_GetRecommendedAlbums);
    }

    private void Callback_GetRecommendedAlbums(object[] _list)
    {
        MwsiveRecommendedAlbumsRoot mwsiveRecommendedAlbumsRoot = (MwsiveRecommendedAlbumsRoot)_list[1];

        string[] albums_ids = new string[mwsiveRecommendedAlbumsRoot.albums.Count];

        for (int i = 0; i < mwsiveRecommendedAlbumsRoot.albums.Count; i++)
        {
            albums_ids[i] = mwsiveRecommendedAlbumsRoot.albums[i].spotify_id;
            count++;
        }

        SpotifyConnectionManager.instance.GetSeveralAlbums(albums_ids, Callback_GetSeveralAlbums);
    }

    private void Callback_GetSeveralAlbums(object[] _list)
    {

        SeveralAlbumRoot severalAlbumRoot = (SeveralAlbumRoot)_list[1];

        foreach (Album album in severalAlbumRoot.albums)
        {
            GameObject trackInstance = GameObject.Instantiate(albumPrefab, albumScrollContent);
            trackInstance.GetComponent<AlbumAppObject>().Initialize(album);
        }

    }

    public void GetMorePlaylist()
    {
        if (scrollRect.verticalNormalizedPosition <= end)
        {
            MwsiveConnectionManager.instance.GetRecommendedAlbums(Callback_GetRecommendedAlbums, count, 20);
        }
           
    }

    public void OnClick_BackButton()
    {
        NewScreenManager.instance.ChangeToMainView(ViewID.ExploreViewModel);
    }
}
