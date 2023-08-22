using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AppManager : Manager
{
    private const string TOP_GLOBAL_PLAYLIST_ID = "37i9dQZEVXbO3qyFxbkOE1";

    private static AppManager _instance;
    public static AppManager instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<AppManager>();

            return _instance;
        }
    }

    public string profileID;
    public Image profilePicture;   
    public GameObject Container;
    public Transform surfTransform;
    public string trackID;
    public string uri;
    public string url;
    public bool isSelected;
    public bool yours = true;
    public int countTopArtist = 1;
    public int countTopCurators = 1;
    
#if PLATFORM_ANDROID
    private System.Action androidBackAction;
#endif

    public SelectedPlaylistNameAppObject appObject;
    public ButtonSurfPlaylist buttonSurfPlaylist;

    private SearchedPlaylist currentPlaylist = null;
    private SpotifyWebCallback refreshPlaylistCallback;

    void Start()
    {
        StartSearch();
        if (ProgressManager.instance.progress.userDataPersistance.spotify_userTokenSetted)
        {
            // Normal Spotify Login flow
            SpotifyConnectionManager.instance.GetCurrentUserProfile(Callback_GetUserProfile_LogInFlow);
        }
        else
        {
            // No Spotify Login flow
            SpotifyConnectionManager.instance.GetPlaylist(TOP_GLOBAL_PLAYLIST_ID, Callback_GetTopPlaylist_NoLogInFLow);
        }
    }

#if PLATFORM_ANDROID
    public void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (androidBackAction != null) {
                    androidBackAction();
                }
            }
        }
    }

    public void SetAndroidBackAction(System.Action action)
    {
        this.androidBackAction = action;
    }

    public void ResetAndroidBackAction()
    {
        this.androidBackAction = null;
    }
