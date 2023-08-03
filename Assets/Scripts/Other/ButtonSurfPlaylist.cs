using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;

public class ButtonSurfPlaylist : ViewModel
{
    public HolderManager holderManager;
    public TMP_Text playlistText;
    public string playlistName;
    public void SetSelectedPlaylistNameAppEvent(string _playlistName) { playlistText.text = _playlistName; }
    public string GetSelectedPlaylistNameAppEvent() { return playlistName; }
    private void OnEnable()
    {
        AddEventListener<SelectedPlaylistNameAppEvent>(SelectedPlaylistNameEventListener);
    }
    private void OnDisable()
    {
        RemoveEventListener<SelectedPlaylistNameAppEvent>(SelectedPlaylistNameEventListener);
    }
    // Start is called before the first frame update
    void Start()
    {

        GetPlaylist();
    }

    public void SelectedPlaylistNameEventListener(SelectedPlaylistNameAppEvent _event)
    {
        GetPlaylist();
        
    }
    public void OnClickPlaylistButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("surfMiPlaylist");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }

    public void GetPlaylist()
    {
        SpotifyConnectionManager.instance.GetPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, Callback_OnCLick_GetPlaylist);
    }
    private void Callback_OnCLick_GetPlaylist(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;
        SearchedPlaylist searchedPlaylist = (SearchedPlaylist)_value[1];
        playlistName = searchedPlaylist.name;
        SetSelectedPlaylistNameAppEvent(searchedPlaylist.name);
        playlistText.text = playlistName;
    }
}
