using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using VoxelBusters.CoreLibrary;
using UnityEngine.SceneManagement;

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
    public MwsiveUser currentMwsiveUser;
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

    private Sprite _profileSprite;
    public Sprite profileSprite
    {
        get { return _profileSprite; }
    }

    private LogInCallback previousAction;

#if PLATFORM_ANDROID
    private System.Action androidBackAction;
#endif

    public SelectedPlaylistNameAppObject appObject;
    public ButtonSurfPlaylist buttonSurfPlaylist;
    public GameObject loadingCard;

    private SpotifyPlaylistRoot topSongsPlaylistSavedList = null;
    private SpotifyPlaylistRoot currentPlaylist = null;
    private SpotifyWebCallback refreshPlaylistCallback;
    private bool _isLogInMode = true;
    public bool isLogInMode { get { return _isLogInMode; } }

    void Start()
    {
        StartAppProcess();
    }

    public void StartAppProcess()
    {
        loadingCard.SetActive(true);
        if (ProgressManager.instance.progress.userDataPersistance.spotify_userTokenSetted)
        {
            // Normal Spotify Login flow
            SetLogInMode(true);
            SpotifyConnectionManager.instance.GetCurrentUserProfile(Callback_GetUserProfile_LogInFlow);
        }
        else
        {
            // No Spotify Login flow
            SetLogInMode(false);
            SpotifyConnectionManager.instance.GetPlaylist(TOP_GLOBAL_PLAYLIST_ID, Callback_GetTopPlaylist_NoLogInFLow);
        }
    }

    public void StartAppProcessFromOutside(LogInCallback _callback = null)
    {
        loadingCard.SetActive(true);
        if (ProgressManager.instance.progress.userDataPersistance.spotify_userTokenSetted)
        {
            // Normal Spotify Login flow
            SetLogInMode(true);
            previousAction = _callback;
            SpotifyConnectionManager.instance.GetCurrentUserProfile(Callback_GetUserProfile_OutsideLogInFlow);
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
        if (androidBackAction != null) androidBackAction = null;

        this.androidBackAction = action;
    }

    public void ResetAndroidBackAction()
    {
        this.androidBackAction = null;
    }
#endif

    #region Playlist Settings

    public void ChangeCurrentPlaylist(SpotifyPlaylistRoot _searchedPlaylist)
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
        SpotifyPlaylistRoot searchedPlaylist = (SpotifyPlaylistRoot)_value[1];
        currentPlaylist = searchedPlaylist;
        ProgressManager.instance.progress.userDataPersistance.current_playlist = searchedPlaylist.id;
        ProgressManager.instance.save();

        MwsiveConnectionManager.instance.PutLastSavedPlaylist(searchedPlaylist.id);

        InvokeEvent<SelectedPlaylistNameAppEvent>(new SelectedPlaylistNameAppEvent(searchedPlaylist.name));

        EndSearch();
    }

    public SpotifyPlaylistRoot GetCurrentPlaylist()
    {
        return currentPlaylist;
    }

    public bool SearchTrackOnCurrentPlaylist(string _id)
    {
        if (currentPlaylist != null)
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
        SpotifyPlaylistRoot searchedPlaylist = (SpotifyPlaylistRoot)_value[1];
        currentPlaylist = searchedPlaylist;

        if (refreshPlaylistCallback != null)
        {
            refreshPlaylistCallback(null);
        }

        refreshPlaylistCallback = null;

        MwsiveConnectionManager.instance.PutLastSavedPlaylist(searchedPlaylist.id);
    }

    #endregion

    #region Log In FLow

    private void Callback_GetUserProfile_LogInFlow(object[] _value)
    {
        try
        {
            bool profileImageSetted = false;

            ProfileRoot profileRoot = (ProfileRoot)_value[1];
            profileID = profileRoot.id;
            MwsiveConnectionManager.instance.GetCurrentMwsiveUser(Callback_GetCurrentMwsiveUser);
        }
        catch (System.NullReferenceException)
        {
            SceneManager.LoadScene("MainScene_Ricardo");
            UIMessage.instance.UIMessageInstanciate("Ocurri? Un error, vuelve a iniciar sesi?n para continuar");
        }
        
    }

    private void Callback_GetCurrentMwsiveUser(object[] _value)
    {
        MwsiveUserRoot mwsiveUserRoot = (MwsiveUserRoot)_value[1];

        currentMwsiveUser = mwsiveUserRoot.user;

        /*if (mwsiveUserRoot.user.image_url != null)
            ImageManager.instance.GetImage(mwsiveUserRoot.user.image_url, profilePicture, (RectTransform)surfTransform);*/

        InvokeEvent<ChangeDiskAppEvent>(new ChangeDiskAppEvent(mwsiveUserRoot.user.total_disks));

        SpotifyConnectionManager.instance.GetPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, Callback_GetCurrentMwsiveUserPlaylist_LogInFlow);
    }

    private void Callback_GetCurrentMwsiveUserPlaylist_LogInFlow(object[] _value)
    {
        if (WebCallsUtils.IsResponseItemNotFound((long)_value[0])) {

            SpotifyConnectionManager.instance.CreatePlaylist(profileID, Callback_CreatePlaylist_LogInFlow);
            return;
        }

        SpotifyPlaylistRoot searchedPlaylist = (SpotifyPlaylistRoot)_value[1];
        currentPlaylist = searchedPlaylist;
        MwsiveConnectionManager.instance.PutLastSavedPlaylist(searchedPlaylist.id);

        SpotifyConnectionManager.instance.GetCurrentUserTopTracks(Callback_GetUserTopTracks_LogInFlow);
    }

    private void Callback_CreatePlaylist_LogInFlow(object[] _value)
    {
        SpotifyPlaylistRoot spotifyPlaylistRoot = (SpotifyPlaylistRoot)_value[1];

        currentPlaylist = spotifyPlaylistRoot;
        MwsiveConnectionManager.instance.PutLastSavedPlaylist(spotifyPlaylistRoot.id);

        SpotifyConnectionManager.instance.GetCurrentUserTopTracks(Callback_GetUserTopTracks_LogInFlow);
    }

    private void Callback_GetUserTopTracks_LogInFlow(object[] _value)
    {
        UserTopItemsRoot userTopItemsRoot = (UserTopItemsRoot)_value[1];

        if (userTopItemsRoot.total <= 5)
        {
            SpotifyConnectionManager.instance.GetPlaylist(TOP_GLOBAL_PLAYLIST_ID, Callback_GetGlobalTopTracks_LogInFlow);
            return;
        }

        string[] trackSeeds = new string[5];

        for (int i = 0; i < trackSeeds.Length; i++)
        {
            trackSeeds[i] = userTopItemsRoot.items[Random.Range(0, userTopItemsRoot.items.Count)].id;
        }
        if (SurfController.instance.ReturnMain().GetComponent<SurfManager>().IsManagerEmpty())
        {
            SpotifyConnectionManager.instance.GetRecommendations(new string[] { }, trackSeeds, Callback_GetPersonalRecommendations_LogInFlow);
        }
        
    }

    private void Callback_GetGlobalTopTracks_LogInFlow(object[] _value)
    {
        topSongsPlaylistSavedList = null;

        SpotifyPlaylistRoot searchedPlaylist = (SpotifyPlaylistRoot)_value[1];

        topSongsPlaylistSavedList = searchedPlaylist;

        string[] trackSeeds = new string[5];

        for (int i = 0; i < trackSeeds.Length; i++)
        {
            if (searchedPlaylist.tracks.items[Random.Range(0, searchedPlaylist.tracks.items.Count)].track != null)
                trackSeeds[i] = searchedPlaylist.tracks.items[Random.Range(0, searchedPlaylist.tracks.items.Count)].track.id;
        }

        SpotifyConnectionManager.instance.GetRecommendations(new string[] { }, trackSeeds, Callback_GetTopRecommendations_LogInFlow);
    }

    private void Callback_GetPersonalRecommendations_LogInFlow(object[] _value)
    {

        if (((long)_value[0]).Equals(WebCallsUtils.GATEWAY_TIMEOUT)) {
            SpotifyConnectionManager.instance.GetPlaylist(TOP_GLOBAL_PLAYLIST_ID, Callback_GetGlobalTopTracks_LogInFlow);
            return;
        }

        loadingCard.SetActive(false);
        RecommendationsRoot recommendationsRoot = (RecommendationsRoot)_value[1];
        SurfManager.instance.DynamicPrefabSpawnerSong(new object[] { recommendationsRoot });
    }

    private void Callback_GetTopRecommendations_LogInFlow(object[] _value)
    {
        if (((long)_value[0]).Equals(WebCallsUtils.GATEWAY_TIMEOUT))
        {
            loadingCard.SetActive(false);
            if (topSongsPlaylistSavedList != null)
            {
                SurfManager.instance.DynamicPrefabSpawnerPL(new object[] { topSongsPlaylistSavedList });
            }
            else {
                //TODO: Call pop up of error 503
            }
            return;
        }

        loadingCard.SetActive(false);
        RecommendationsRoot recommendationsRoot = (RecommendationsRoot)_value[1];
        SurfManager.instance.DynamicPrefabSpawnerSong(new object[] { recommendationsRoot });
    }

    #endregion

    #region No LogIn Flow

    private void Callback_GetTopPlaylist_NoLogInFLow(object[] _value)
    {
        topSongsPlaylistSavedList = null;

        SpotifyPlaylistRoot searchedPlaylist = (SpotifyPlaylistRoot)_value[1];

        topSongsPlaylistSavedList = searchedPlaylist;

        string[] trackSeeds = new string[5];

        for (int i = 0; i < trackSeeds.Length; i++)
        {
            if (searchedPlaylist.tracks.items[Random.Range(0, searchedPlaylist.tracks.items.Count)].track != null)
                trackSeeds[i] = searchedPlaylist.tracks.items[Random.Range(0, searchedPlaylist.tracks.items.Count)].track.id;
        }

        SpotifyConnectionManager.instance.GetRecommendations(new string[] { }, trackSeeds, Callback_GetTopRecommendations_NoLogInFlow);
    }

    private void Callback_GetTopRecommendations_NoLogInFlow(object[] _value)
    {

        if (((long)_value[0]).Equals(WebCallsUtils.GATEWAY_TIMEOUT))
        {
            loadingCard.SetActive(false);
            if (topSongsPlaylistSavedList != null)
            {
                SurfManager.instance.DynamicPrefabSpawnerPL(new object[] { topSongsPlaylistSavedList });
            }
            else
            {
                //TODO: Call pop up of error 503
            }
            return;
        }

        loadingCard.SetActive(false);
        RecommendationsRoot recommendationsRoot = (RecommendationsRoot)_value[1];
        SurfManager.instance.DynamicPrefabSpawnerSong(new object[] { recommendationsRoot });
    }

    #endregion

    #region Outside LogIn Flow
    private void Callback_GetUserProfile_OutsideLogInFlow(object[] _value)
    {
        bool profileImageSetted = false;

        ProfileRoot profileRoot = (ProfileRoot)_value[1];
        profileID = profileRoot.id;

        MwsiveConnectionManager.instance.GetCurrentMwsiveUser(Callback_GetCurrentMwsiveUser_OutsideLogInFlow);
    }

    private void Callback_GetCurrentMwsiveUser_OutsideLogInFlow(object[] _value)
    {
        MwsiveUserRoot mwsiveUserRoot = (MwsiveUserRoot)_value[1];

        currentMwsiveUser = mwsiveUserRoot.user;

        SpotifyConnectionManager.instance.GetPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, Callback_GetCurrentMwsiveUserPlaylist_OutsideLogInFlow);
    }

    private void Callback_GetCurrentMwsiveUserPlaylist_OutsideLogInFlow(object[] _value)
    {
        if (WebCallsUtils.IsResponseItemNotFound((long)_value[0]))
        {
            SpotifyConnectionManager.instance.CreatePlaylist(profileID, Callback_CreatePlaylist_OutsideLogInFlow);
            return;
        }

        SpotifyPlaylistRoot searchedPlaylist = (SpotifyPlaylistRoot)_value[1];
        currentPlaylist = searchedPlaylist;
        MwsiveConnectionManager.instance.PutLastSavedPlaylist(searchedPlaylist.id);

        InvokeEvent<SelectedPlaylistNameAppEvent>(new SelectedPlaylistNameAppEvent(currentPlaylist.name));

        StartPreviousAction();

        loadingCard.SetActive(false);
    }

    private void Callback_CreatePlaylist_OutsideLogInFlow(object[] _value)
    {
        SpotifyPlaylistRoot spotifyPlaylistRoot = (SpotifyPlaylistRoot)_value[1];

        currentPlaylist = spotifyPlaylistRoot;
        MwsiveConnectionManager.instance.PutLastSavedPlaylist(spotifyPlaylistRoot.id);

        InvokeEvent<SelectedPlaylistNameAppEvent>(new SelectedPlaylistNameAppEvent(currentPlaylist.name));

        StartPreviousAction();

        loadingCard.SetActive(false);
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
        loadingCard.SetActive(false);
    }

    public void SetLogInMode(bool _value)
    {
        _isLogInMode = _value;
    }

    #region Refresh Mwsive User

    private MwsiveWebCallback refreshMwsiveUserCallback;

    public void RefreshUser(MwsiveWebCallback _callback = null){
        if(_callback != null)
        {
            refreshMwsiveUserCallback = Callback_RefreshUser;
            refreshMwsiveUserCallback += _callback;
            MwsiveConnectionManager.instance.GetCurrentMwsiveUser(refreshMwsiveUserCallback);
        }
        else
        {
            MwsiveConnectionManager.instance.GetCurrentMwsiveUser(Callback_RefreshUser);
        }
    }

    private void Callback_RefreshUser(object[] _value){
        currentMwsiveUser = ((MwsiveUserRoot)_value[1]).user;
        InvokeEvent<ChangeDiskAppEvent>(new ChangeDiskAppEvent(currentMwsiveUser.total_disks));

        if (currentMwsiveUser.image_url != null)
            ImageManager.instance.GetImage(currentMwsiveUser.image_url, null, (RectTransform)surfTransform, "PROFILEIMAGE", Callback_ProfileSprite);
    }

    private void Callback_ProfileSprite(object[] _value)
    {
        _profileSprite = (Sprite)_value[0];
        InvokeEvent<ChangeProfileSpriteAppEvent>(new ChangeProfileSpriteAppEvent() { newSprite = _profileSprite });
    }

    #endregion

    private void StartPreviousAction()
    {
        if (previousAction != null)
        {
            previousAction(null);
            previousAction = null;
        }
    }

}
