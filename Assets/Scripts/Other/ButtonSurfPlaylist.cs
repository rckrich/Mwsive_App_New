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
    public string previewURL;
    public string mp3URL;

    public void SetSelectedPlaylistNameAppEvent(string _playlistName)
    {
        playlistName = _playlistName;
    }
    public string GetSelectedPlaylistNameAppEvent() { return playlistName; }
    private void OnEnable()
    {
        playlistText.text = playlistName;
        AddEventListener<SelectedPlaylistNameAppEvent>(SelectedPlaylistNameEventListener);
    }
    private void OnDisable()
    {
        RemoveEventListener<SelectedPlaylistNameAppEvent>(SelectedPlaylistNameEventListener);
    }

    public void InitializeMwsiveSong(string _playlistName, string _trackname, string _album, string _artist, string _image, string _spotifyid, string _url, string _previewURL)
    {
        playlistText.text = _playlistName;
        playlistName = _playlistName;
        if (_trackname.Length > 27)
        {
            string _text2 = "";
            for (int i = 0; i < 27; i++)
            {
                _text2 = _text2 + _trackname[i];
            }
            _text2 = _text2 + "...";
            trackName.text = _text2;
        }
        else
        {
            trackName.text = _trackname;
        }
        albumName.text = _album;
        artistName.text = _artist;
        ImageManager.instance.GetImage(_image, trackCover, (RectTransform)this.transform);
        trackID = _spotifyid;
        uris[0] = _url;
        previewURL = _previewURL;
    }

    public void PlayAudioPreview()
    {
        SpotifyPreviewAudioManager.instance.GetTrack(previewURL);

    }

    public void OnClic_StopAudioPreview()
    {
        SpotifyPreviewAudioManager.instance.Pause();
    }
  
    public void OnClick_OpenPlaylist()
    {
        NewScreenManager.instance.ChangeToSpawnedView("surfMiPlaylist");

    }

    public void Swipe()
    {
        SpotifyConnectionManager.instance.AddItemsToPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, uris, Callback_Swipe);
    }

    public void Callback_Swipe(object[] _value)
    {

    }

    public void BackSwipe()
    {
        SpotifyConnectionManager.instance.RemoveItemsFromPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, uris, Callback_Swipe);
    }

    public void CallBack_BackSwipe()
    {

    }

    public void SelectedPlaylistNameEventListener(SelectedPlaylistNameAppEvent _event)
    {
        OnPlaylistChange();
        
    }

    public void OnPlaylistChange()
    {
        SpotifyConnectionManager.instance.GetPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, Callback_OnPlaylistChange);
    }

    public void Callback_OnPlaylistChange(object[] _value)
    {
        SearchedPlaylist searchedPlaylist = (SearchedPlaylist)_value[1];
        playlistName = searchedPlaylist.name;
        playlistText.text = searchedPlaylist.name;
    }
}