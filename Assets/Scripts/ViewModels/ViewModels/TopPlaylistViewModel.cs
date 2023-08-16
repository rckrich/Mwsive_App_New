using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopPlaylistViewModel : ViewModel
{
    public GameObject playlistPrefab;
    public Transform playlistScrollContent;
    public ScrollRect scrollRect;
    public float end = -0.01f;

    private int count;
    private const int MAXIMUM_VERTICAL_SCROLL_SPAWNS = 20;

    void Start()
    {
        MwsiveConnectionManager.instance.GetRecommendedPlaylists(Callback_GetRecommendedPlaylists);
    }

    private void Callback_GetRecommendedPlaylists(object[] _list)
    {
        MwsiveRecommendedPlaylistsRoot mwsiveRecommendedPlaylistsRoot = (MwsiveRecommendedPlaylistsRoot)_list[1];

        int maxSpawnCounter = 0;

        for (int i = 0; i < mwsiveRecommendedPlaylistsRoot.playlists.Count; i++)
        {
            if (maxSpawnCounter < MAXIMUM_VERTICAL_SCROLL_SPAWNS)
            {
                GameObject playlistInstance = GameObject.Instantiate(playlistPrefab, playlistScrollContent);
                PlaylistAppObject playlist = playlistInstance.GetComponent<PlaylistAppObject>();
                playlist.Initialize(mwsiveRecommendedPlaylistsRoot.playlists[i].name, mwsiveRecommendedPlaylistsRoot.playlists[i].mwsive_tracks);
                maxSpawnCounter++;
                count++;
            }
        }
    }

    public void OnReachEnd()
    {

    }
}
