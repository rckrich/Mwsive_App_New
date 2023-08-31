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
    public TrackViewModel trackViewModel;
    public string url;
    public string uri;

    public Button AddItemsToPlaylistButton;

    private void Start()
    {
        if (AddItemsToPlaylistButton != null)
        {
            AddItemsToPlaylistButton.interactable = AppManager.instance.isLogInMode;
            AddItemsToPlaylistButton.GetComponentInChildren<Image>().color = AppManager.instance.isLogInMode ? AddItemsToPlaylistButton.colors.normalColor : AddItemsToPlaylistButton.colors.disabledColor;
        }

#if PLATFORM_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            AppManager.instance.SetAndroidBackAction(() => {
                if (finishedLoading)
                {
                    OnClick_BackButton();
                }
            });
        }
#endif

    }

    public void OnClick_AddItemsToPlaylist()
    {
        uri = AppManager.instance.uri;      
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
       url = AppManager.instance.url;
       Application.OpenURL(url);
    }

    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
#if PLATFORM_ANDROID
        AppManager.instance.SetAndroidBackAction(null);
#endif
    }

}
