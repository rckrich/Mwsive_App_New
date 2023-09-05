using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiPlaylistViewModel : ViewModel
{
    private const int LIMIT = 20;
    // Start is called before the first frame update
    public GameObject playlistHolderPrefab;
    public Transform instanceParent;
    public ScrollRect scrollRect;
    public float end;
    public int offset = 20;
    int onlyone = 0;
    public string profileID;
    public RectTransform Scroll;



    public void GetCurrentUserPlaylist()
    {
        offset = 20;
        SpotifyConnectionManager.instance.GetCurrentUserPlaylists(Callback_OnClick_GetCurrentUserPlaylists);
    }

    private void Callback_OnClick_GetCurrentUserPlaylists(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        PlaylistRoot playlistRoot = (PlaylistRoot)_value[1];

        foreach (Item item in playlistRoot.items)
        {
            MiplaylistHolder instance = GameObject.Instantiate(playlistHolderPrefab, instanceParent).GetComponent<MiplaylistHolder>();
            instance.Initialize(item.name, item.id, item.owner.display_name, item.@public, item.external_urls);
            if (!item.@public) { instance.PublicTrue(); }
            if (item.images != null && item.images.Count > 0) { instance.SetImage(item.images[0].url); }              
        }
    }

    public void GetUserPlaylist(string _profileId)
    {
        profileID = _profileId;
        offset = 20;
        SpotifyConnectionManager.instance.GetUserPlaylists(_profileId, Callback_OnClick_GetCurrentUserPlaylists);
    }

    public void OnReachEnd()
    {
        if (profileID.Equals(""))
        {
            if (onlyone == 0)
            {
                if (scrollRect.verticalNormalizedPosition <= end)
                {
                    SpotifyConnectionManager.instance.GetCurrentUserPlaylists(Callback_GetMoreUserPlaylists, LIMIT, offset);
                    offset += 20;
                    onlyone = 1;
                    Debug.Log("iiiiiiii");
                }
            }
        }
        else
        {
            if (onlyone == 0)
            {
                if (scrollRect.verticalNormalizedPosition <= end)
                {
                    SpotifyConnectionManager.instance.GetUserPlaylists(profileID, Callback_GetMoreUserPlaylists,LIMIT, offset);
                    offset += 20;
                    onlyone = 1;
                    Debug.Log("ooooooo");
                }
            }
        }
        
    }

    private void Callback_GetMoreUserPlaylists(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        PlaylistRoot playlistRoot = (PlaylistRoot)_value[1];

        foreach (Item item in playlistRoot.items)
        {
            MiplaylistHolder instance = GameObject.Instantiate(playlistHolderPrefab, instanceParent).GetComponent<MiplaylistHolder>();
            instance.Initialize(item.name, item.id, item.owner.display_name, item.@public, item.external_urls);
            if (!item.@public) { instance.PublicTrue(); }
            if (item.images != null && item.images.Count > 0)
                instance.SetImage(item.images[0].url);
        }
        onlyone = 0;
    }
 
    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
    }

    public void OnClick_SpawnCrearPlaylistButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("crearPlaylist");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }
}
