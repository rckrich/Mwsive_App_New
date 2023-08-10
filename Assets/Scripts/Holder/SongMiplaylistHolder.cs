using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SongMiplaylistHolder : ViewModel
{

    public TextMeshProUGUI playlistName;
    public Image playlistPicture;
    public TextMeshProUGUI playlistOwner;
    private string spotifyID;
    public PlaylistViewModel playlistViewModel;
    public SurfMiPlaylistViewModel miPlaylistViewModel;
    public bool @public;
    public HolderManager holderManager;
    private bool isEnabled;
    public GameObject selected;
    public ExternalUrls url;
    public void SetOnSelectedPlaylist(bool _enabled) { isEnabled = _enabled; }
    public bool GetOnSelectedPlaylist() { return isEnabled; }
    public void OnEnable()
    {
        AddEventListener<OnSelectedPlaylistClick>(SelectedPlaylistEventListener);
    }
    public void OnDisable()
    {
        RemoveEventListener<OnSelectedPlaylistClick>(SelectedPlaylistEventListener);
    }
    public void Initialize(string _playlistName, string _spotifyID, string _owner, ExternalUrls _url)
    {
        playlistName.text = _playlistName;
        spotifyID = _spotifyID;
        playlistOwner.text = _owner;      
        url = _url;
    }

    public void Initialize(string _playlistName, string _spotifyID, string _owner,ExternalUrls _url, string _pictureURL)
    {
        playlistName.text = _playlistName;
        spotifyID = _spotifyID;
        playlistOwner.text = _owner;
        url = _url;
        ImageManager.instance.GetImage(_pictureURL, playlistPicture, (RectTransform)this.transform);
    }

    public void SetImage(string _pictureURL)
    {
        ImageManager.instance.GetImage(_pictureURL, playlistPicture, (RectTransform)this.transform);
    }


    public void OnClick_SpawnPlaylistButton()
    {
        playlistViewModel.playlistName.text = playlistName.text;
        playlistViewModel.id = spotifyID;
        playlistViewModel.@public = @public;
        holderManager.playlistExternalUrl = url;
        NewScreenManager.instance.ChangeToSpawnedView("playlist");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }
    public void SelectedPlaylistEventListener(OnSelectedPlaylistClick _enable)
    {
        SetOnSelectedPlaylist(false);
        selected.SetActive(false);
    }
    public void OnClickSelected()
    {
        AppManager.instance.ChangeCurrentPlaylist(spotifyID);        
        selected.SetActive(true);
        
    }
    public void Charging()
    {
        selected.SetActive(true);
    }
    public void NoSelected()
    {
        selected.SetActive(false);
    }


}
