using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaylistViewModel : ViewModel
{
    [Header("Instance Referecnes")]
    public GameObject trackHolderPrefab, SurfButton;
    public Transform instanceParent;
    public GameObject listenOnSpotify;
    private string id;
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
    public bool isExplore = false;
    public GameObject shimmer;

    private List<Track> trackList = new List<Track>();
    private SpotifyPlaylistRoot searchedPlaylist;
    public PlaylistRoot playlist  = null;
    private int NumberofTracks, NumberofTracksToCompare;


    public override void Initialize(params object[] list)
    {
#if PLATFORM_ANDROID
        SetAndroidBackAction();
#endif
    }

    public void GetPlaylist()
    {
        if (!id.Equals(""))
        {
            shimmer.SetActive(true);
            SpotifyConnectionManager.instance.GetPlaylist(id, Callback_GetPlaylist);
            SpotifyConnectionManager.instance.GetPlaylistItems(id, Callback_SurfButton, "ES", 100);
        }
    }

    private void InstanceTrackObjects(Tracks _tracks)
    {
        ClearScroll(instanceParent);
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
            shimmer.SetActive(false);
        }
        if (NumberofTracks == NumberofTracksToCompare)
        {
            SurfButton.SetActive(false);
        }
        if(_tracks.items.Count == 0){
            shimmer.SetActive(false);
            UIMessage.instance.UIMessageInstanciate("Esta Playlist esta vacia");
        }
    }

    private void Callback_GetPlaylist(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        searchedPlaylist = (SpotifyPlaylistRoot)_value[1];

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
                SpotifyConnectionManager.instance.GetPlaylistItems(id, Callback_GetMorePlaylist, "ES", 50, offset);
                
                offset += 50;
                onlyone = 1;
            }
        }
    }
    private void Callback_GetMorePlaylist(object[] _value)
    {
        Debug.Log("more");
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        PlaylistRoot playlistRoot = (PlaylistRoot)_value[1];
        NumberofTracks = 0;
        NumberofTracksToCompare = 0;
        
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
                NumberofTracksToCompare++;
                if (NumberofTracks == NumberofTracksToCompare)
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
        NewScreenManager.instance.GetCurrentView().SetAndroidBackAction();

        SpotifyPreviewAudioManager.instance.StopTrack();
        if (NewScreenManager.instance.GetCurrentView().GetComponent<ProfileViewModel>())
        {
            NewScreenManager.instance.GetCurrentView().GetComponent<ProfileViewModel>().SetAndroidBackAction();
        }

        if (NewScreenManager.instance.GetCurrentView().GetComponent<SurfMiPlaylistViewModel>())
        {
            NewScreenManager.instance.GetCurrentView().GetComponent<SurfMiPlaylistViewModel>().SetAndroidBackAction();
        }
    }

    public void OnClick_SurfButton(){

        Debug.Log(isExplore);
        if (trackList.Count > 0)
        {
            NewScreenManager.instance.ChangeToSpawnedView("surf");
            NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().DynamicPrefabSpawnerSeveralTracks(trackList);
            
        }
        else
        {
            
            if (searchedPlaylist.tracks.items.Count == 0)
            {
                UIMessage.instance.UIMessageInstanciate("Esta Playlist no tiene contenido");
            }
            else
            {
                
                NewScreenManager.instance.ChangeToSpawnedView("surf");
                try
                {
                    NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().DynamicPrefabSpawnerPLItems(new object[] { playlist }, true, true, id);
                }
                catch (System.NullReferenceException)
                {
                    SpotifyConnectionManager.instance.GetPlaylistItems(id, Callback_SurfButtonNoPl, "ES", 100);
                }                

                
            }
        }
        
        
    }

    private void Callback_SurfButton(object[] _value)
    {
        playlist = (PlaylistRoot)_value[1];
        
    }
    private void Callback_SurfButtonNoPl(object[] _value)
    {
        playlist = (PlaylistRoot)_value[1];
        NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().DynamicPrefabSpawnerPLItems(new object[] { playlist }, true, true, id);

    }

    public void GetSeveralTracks(string[] _tracksID, string _playlist_name)
    {
        shimmer.SetActive(true);
        isExplore = true;
        playlistName.text = _playlist_name;
        SpotifyConnectionManager.instance.GetSeveralTracks(_tracksID, Callback_GetSeveralTracks);
    }

    public void Callback_GetSeveralTracks(object[] _value)
    {
        SeveralTrackRoot severalTrackRoot = (SeveralTrackRoot)_value[1];

        foreach (Track track in severalTrackRoot.tracks)
        {
            if (track != null)
            {
                trackList.Add(track);
                TrackHolder instance = GameObject.Instantiate(trackHolderPrefab, instanceParent).GetComponent<TrackHolder>();
                artists = "";
                foreach (Artist artist in track.artists) { artists += artist.name + ", "; }
                instance.Initialize(track.name, artists, track.id, track.artists[0].id, track.uri, track.preview_url, track.external_urls);
                if (track.album.images != null && track.album.images.Count > 0)
                    instance.SetImage(track.album.images[0].url);
                if (track.preview_url == null)
                {
                    instance.PreviewUrlGrey();
                    NumberofTracks++;
                }
            }

        }
        shimmer.SetActive(false);
    }

    public void SetPlaylistID(string _id) { id = _id; }
    public string GetPlaylistID(string _id) { return id; }

    private void ClearScroll(Transform _scroll)
    {
        for (int i = 1; i < _scroll.childCount; i++)
        {
            Destroy(_scroll.GetChild(i).gameObject);
        }
    }

    public override void SetAndroidBackAction()
    {
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
}

