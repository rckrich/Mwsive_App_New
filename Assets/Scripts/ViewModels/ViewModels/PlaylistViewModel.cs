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
    public GameObject trackHolderPrefab, SurfButton;
    public Transform instanceParent;
   //public int objectsToNotDestroyIndex;
    public string id;
    public TextMeshProUGUI playlistName;
    public bool @public;
    public GameObject button, PF_SURF;
    public string artists;
    public float end;
    public int offset = 1;
    int onlyone = 0;
    public ScrollRect scrollRect;
    public ExternalUrls url;
    public string stringUrl;

    private SearchedPlaylist searchedPlaylist;
    private int NumberofTracks, NumberofTracksToCompare;

   
    void Start()
    {
        GetPlaylist();
    }

    public void GetPlaylist()
    {
        if (!id.Equals(""))
        {
            SpotifyConnectionManager.instance.GetPlaylist(id, Callback_GetPlaylist);
            

        }
    }
    private void InstanceTrackObjects(Tracks _tracks)
    {
        NumberofTracks = 0;
        NumberofTracksToCompare = 0;
        foreach (Item item in _tracks.items)
        {
            if(item.track != null)
            {
                TrackHolder instance = GameObject.Instantiate(trackHolderPrefab, instanceParent).GetComponent<TrackHolder>();
                artists = "";
                foreach (Artist artist in item.track.artists) { artists += artist.name + ", "; }
                instance.Initialize(item.track.name, artists, item.track.id, item.track.artists[0].id, item.track.uri, item.track.preview_url, item.track.external_urls);
                if (item.track.album.images != null && item.track.album.images.Count > 0)
                    instance.SetImage(item.track.album.images[0].url);
                if (item.track.preview_url == null)
                {
                    instance.PreviewUrlGrey();
                    NumberofTracks++;
                }
                offset++;
                NumberofTracksToCompare++;
            }
            
        }
        Debug.Log(NumberofTracks);
        Debug.Log(NumberofTracksToCompare);

        if (NumberofTracks == NumberofTracksToCompare)
        {
            

            SurfButton.SetActive(false);
        }
    }
    

    private void Callback_GetPlaylist(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        searchedPlaylist = (SearchedPlaylist)_value[1];

        if (searchedPlaylist.name.Length > 27)
        {
            string _text2 = "";
            for (int k = 0; k < 27; k++)
            {

                _text2 = _text2 + searchedPlaylist.name[k];

            }
            _text2 = _text2 + "...";
            playlistName.text = _text2;
        }
        else
        {
            playlistName.text = searchedPlaylist.name;
        }
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
        Debug.Log("more");
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        PlaylistRoot playlistRoot = (PlaylistRoot)_value[1];
        NumberofTracks = 0;
        
        foreach (Item item in playlistRoot.items)
        {
            if(item.track != null)
            {
                TrackHolder instance = GameObject.Instantiate(trackHolderPrefab, instanceParent).GetComponent<TrackHolder>();
                artists = "";
                foreach (Artist artist in item.track.artists) { artists += artist.name + ", "; }
                instance.Initialize(item.track.name, artists, item.track.id, item.track.artists[0].id, item.track.uri, item.track.preview_url, item.track.external_urls);
                if (item.track.album.images != null && item.track.album.images.Count > 0)
                {
                    instance.SetImage(item.track.album.images[0].url);
                    if (item.track.preview_url == null)
                    {
                        instance.PreviewUrlGrey();
                        NumberofTracks++;
                    }
                }
                if (NumberofTracks == playlistRoot.items.Count)
                {
                    SurfButton.SetActive(false);
                }
            }
            

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

