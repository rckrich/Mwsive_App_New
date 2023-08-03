using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddSongOptions : ViewModel
{
    public string playlistID;
    public List<string> trackSpotifyUris = new List<string>();
    public string trackID;
    [Header("Instance References")]
    public GameObject trackHolderPrefab;
    public Transform instanceParent;
    public int objectsToNotDestroyIndex;
    public HolderManager holderManager;
    public TrackViewModel trackViewModel;
    public ExternalUrls url;
    public string stringUrl;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void OnClick_AddItemsToPlaylist()
    {
        holderManager.trackID = trackID;
        NewScreenManager.instance.ChangeToSpawnedView("songMiPlaylist");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }

    private void Callback_OnCLick_AddItemsToPlaylist(object[] _value)
    {
        //if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        //SpotifyConnectionManager.instance.GetPlaylist(playlistID, Callback_OnCLick_GetPlaylist);
    }
    public void OnClickSong()
    {
         trackViewModel.trackID = trackID;
        NewScreenManager.instance.ChangeToSpawnedView("cancion");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }
    public void ListenOnSpotify()
    {
        url = holderManager.trackExternalUrl;
        stringUrl = url.spotify.ToString();
        Application.OpenURL(stringUrl);
    }
    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
    }

}
