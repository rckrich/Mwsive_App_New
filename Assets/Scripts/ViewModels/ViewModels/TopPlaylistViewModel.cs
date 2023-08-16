using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopPlaylistViewModel : ViewModel
{
    public GameObject playlistPrefab;
    public Transform playlistScrollContent;

    private const int MAXIMUM_HORIZONTAL_SCROLL_SPAWNS = 20;

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
            if (maxSpawnCounter < MAXIMUM_HORIZONTAL_SCROLL_SPAWNS)
            {
                GameObject playlistInstance = GameObject.Instantiate(playlistPrefab, playlistScrollContent);
                PlaylistAppObject playlist = playlistInstance.GetComponent<PlaylistAppObject>();
                playlist.Initialize(mwsiveRecommendedPlaylistsRoot.playlists[i].name, mwsiveRecommendedPlaylistsRoot.playlists[i].mwsive_tracks);
                maxSpawnCounter++;
            }
        }
    }
}
