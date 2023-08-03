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
    public void SetSelectedPlaylistNameAppEvent(string _playlistName) { playlistName = _playlistName; }
    public string GetSelectedPlaylistNameAppEvent() { return playlistName; }
    private void OnEnable()
    {
        playlistText.text = holderManager.playlistName;
        AddEventListener<SelectedPlaylistNameAppEvent>(SelectedPlaylistNameEventListener);
    }
    private void OnDisable()
    {
        RemoveEventListener<SelectedPlaylistNameAppEvent>(SelectedPlaylistNameEventListener);
    }
    // Start is called before the first frame update
    void Start()
    {
        playlistText.text = holderManager.playlistName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SelectedPlaylistNameEventListener(SelectedPlaylistNameAppEvent _event)
    {
        playlistName = holderManager.playlistName;
        SetSelectedPlaylistNameAppEvent(playlistName);
        playlistText.text = playlistName;
    }
    public void OnClickPlaylistButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("surfMiPlaylist");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }

   
}
