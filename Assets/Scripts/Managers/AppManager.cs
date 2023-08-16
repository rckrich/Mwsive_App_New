using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int countTopArtist = 1;
    public int countTopCurators = 1;

    public SelectedPlaylistNameAppObject appObject;
    public ButtonSurfPlaylist buttonSurfPlaylist;

    private SearchedPlaylist currentPlaylist = null;
    private SpotifyWebCallback refreshPlaylistCallback;

    void Start()
    {
        StartSearch();
        if (ProgressManager.instance.progress.userDataPersistance.spotify_userTokenSetted)
        {
            SpotifyConnectionManager.instance.GetCurrentUserProfile(Callback_GetUserProfile);
        }
        else
        {
            SpotifyConnectionManager.instance.GetPlaylist(TOP_GLOBAL_PLAYLIST_ID, Callback_GetTopPlaylist);
        }
    }

    public void ChangeCurrentPlaylist(SearchedPlaylist _searchedPlaylist)
    {
        currentPlaylist = _searchedPlaylist;
    }

    public void ChangeCurrentPlaylist(string _playlistID)
    {
        StartSearch();
        SpotifyConnectionManager.instance.GetPlaylist(_playlistID, Callback_OnPlaylistChange);
    }

    public void Callback_OnPlaylistChange(object[] _value)
    {
        SearchedPlaylist searchedPlaylist = (SearchedPlaylist)_value[1];
        currentPlaylist = searchedPlaylist;
        ProgressManager.instance.progress.userDataPersistance.current_playlist = searchedPlaylist.id;
        ProgressManager.instance.save();

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

    private void Callback_GetUserProfile(object[] _value)
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

        SpotifyConnectionManager.instance.GetPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, Callback_GetCurrentMwsiveUserPlaylist);
    }

    private void Callback_GetCurrentMwsiveUserPlaylist(object[] _value)
    {
        SearchedPlaylist searchedPlaylist = (SearchedPlaylist)_value[1];
        currentPlaylist = searchedPlaylist;
        SpotifyConnectionManager.instance.GetPlaylist(TOP_GLOBAL_PLAYLIST_ID, Callback_GetTopPlaylist);
    }

    private void Callback_GetTopPlaylist(object[] _value)
    {
        EndSearch();
        SearchedPlaylist searchedPlaylist = (SearchedPlaylist)_value[1];
        SurfManager.instance.DynamicPrefabSpawnerPL(new object[] { searchedPlaylist });
    }

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

    public void RefreshCurrentPlaylistInformation(SpotifyWebCallback _callback)
    {
        refreshPlaylistCallback = _callback;
        SpotifyConnectionManager.instance.GetPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, Callback_RefreshCurrentPlaylist);
    }

    private void Callback_RefreshCurrentPlaylist(object[] _value)
    {
        SearchedPlaylist searchedPlaylist = (SearchedPlaylist)_value[1];
        currentPlaylist = searchedPlaylist;

        if(refreshPlaylistCallback != null)
        {
            refreshPlaylistCallback(null);
        }

        refreshPlaylistCallback = null;
    }


}