#endif

    #region Playlist Settings

    public void ChangeCurrentPlaylist(SearchedPlaylist _searchedPlaylist)
    {
        currentPlaylist = _searchedPlaylist;
    }

    public void ChangeCurrentPlaylist(string _playlistID)
    {
        StartSearch();
        SpotifyConnectionManager.instance.GetPlaylist(_playlistID, Callback_OnSpotifyPlaylistChange);
    }

    public void Callback_OnSpotifyPlaylistChange(object[] _value)
    {
        SearchedPlaylist searchedPlaylist = (SearchedPlaylist)_value[1];
        currentPlaylist = searchedPlaylist;
        ProgressManager.instance.progress.userDataPersistance.current_playlist = searchedPlaylist.id;
        ProgressManager.instance.save();

        MwsiveConnectionManager.instance.PutLastSavedPlaylist(searchedPlaylist.id);

        InvokeEvent<SelectedPlaylistNameAppEvent>(new SelectedPlaylistNameAppEvent(searchedPlaylist.name));

        EndSearch();
    }

    public SearchedPlaylist GetCurrentPlaylist()
    {
        return currentPlaylist;
    }

    public bool SearchTrackOnCurrentPlaylist(string _id)
    {
        if(currentPlaylist != null)
        {
            Item searchedItem = currentPlaylist.tracks.items.Find((x) => x.track.id.Equals(_id));

            return searchedItem != null;
        }

        return false;
    }

    public void RefreshCurrentPlaylistInformation(SpotifyWebCallback _callback)
    {
        refreshPlaylistCallback = _callback;
        SpotifyConnectionManager.instance.GetPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, Callback_RefreshCurrentPlaylist);
    }

    private void Callback_RefreshCurrentPlaylist(object[] _value)
    {
        SearchedPlaylist searchedPlaylist = (SearchedPlaylist)_value[1];
        currentPlaylist = searchedPlaylist;

        if (refreshPlaylistCallback != null)
        {
            refreshPlaylistCallback(null);
        }

        refreshPlaylistCallback = null;
    }

    #endregion

    #region Log In FLow

    private void Callback_GetUserProfile_LogInFlow(object[] _value)
    {
        bool profileImageSetted = false;

        ProfileRoot profileRoot = (ProfileRoot)_value[1];
        profileID = profileRoot.id;
        if(profileRoot.images != null)
        {
            if (profileRoot.images.Count > 0) {

                foreach(SpotifyImage image in profileRoot.images)
                {
                    if(image.height == 300 && image.width == 300)
                    {
                        ImageManager.instance.GetImage(image.url, profilePicture, (RectTransform)surfTransform);
                        profileImageSetted = true;
                        break;
                    }
                }

                if(!profileImageSetted)
                    ImageManager.instance.GetImage(profileRoot.images[0].url, profilePicture, (RectTransform)surfTransform);
            }
        }

        SpotifyConnectionManager.instance.GetPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, Callback_GetCurrentMwsiveUserPlaylist_LogInFlow);
    }

    private void Callback_GetCurrentMwsiveUserPlaylist_LogInFlow(object[] _value)
    {
        if (WebCallsUtils.IsResponseItemNotFound((long)_value[0])){

            SpotifyConnectionManager.instance.CreatePlaylist(profileID, Callback_CreatePlaylist_LogInFlow);
            return;
        }

        SearchedPlaylist searchedPlaylist = (SearchedPlaylist)_value[1];
        currentPlaylist = searchedPlaylist;

        SpotifyConnectionManager.instance.GetCurrentUserTopTracks(Callback_GetUserTopTracks_LogInFlow);
    }

    private void Callback_CreatePlaylist_LogInFlow(object[] _value)
    {
        CreatedPlaylistRoot createdPlaylistRoot = (CreatedPlaylistRoot)_value[1];

        SearchedPlaylist newlySearchedPlaylistRoot = new SearchedPlaylist
        {
            collaborative = createdPlaylistRoot.collaborative,
            description = createdPlaylistRoot.description,
            external_urls = createdPlaylistRoot.external_urls,
            followers = createdPlaylistRoot.followers,
            href = createdPlaylistRoot.href,
            id = createdPlaylistRoot.id,
            name = createdPlaylistRoot.name,
            owner = createdPlaylistRoot.owner,
            @public = createdPlaylistRoot.@public,
            snapshot_id = createdPlaylistRoot.snapshot_id,
            tracks = createdPlaylistRoot.tracks,
            type = createdPlaylistRoot.type,
            uri = createdPlaylistRoot.uri
        };

        foreach(SpotifyImage image in createdPlaylistRoot.images)
        {
            newlySearchedPlaylistRoot.images.Add(image);
        }

        currentPlaylist = newlySearchedPlaylistRoot;

        SpotifyConnectionManager.instance.GetCurrentUserTopTracks(Callback_GetUserTopTracks_LogInFlow);
    }

    private void Callback_GetUserTopTracks_LogInFlow(object[] _value)
    {
        UserTopItemsRoot userTopItemsRoot = (UserTopItemsRoot)_value[1];

        if(userTopItemsRoot.total <= 5)
        {
            SpotifyConnectionManager.instance.GetPlaylist(TOP_GLOBAL_PLAYLIST_ID, Callback_GetGlobalTopTracks_LogInFlow);
            return;
        }

        string[] trackSeeds = new string[5];

        for(int i = 0; i < trackSeeds.Length; i++)
        {
            trackSeeds[i] = userTopItemsRoot.items[Random.Range(0, userTopItemsRoot.items.Count)].id;
        }

        SpotifyConnectionManager.instance.GetRecommendations(new string[] { }, trackSeeds, Callback_GetPersonalRecommendations_LogInFlow);
    }

    private void Callback_GetGlobalTopTracks_LogInFlow(object[] _value)
    {
        SearchedPlaylist searchedPlaylist = (SearchedPlaylist)_value[1];

        string[] trackSeeds = new string[5];

        for (int i = 0; i < trackSeeds.Length; i++)
        {
            if(searchedPlaylist.tracks.items[Random.Range(0, searchedPlaylist.tracks.items.Count)].track != null)
                trackSeeds[i] = searchedPlaylist.tracks.items[Random.Range(0, searchedPlaylist.tracks.items.Count)].track.id;
        }

        SpotifyConnectionManager.instance.GetRecommendations(new string[] { }, trackSeeds, Callback_GetTopRecommendations_LogInFlow);
    }

    private void Callback_GetPersonalRecommendations_LogInFlow(object[] _value)
    {
        EndSearch();
        RecommendationsRoot recommendationsRoot = (RecommendationsRoot)_value[1];
        SurfManager.instance.DynamicPrefabSpawnerSong(new object[] { recommendationsRoot });
    }

    private void Callback_GetTopRecommendations_LogInFlow(object[] _value)
    {
        EndSearch();
        RecommendationsRoot recommendationsRoot = (RecommendationsRoot)_value[1];
        SurfManager.instance.DynamicPrefabSpawnerSong(new object[] { recommendationsRoot });
    }

    #endregion

    #region No LogIn Flow

    private void Callback_GetTopPlaylist_NoLogInFLow(object[] _value)
    {
        EndSearch();
        SearchedPlaylist searchedPlaylist = (SearchedPlaylist)_value[1];
        SurfManager.instance.DynamicPrefabSpawnerPL(new object[] { searchedPlaylist });
    }

    #endregion

    public void GetTrack(string _trackId)
    {
        SpotifyConnectionManager.instance.GetTrack(_trackId, Callback_GetTrack);
    }

    public void Callback_GetTrack(object[] _value)
    {
        TrackRoot trackRoot = (TrackRoot)_value[1];
        url = trackRoot.external_urls.spotify;
        uri = trackRoot.uri;       
        NewScreenManager.instance.ChangeToSpawnedView("listaDeOpciones");
        EndSearch();
    }

}
