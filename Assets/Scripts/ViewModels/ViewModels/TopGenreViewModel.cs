using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopGenreViewModel : ViewModel
{
    public GameObject genrePrefab;
    public RectTransform genrePrefabRectTransform;
    public Transform genreScrollContent;
   

    private const int MAXIMUM_HORIZONTAL_SCROLL_SPAWNS = 20;

    void Start()
    {
        MwsiveConnectionManager.instance.GetGenres(Callback_GetGenres);
    }

    private void Callback_GetGenres(object[] _list)
    {
        MwsiveGenresRoot mwsiveGenresRoot = (MwsiveGenresRoot)_list[1];

        int maxSpawnCounter = 0;

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
}
