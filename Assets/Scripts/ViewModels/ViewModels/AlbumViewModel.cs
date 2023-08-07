using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlbumViewModel : ViewModel
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
    public string artists;
    public float end;
    public int offset = 1;
    int onlyone = 0;
    public ScrollRect scrollRect;
    public ExternalUrls url;
    public string stringUrl;


    private string image;
   
    void Start()
    {
        GetAlbum();
    }
    public void GetAlbum()
    {
        if (!id.Equals(""))
        {
            SpotifyConnectionManager.instance.GetAlbum(id, Callback_GetPLayAlbum);
        }
        
    }
    private void InstanceTrackObjects(Tracks _tracks)
    {
        Debug.Log(_tracks.items.Count);
        foreach (Item item in _tracks.items)
        {

            TrackHolder instance = GameObject.Instantiate(trackHolderPrefab, instanceParent).GetComponent<TrackHolder>();
            artists = "";
            foreach(Artist artist in item.artists) { artists += artist.name + ", "; }
            

            instance.Initialize(item.name, artists, item.id, item.artists[0].id, item.uri, item.preview_url, item.external_urls); 
            
            instance.SetImage(image);
            offset++;
        }
    }
    

    private void Callback_GetPLayAlbum(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        AlbumRoot _Album = (AlbumRoot)_value[1];
        if(_Album.images[0].url != null){
            image = _Album.images[0].url;
        }
    
        InstanceTrackObjects(_Album.tracks);
        stringUrl = _Album.external_urls.spotify;
       
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
    }
}
