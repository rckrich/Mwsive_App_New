using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiPlaylistViewModel : ViewModel
{
    // Start is called before the first frame update
    public GameObject playlistHolderPrefab;
    public Transform instanceParent;
    public ScrollRect scrollRect;
    public float end;
    public int offset = 21;
    int onlyone = 0;
    void Start()
    {
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

    public void OnReachEnd()
    {
       
        if (onlyone == 0)
        {
            if (scrollRect.verticalNormalizedPosition <= end)
            {
                SpotifyConnectionManager.instance.GetCurrentUserPlaylists(Callback_GetMoreUserPlaylists, 20, offset);
                offset += 20;
                onlyone = 1;
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
    public void ResetScroll()
    {
        if (scrollRect.verticalNormalizedPosition >= 1.1)
        {
            for (int i = 0; i < instanceParent.childCount; i++)
            {
                Destroy(instanceParent.GetChild(i).gameObject);
            }
            SpotifyConnectionManager.instance.GetCurrentUserPlaylists(Callback_OnClick_GetCurrentUserPlaylists);
        }
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
