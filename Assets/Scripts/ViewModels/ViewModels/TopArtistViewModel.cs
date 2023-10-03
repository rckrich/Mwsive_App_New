using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopArtistViewModel : ViewModel
{
    public GameObject artistPrefab;
    public Transform artistScrollContent;
    public int count;
    public ScrollRect scrollRect;
    public GameObject shimmer;

    private float end = -0.01f;

    public override void Initialize(params object[] list)
    {
#if PLATFORM_ANDROID
        SetAndroidBackAction();
#endif
        shimmer.SetActive(true);
        MwsiveConnectionManager.instance.GetRecommendedArtists(Callback_GetRecommendedArtists);
    }

    private void Callback_GetRecommendedArtists(object[] _list)
    {
        MwsiveRecommendedArtistsRoot mwsiveRecommendedArtistsRoot = (MwsiveRecommendedArtistsRoot)_list[1];
        shimmer.SetActive(false);
        string[] artists_ids = new string[mwsiveRecommendedArtistsRoot.artists.Count];

        for (int i = 0; i < mwsiveRecommendedArtistsRoot.artists.Count; i++)
        {

            artists_ids[i] = mwsiveRecommendedArtistsRoot.artists[i].spotify_id;
            count++;
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

    public void OnReachEnd()
    {
        if (scrollRect.verticalNormalizedPosition <= end)
        {
            MwsiveConnectionManager.instance.GetRecommendedArtists(Callback_GetRecommendedArtists, count, 20);
        }
    }

    public void OnClick_BackButton()
    {
        NewScreenManager.instance.ChangeToMainView(ViewID.ExploreViewModel);
        NewScreenManager.instance.GetCurrentView().SetAndroidBackAction();
    }

    public override void SetAndroidBackAction()
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
