using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopGenreViewModel : ViewModel
{
    public GameObject genrePrefab;
    public RectTransform genrePrefabRectTransform;
    public Transform genreScrollContent;
    public GameObject shimmer;
   

    private const int MAXIMUM_HORIZONTAL_SCROLL_SPAWNS = 20;

    public override void Initialize(params object[] list)
    {
#if PLATFORM_ANDROID
        SetAndroidBackAction();
#endif
    }
    void Start()
    {
        shimmer.SetActive(true);
        MwsiveConnectionManager.instance.GetGenres(Callback_GetGenres);
    }

    private void Callback_GetGenres(object[] _list)
    {
        MwsiveGenresRoot mwsiveGenresRoot = (MwsiveGenresRoot)_list[1];

        int maxSpawnCounter = 0;
        shimmer.SetActive(false);
        for (int i = 0; i < mwsiveGenresRoot.genres.Count; i++)
        {
            if (maxSpawnCounter < MAXIMUM_HORIZONTAL_SCROLL_SPAWNS)
            {
                
                GameObject playlistInstance = GameObject.Instantiate(genrePrefab, genreScrollContent);
                GenreAppObject genre = playlistInstance.GetComponent<GenreAppObject>();
                genre.Initialize(mwsiveGenresRoot.genres[i].genre, mwsiveGenresRoot.genres[i].mwsive_tracks);
                maxSpawnCounter++;
            }
        }
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
