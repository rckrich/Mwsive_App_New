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
    public GameObject shimmer;

    private int count;
    private const int MAXIMUM_VERTICAL_SCROLL_SPAWNS = 20;

    public override void Initialize(params object[] list)
    {
#if PLATFORM_ANDROID
        SetAndroidBackAction();
#endif
    }
    void Start()
    {
        shimmer.SetActive(true);
        MwsiveConnectionManager.instance.GetRecommendedPlaylists(Callback_GetRecommendedPlaylists);
    }

    private void Callback_GetRecommendedPlaylists(object[] _list)
    {
        MwsiveRecommendedPlaylistsRoot mwsiveRecommendedPlaylistsRoot = (MwsiveRecommendedPlaylistsRoot)_list[1];
        shimmer.SetActive(false); 
        int maxSpawnCounter = 0;

        for (int i = 0; i < mwsiveRecommendedPlaylistsRoot.playlists.Count; i++)
        {
            if (maxSpawnCounter < MAXIMUM_VERTICAL_SCROLL_SPAWNS)
            {
                GameObject playlistInstance = GameObject.Instantiate(playlistPrefab, playlistScrollContent);
                PlaylistAppObject playlist = playlistInstance.GetComponent<PlaylistAppObject>();
                playlist.Initialize(mwsiveRecommendedPlaylistsRoot.playlists[i], mwsiveRecommendedPlaylistsRoot.playlists[i].mwsive_tracks);
                maxSpawnCounter++;
                count++;
            }
        }
    }

    public void OnReachEnd()
    {

    }

    public void OnClick_BackButton()
    {
        NewScreenManager.instance.ChangeToMainView(ViewID.ExploreViewModel);
    }

    public void SetAndroidBackAction()
    {
#if PLATFORM_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            AppManager.instance.SetAndroidBackAction(() => {
                if (finishedLoading)
                {
                    OnClick_BackButton();
                }
            });
        }
#endif
    }
}
