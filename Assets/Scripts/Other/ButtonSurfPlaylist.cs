using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonSurfPlaylist : ViewModel
{
    
    public TMP_Text playlistText;
    public TMP_Text trackName;
    public TMP_Text artistName;
    public TMP_Text albumName;
    public Image trackCover;
    public string playlistName;
    public AppManager appManager;
    public Transform transformImage;
    public string trackID;
    public List<string> uris = new List<string>();

 

    public void SetSelectedPlaylistNameAppEvent(string _playlistName) { playlistText.text = _playlistName;
    }
    public string GetSelectedPlaylistNameAppEvent() { return playlistName; }
    private void OnEnable()
    {
        
        AddEventListener<SelectedPlaylistNameAppEvent>(SelectedPlaylistNameEventListener);
    }
    private void OnDisable()
    {
        RemoveEventListener<SelectedPlaylistNameAppEvent>(SelectedPlaylistNameEventListener);
    }
 
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
        GetTrack();
    }

    public void SetPlaylist(string _playlistName)
    {

        playlistName = _playlistName;

    }


    public void GetTrack()
    {
       SpotifyConnectionManager.instance.GetTrack(trackID, Callback_GetTrack);
    }

    private void Callback_GetTrack(object[] _value)
    {
        TrackRoot trackRoot = (TrackRoot)_value[1];
        trackName.text = trackRoot.name;
        foreach(Artist artist in trackRoot.artists)
        {
            artistName.text += trackRoot.artists + ", ";
        }
        albumName.text = trackRoot.album.name;
        uris.Add(trackRoot.uri);
        ImageManager.instance.GetImage(trackRoot.album.images[0].url, trackCover, (RectTransform)transformImage);

        MwsiveConnectionManager.instance.GetFollowingThatVoted(trackID, Callback_GetFollowingThatVoted);
    }

    private void Callback_GetFollowingThatVoted(object[] _value)
    {
       
    }

    public void OnSwipe()
    {
        SpotifyConnectionManager.instance.AddItemsToPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, uris, Callback_AddItemsToPlayList);

    }

    private void Callback_AddItemsToPlayList(object[] _value)
    {

    } 
}
