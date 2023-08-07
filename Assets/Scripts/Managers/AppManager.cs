using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : ViewModel
{
    public string profileID;
    public Image profilePicture;   
    public SurfViewModel surfViewModel;
    public GameObject Container;
    public Transform transform;
    public string playlistName;
    public SelectedPlaylistNameAppObject appObject;
    public ButtonSurfPlaylist buttonSurfPlaylist;
    void Start()
    {
        SpotifyConnectionManager.instance.GetCurrentUserProfile(Callback_GetUserProfile);
    }

    private void Callback_GetUserProfile(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        ProfileRoot profileRoot = (ProfileRoot)_value[1];
        profileID = profileRoot.id;
        Debug.Log(profileRoot.images[0].url);
        ImageManager.instance.GetImage(profileRoot.images[0].url, profilePicture, (RectTransform)transform);
        SpotifyConnectionManager.instance.GetPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, Callback_OnCLick_GetPlaylist);

    }
    private void Callback_OnCLick_GetPlaylist(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;
        SearchedPlaylist searchedPlaylist = (SearchedPlaylist)_value[1];
        playlistName = searchedPlaylist.name;

    }


}
