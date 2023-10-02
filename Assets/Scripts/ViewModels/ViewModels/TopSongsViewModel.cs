using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopSongsViewModel : ViewModel
{
    public GameObject trackPrefab;
    public Transform trackScrollContent;
    public int count;
    public ScrollRect scrollRect;
    public GameObject shimmer;

    private float end = -0.01f;

    public override void Initialize(params object[] list)
    {
#if PLATFORM_ANDROID
        SetAndroidBackAction();
#endif
    }
    void Start()
    {
        shimmer.SetActive(true);
        MwsiveConnectionManager.instance.GetRecommendedTracks(Callback_GetRecommendedTracks);
    }

    private void Callback_GetRecommendedTracks(object[] _list)
    {
        MwsiveRecommendedTracksRoot mwsiveRecommendedTracksRoot = (MwsiveRecommendedTracksRoot)_list[1];

        string[] tracks_ids = new string[mwsiveRecommendedTracksRoot.tracks.Count];
        shimmer.SetActive(false);
        for (int i = 0; i < mwsiveRecommendedTracksRoot.tracks.Count; i++)
        {
            tracks_ids[i] = mwsiveRecommendedTracksRoot.tracks[i].mwsive_track.spotify_track_id;
            count++;
        }

        SpotifyConnectionManager.instance.GetSeveralTracks(tracks_ids, Callback_GetSeveralTracks);
    }

    private void Callback_GetSeveralTracks(object[] _list)
    {

        SeveralTrackRoot severalTrackRoot = (SeveralTrackRoot)_list[1];

        foreach (Track track in severalTrackRoot.tracks)
        {
            GameObject trackInstance = GameObject.Instantiate(trackPrefab, trackScrollContent);
            trackInstance.GetComponent<TrackAppObject>().Initialize(track);
        }

    }

    public void OnReachEnd()
    {
        if (scrollRect.verticalNormalizedPosition <= end)
        {
            MwsiveConnectionManager.instance.GetRecommendedTracks(Callback_GetRecommendedTracks, count, 20);
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
