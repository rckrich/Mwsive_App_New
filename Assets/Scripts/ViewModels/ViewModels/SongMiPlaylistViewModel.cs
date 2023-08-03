using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongMiPlaylistViewModel : ViewModel 
{
    // Start is called before the first frame update
    public GameObject playlistHolderPrefab;
    public Transform instanceParent;
    public ScrollRect scrollRect;
    public float end;
    public int offset = 21;
    int onlyone = 0;
    public HolderManager holderManager;
    public List<Image> imagenes;
    public SongMiplaylistHolder holder;
   

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
            if (holderManager.playlistId == item.id)
            {

                holder.Charging();
            }
            else { holder.NoSelected(); }
                     
            SongMiplaylistHolder instance = GameObject.Instantiate(playlistHolderPrefab, instanceParent).GetComponent<SongMiplaylistHolder>();
            instance.Initialize(item.name, item.id, item.owner.display_name, item.@public);
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
            if (holderManager.playlistId == item.id)
            {

                holder.Charging();
            }
            else { holder.NoSelected(); }
            SongMiplaylistHolder instance = GameObject.Instantiate(playlistHolderPrefab, instanceParent).GetComponent<SongMiplaylistHolder>();
            instance.Initialize(item.name, item.id, item.owner.display_name, item.@public);
            if (item.images != null && item.images.Count > 0)
                instance.SetImage(item.images[0].url);
            
        }
        onlyone = 0;
    }
     public void OnClickDone()
    {
        if (!holderManager.playlistId.Equals(""))
            SpotifyConnectionManager.instance.AddItemsToPlaylist(holderManager.playlistId, holderManager.uri, Callback_OnCLick_AddItemsToPlaylist);

        NewScreenManager.instance.BackToPreviousView();
    }
    private void Callback_OnCLick_AddItemsToPlaylist(object[] _value)
    {
        //if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        //SpotifyConnectionManager.instance.GetPlaylist(playlistID, Callback_OnCLick_GetPlaylist);
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
