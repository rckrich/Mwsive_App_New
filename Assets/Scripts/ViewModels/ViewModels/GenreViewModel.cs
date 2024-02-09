using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenreViewModel : ViewModel
{
    [Header("Instance Referecnes")]
    public GameObject trackHolderPrefab;
    public Transform instanceParent;
    public string artists;
    public TextMeshProUGUI nameTXT;
    public GameObject shimmer;

    private SeveralTrackRoot severalTrackRoot;
    private string[] severalID;
    private int trackCount = 0;
    private bool noMoreTracks = false;
    public ScrollRect scrollRect;
    public float end;
    int onlyone = 0;

    private List<Track> trackList = new List<Track>();
    bool NeedsReset = false;

    public override void Initialize(params object[] list)
    {
#if PLATFORM_ANDROID
        SetAndroidBackAction();
#endif
    }

    public void GetSeveralTracks(string[] _genreID, string _name)
    {
        severalID = _genreID; 
        shimmer.SetActive(true);
        if(severalID.Length >= 50)
        {
            
            MoreThan50IDs();
        }
        else
        {
            SpotifyConnectionManager.instance.GetSeveralTracks(severalID, Callback_GetSeveralTracks);  
        }
        nameTXT.text = _name;
    }

    public void Callback_GetSeveralTracks(object[] _value)
    {
        severalTrackRoot = (SeveralTrackRoot)_value[1];
        shimmer.SetActive(false);
        foreach (Track track in severalTrackRoot.tracks)
        {
            if (track != null)
            {
                TrackHolder instance = GameObject.Instantiate(trackHolderPrefab, instanceParent).GetComponent<TrackHolder>();
                trackList.Add(track);
                artists = "";
                foreach (Artist artist in track.artists) { artists += artist.name + ", "; }
                instance.Initialize(track.name, artists, track.id, track.artists[0].id, track.uri, track.preview_url, track.external_urls);
                if (track.album.images != null && track.album.images.Count > 0)
                    instance.SetImage(track.album.images[0].url);
                if (track.preview_url == null)
                {
                    instance.PreviewUrlGrey();
                }
                
            }

        }
        onlyone = 0;
    }
    public void Callback_GetSeveralTracksForSurf(object[] _value)
    {
        SeveralTrackRoot severalTrackRootTemporal = (SeveralTrackRoot)_value[1];

        foreach (Track track in severalTrackRootTemporal.tracks)
        {
            if (track != null)
            {
                trackList.Add(track);

            }
        }
        NeedsReset = true;
        if (trackList.Count < severalID.Length)
        {
            AskFor50Songs(true);
        }
        else
        {
            
            NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().DynamicPrefabSpawnerSeveralTracks(trackList);
        }

    }

    private void AskFor50Songs(bool flag = false)
    {
        List<string> genreID = new List<string>();
        for (int i = trackCount; i < trackCount + 50; i++)
        {
            if (i > severalID.Length - 1)
            {
                noMoreTracks = true;

                break;
            }

            genreID.Add(severalID[i]);

        }

        trackCount = trackCount + 50;

        if (flag == false)
        {

            SpotifyConnectionManager.instance.GetSeveralTracks(genreID.ToArray(), Callback_GetSeveralTracks);
        }
        else
        {

            SpotifyConnectionManager.instance.GetSeveralTracks(genreID.ToArray(), Callback_GetSeveralTracksForSurf);
        }

    }

    private void MoreThan50IDs()
    {
        if (NeedsReset)
        {
            noMoreTracks = false;
            trackCount = 50;
            trackList.RemoveRange(50, trackList.Count - 50);
            NeedsReset = false;
        }
        if (!noMoreTracks)
        {
            AskFor50Songs();
        }

    }

    public void OnReachEnd()
    {
        if(severalID != null)
        {
            if (onlyone == 0)
            {
                if (scrollRect.verticalNormalizedPosition <= end)
                {
                    onlyone = 1;
                    MoreThan50IDs();
                    
                }
            }
        }
       
    }
    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
        NewScreenManager.instance.GetCurrentView().SetAndroidBackAction();
    }

    public void OnClick_SurfButton()
    {
        

        if(trackList.Count < severalID.Length)
        {
            NewScreenManager.instance.ChangeToSpawnedView("surf");
            NewScreenManager.instance.GetCurrentView().GetComponent<PF_SurfViewModel>().Initialize();
            AskFor50Songs(true);
        }
        else
        {
            NewScreenManager.instance.ChangeToSpawnedView("surf");
            NewScreenManager.instance.GetCurrentView().GetComponent<PF_SurfViewModel>().Initialize();
            NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().DynamicPrefabSpawnerSeveralTracks(trackList);
        }


        
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
