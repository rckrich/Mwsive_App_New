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
    public List<Image> imagenes;
    public List<string> uri;
    public bool isSelected = false;
    
   

    void Start()
    {
        StartSearch();
        SpotifyConnectionManager.instance.GetCurrentUserPlaylists(Callback_OnClick_GetCurrentUserPlaylists);
    }

    private void Callback_OnClick_GetCurrentUserPlaylists(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        PlaylistRoot playlistRoot = (PlaylistRoot)_value[1];

        foreach (Item item in playlistRoot.items)
        {           
            SongMiplaylistHolder instance = GameObject.Instantiate(playlistHolderPrefab, instanceParent).GetComponent<SongMiplaylistHolder>();
            instance.Initialize(item.name, item.id, item.owner.display_name, item.external_urls);
            if (item.images != null && item.images.Count > 0) { instance.SetImage(item.images[0].url); }
        }
        EndSearch();
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
        
    }
     public void OnClickDone()
     {
        isSelected = AppManager.instance.isSelected;
        uri[0] = AppManager.instance.uri;       
        if (!uri[0].Equals("") && isSelected)
        {
            SpotifyConnectionManager.instance.AddItemsToPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, uri, Callback_OnCLick_AddItemsToPlaylist);
        }
        else
        {
            UIMessage.instance.UIMessageInstanciate("Debe seleccionar una playlist");
        }
            

        
    }
    private void Callback_OnCLick_AddItemsToPlaylist(object[] _value)
    {
        NewScreenManager.instance.BackToPreviousView();
    }
    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
    }

    public void OnClick_SpawnCrearPlaylistButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("crearPlaylist");
    }

}
