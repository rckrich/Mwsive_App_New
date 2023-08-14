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
    public bool isAdd = false;
    public string externalURL;

    public MwsiveButton mwsiveButton;
    public GameObject loadingAnimGameObject;

    public void SetSelectedPlaylistNameAppEvent(string _playlistName)
    {
        playlistName = _playlistName;
    }
    public string GetSelectedPlaylistNameAppEvent() { return playlistName; }

    private void OnEnable()
    {
        playlistText.text = playlistName;
        AddEventListener<SelectedPlaylistNameAppEvent>(SelectedPlaylistNameEventListener);
        playlistText.text = AppManager.instance.GetCurrentPlaylist().name;
    }

    private void OnDisable()
    {
        RemoveEventListener<SelectedPlaylistNameAppEvent>(SelectedPlaylistNameEventListener);
    }

    public void InitializeMwsiveSong(string _playlistName, string _trackname, string _album, string _artist, string _image, string _spotifyid, string _url, string _previewURL, string _externalURL)
    {
        if(_playlistName != null){
            playlistText.text = _playlistName;
        }
        if(_trackname != null){
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
        }
        
        if(_album != null){
            albumName.text = _album;
        }
        if(_artist != null){
            artistName.text = _artist;
        }
        
        if(_image != null){
            ImageManager.instance.GetImage(_image, trackCover, (RectTransform)this.transform);
        }
        
        if(_spotifyid != null){
            trackID = _spotifyid;
            if (AppManager.instance.SearchTrackOnCurrentPlaylist(_spotifyid))
            {
                mwsiveButton.ChangeAddToPlaylistButtonColor(0.5f);
                //Pintar de morado el que está en playlist
            }
        }
        
        if(_url != null){
            uris[0] = _url;
        }
        
        if(_previewURL != null){
            previewURL = _previewURL;
        }
        
        if(_externalURL != null){
            externalURL = _externalURL;
        }
        

        
    }

    public void PlayAudioPreview()
    {
        //StartSearch();
        SpotifyPreviewAudioManager.instance.GetTrack(previewURL, Callback_GetTrack);
    }

    private void Callback_GetTrack(object[] _list)
    {
        loadingAnimGameObject.SetActive(false);
        
        if (SurfManager.instance.isActiveAndEnabled == false)
        {
            if (NewScreenManager.instance.GetCurrentView().GetViewID() != ViewID.SurfViewModel)
            {
                SpotifyPreviewAudioManager.instance.StopTrack();
            }
            
        }
        
        //EndSearch();
    }

    public void OnClic_StopAudioPreview()
    {
        SpotifyPreviewAudioManager.instance.Pause();
    }
  
    public void OnClick_OpenPlaylist()
    {
        SurfManager.instance.SetActive(false);
        NewScreenManager.instance.ChangeToSpawnedView("surfMiPlaylist");
    }

    public void Swipe()
    {
        AddToPlaylist();
    }

    public void AddToPlaylist()
    {
        if (!AppManager.instance.SearchTrackOnCurrentPlaylist(trackID))
        {
            SpotifyConnectionManager.instance.AddItemsToPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, uris, Callback_AddToPlaylist);
        }
        else
        {
            UIMessage.instance.UIMessageInstanciate("Esta canción ya está en la playlist");
        }
    }

    public void AddToPlaylistButton()
    {
        if (!AppManager.instance.SearchTrackOnCurrentPlaylist(trackID))
        {
            SpotifyConnectionManager.instance.AddItemsToPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, uris, Callback_AddToPlaylist);
        }
        else
        {
            SpotifyConnectionManager.instance.RemoveItemsFromPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, uris, Callback_RemoveToPlaylist);
        }
    }

    private void Callback_AddToPlaylist(object[] _value)
    {
        AppManager.instance.RefreshCurrentPlaylistInformation((_list) => {
            mwsiveButton.ChangeAddToPlaylistButtonColor(0.5f);
            UIMessage.instance.UIMessageInstanciate("Canción agregada a la playlist");
        });
    }

    private void Callback_RemoveToPlaylist(object[] _value)
    {
        AppManager.instance.RefreshCurrentPlaylistInformation((_list) => {
            mwsiveButton.ChangeAddToPlaylistButtonColor(0.5f);
            UIMessage.instance.UIMessageInstanciate("Canción eliminada de la playlist");
        });
    }

    public void BackSwipe()
    {
        
    }

    public void CallBack_BackSwipe()
    {

    }

    public void Callback_CurrentUserPlaylist(object[] _value)
    {
         

    }

    public void SelectedPlaylistNameEventListener(SelectedPlaylistNameAppEvent _event)
    {
        playlistText.text = AppManager.instance.GetCurrentPlaylist().name;
        if (AppManager.instance.SearchTrackOnCurrentPlaylist(trackID))
        {
            mwsiveButton.ChangeAddToPlaylistButtonColor(0.5f);
            //Pintar de morado el que está en playlist
        }
        else
        {
            mwsiveButton.AddToPlaylistButtonColorButtonColorAgain(0.5f);
        }
    }

    public void OnClick_PlayOnSpotify()
    {
        Application.OpenURL(externalURL);
    }
}