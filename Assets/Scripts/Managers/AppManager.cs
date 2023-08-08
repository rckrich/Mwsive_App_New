using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : Manager
{
    private const string TOP_GLOBAL_PLAYLIST_ID = "37i9dQZEVXbMDoHDwVN2tF";

    public string profileID;
    public Image profilePicture;   
    public SurfViewModel surfViewModel;
    public GameObject Container;
    public Transform surfTransform;
    public string playlistName;
    public SelectedPlaylistNameAppObject appObject;
    public ButtonSurfPlaylist buttonSurfPlaylist;

    void Start()
    {
        SpotifyConnectionManager.instance.GetCurrentUserProfile(Callback_GetUserProfile);
    }

    private void Callback_GetUserProfile(object[] _value)
    {
        ProfileRoot profileRoot = (ProfileRoot)_value[1];
        profileID = profileRoot.id;
        Debug.Log(profileRoot.images[0].url);
        ImageManager.instance.GetImage(profileRoot.images[0].url, profilePicture, (RectTransform)surfTransform);
        SpotifyConnectionManager.instance.GetPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, Callback_GetCurrentMwsiveUserPlaylist);
    }

    private void Callback_GetCurrentMwsiveUserPlaylist(object[] _value)
    {
        SearchedPlaylist searchedPlaylist = (SearchedPlaylist)_value[1];
        playlistName = searchedPlaylist.name;

        SpotifyConnectionManager.instance.GetPlaylist(TOP_GLOBAL_PLAYLIST_ID, Callback_GetTopPlaylist);
    }

    private void Callback_GetTopPlaylist(object[] _value)
    {
        //TODO llenar el surf de la info de esta playlist
        SearchedPlaylist searchedPlaylist = (SearchedPlaylist)_value[1];
        SurfManager.instance.DynamicPrefabSpawner(new object[] { searchedPlaylist });
        //SurfManager.instance.SetTracks(searchedPlaylist.tracks);
        //Start Surf ?????

    }
}
