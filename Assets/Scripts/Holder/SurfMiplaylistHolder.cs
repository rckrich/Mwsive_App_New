using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SurfMiplaylistHolder : ViewModel
{

    public TextMeshProUGUI playlistName;
    public Image playlistPicture;
    public TextMeshProUGUI playlistOwner;
    private string spotifyID;
    public PlaylistViewModel playlistViewModel;
    public SurfMiPlaylistViewModel miPlaylistViewModel;
    public ChangeImage change;
    public bool @public;
    private bool isEnabled;
    public bool changeBool;
    public string description;
    public ExternalUrls url;
    public AppManager appManager;
    
    public void SetOnSelectedPlaylist(bool _enabled) { isEnabled = _enabled; }
    public bool GetOnSelectedPlaylist() { return isEnabled; }
    
    public void OnEnable()
    {
        AddEventListener<OnSelectedPlaylistClick>(SelectedPlaylistEventListener);        
    }

    private void OnDisable()
    {
        
        RemoveEventListener<OnSelectedPlaylistClick>(SelectedPlaylistEventListener);
    }
   
    public void Initialize(string _playlistName, string _spotifyID, string _owner, bool _public, string _description, ExternalUrls _url)
    {
        if (_playlistName != null)
        {
            if (_playlistName.Length > 27)
            {
                string _text2 = "";
                for (int i = 0; i < 27; i++)
                {
                    _text2 = _text2 + _playlistName[i];
                }
                _text2 = _text2 + "...";
                playlistName.text = _text2;
            }
            else
            {
                playlistName.text = _playlistName;
            }
        }
        spotifyID = _spotifyID;
        playlistOwner.text = _owner;
        @public = _public;
        description = _description;
        url = _url;
    }

    public void Initialize(string _playlistName, string _spotifyID, string _owner, bool _public, string _description, ExternalUrls _url, string _pictureURL)
    {
        if (_playlistName != null)
        {
            if (_playlistName.Length > 27)
            {
                string _text2 = "";
                for (int i = 0; i < 27; i++)
                {
                    _text2 = _text2 + _playlistName[i];
                }
                _text2 = _text2 + "...";
                playlistName.text = _text2;
            }
            else
            {
                playlistName.text = _playlistName;
            }
        }
        spotifyID = _spotifyID;
        playlistOwner.text = _owner;
        @public = _public;
        description= _description;
        url= _url;
        ImageManager.instance.GetImage(_pictureURL, playlistPicture, (RectTransform)this.transform);
    }

    public void PublicTrue()
    {
        change.True();
    }

    public void SetImage(string _pictureURL)
    {
        ImageManager.instance.GetImage(_pictureURL, playlistPicture, (RectTransform)this.transform);
    }


    public void OnClick_SpawnPlaylistButton()
    {
        playlistViewModel.playlistName.text = playlistName.text;
        playlistViewModel.id = spotifyID;
        NewScreenManager.instance.ChangeToSpawnedView("playlist");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }

    public void SelectedPlaylistEventListener(OnSelectedPlaylistClick _enable)
    {
        SetOnSelectedPlaylist(false);
        gameObject.GetComponent<Image>().enabled = false;
    }

    public void OnClickSelected()
    {
        AppManager.instance.ChangeCurrentPlaylist(spotifyID);
        gameObject.GetComponent<Image>().enabled = true;
    }
    
    public void ChangePublic()
    {
        if (@public) { changeBool = false; }
        else { changeBool = true; }
        if (!spotifyID.Equals("") && !playlistName.text.Equals(""))
            SpotifyConnectionManager.instance.ChangePlaylistDetails(spotifyID, Callback_OnCLick_ChangePlaylistDetails, playlistName.text, description, changeBool);
    }

    private void Callback_OnCLick_ChangePlaylistDetails(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        SpotifyConnectionManager.instance.GetPlaylist(spotifyID, Callback_OnCLick_GetPlaylist);
    }

    private void Callback_OnCLick_GetPlaylist(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;
       
    }
}
