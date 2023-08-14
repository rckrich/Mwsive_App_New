using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaylistViewModel : ViewModel
{
    // Start is called before the first frame update
    //public TMP_InputField playlistIDInputField;
    //public TextMeshProUGUI publicText;

    [Header("Instance Referecnes")]
    public GameObject trackHolderPrefab;
    public Transform instanceParent;
   //public int objectsToNotDestroyIndex;
    public string id;
    public TextMeshProUGUI playlistName;
    public bool @public;
    public GameObject button, PF_SURF;
    public string artists;
    public HolderManager holderManager;
    public float end;
    public int offset = 1;
    int onlyone = 0;
    public ScrollRect scrollRect;
    public ExternalUrls url;
    public string stringUrl;

    private SearchedPlaylist searchedPlaylist;
   
    void Start()
    {
        GetPlaylist();
    }

    public void GetPlaylist()
    {
        if (!id.Equals(""))
        {
            Debug.Log("cupidddd");
            SpotifyConnectionManager.instance.GetPlaylist(id, Callback_GetPlaylist);
            

        }
        /*
        if (!@public)
        {  
            button.GetComponent<ChangeImage>().OnClickToggle();
        }
        */
    }
    private void InstanceTrackObjects(Tracks _tracks)
    {

        foreach (Item item in _tracks.items)
        {
            TrackHolder instance = GameObject.Instantiate(trackHolderPrefab, instanceParent).GetComponent<TrackHolder>();
            artists = "";
            foreach(Artist artist in item.track.artists) { artists += artist.name + ", "; }
            instance.Initialize(item.track.name, artists, item.track.id, item.track.artists[0].id, item.track.uri, item.track.preview_url, item.track.external_urls); 
            if (item.track.album.images != null && item.track.album.images.Count > 0)
                instance.SetImage(item.track.album.images[0].url);
            offset++;
        }
    }
    

    private void Callback_GetPlaylist(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        searchedPlaylist = (SearchedPlaylist)_value[1];
        playlistName.text = searchedPlaylist.name;
        InstanceTrackObjects(searchedPlaylist.tracks);
        stringUrl = searchedPlaylist.external_urls.spotify;
       
    }
    public void OnReachEnd()
    {

        if (onlyone == 0)
        {
            if (scrollRect.verticalNormalizedPosition <= end)
            {
                SpotifyConnectionManager.instance.GetPlaylistItems(id, Callback_GetMorePLaylist, "ES", 50, offset);
                offset += 50;
                onlyone = 1;
            }
        }
    }
    private void Callback_GetMorePLaylist(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        PlaylistRoot playlistRoot = (PlaylistRoot)_value[1];

        foreach (Item item in playlistRoot.items)
        {
            TrackHolder instance = GameObject.Instantiate(trackHolderPrefab, instanceParent).GetComponent<TrackHolder>();
            artists = "";
            foreach (Artist artist in item.track.artists) { artists += artist.name + ", "; }
            instance.Initialize(item.track.name, artists, item.track.id, item.track.artists[0].id, item.track.uri, item.track.preview_url, item.track.external_urls);
            if (item.track.album.images != null && item.track.album.images.Count > 0)
                instance.SetImage(item.track.album.images[0].url);
        }
        onlyone = 0;

    }
    public void OnClickListenInSpotify()
    {
        Application.OpenURL(stringUrl);
    }

    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
        SpotifyPreviewAudioManager.instance.StopTrack();
    }

    public void OnClick_SurfButton(){
        if(searchedPlaylist.tracks.items.Count == 0){
            UIMessage.instance.UIMessageInstanciate("Esta Playlist no tiene contenido");
        }else{

            NewScreenManager.instance.ChangeToSpawnedView("surf");
            NewScreenManager.instance.GetCurrentView().GetComponentInChildren<SurfManager>().DynamicPrefabSpawnerPL(new object[] { searchedPlaylist });
        }
        
    }
}

