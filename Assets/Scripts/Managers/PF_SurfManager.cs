using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using GG.Infrastructure.Utils.Swipe;


public class PF_SurfManager : Manager
{

    public SwipeListener swipeListener;
    public ScrollRect Controller;
    public GameObject Prefab, AddSong, OlaButton, MwsiveOla, MwsiveContainer;
    public List<GameObject> MwsiveSongs = new List<GameObject>();
    public GameObject[] RestPositions;
    public GameObject loadingCard;

    public float MaxRotation = 18f;
    public float SurfSuccessSensitivity = 2.2f;
    public Vector2 LeftRightOffset;
    public bool CanGetRecomendations = false;
    private SpotifyPlaylistRoot searchedPlaylist;
    private PlaylistRoot ProfileItemsPlaylist;
    private RecommendationsRoot recommendationsRoot;
    private PlaylistRoot UserPlaylists;
    private AlbumRoot albumroot;
    private ChallengeAppObject challenge;
    private TrackInfoRoot trackInfoRoot;

    public List<MwsiveData> MwsiveSongsData = new List<MwsiveData>();
    public List<GameObject> ActiveMwsiveSongs = new List<GameObject>();
    public int CurrentPosition = 0;
    public int SpawnPosition = 0;

    [HideInInspector]
    public bool canSwipe = true;
    public float time = 0;
    public bool HasSideScrollEnded = true;
    public bool Challenge = false;

    private Vector2 ControllerPostion = new Vector2();
    
    private int PrefabPosition = 0;
    private string idPagingPlItems;
    private int offsetPagingPlItems;
    private int totalPagingPlItems;
    private int ProfilePlaylistPosition = 0;
    private bool HasSwipeEnded = true;
    private bool Success = false;
    
    private bool HasFirstPlaylistPlayed = false;

    [SerializeField]
    private int SongsMaxToPaginate;

    private void Start()
    {
        
        ControllerPostion = new Vector2(Controller.transform.position.x, Controller.transform.position.y);
    }


    private void OnEnable()
    {
        loadingCard.SetActive(true);
        SurfController.instance.AddToList(gameObject);
        
            
        
        

        GameObject currentPrefab = GetCurrentPrefab();

        if (currentPrefab != null && SurfController.instance.AmICurrentView(gameObject))
            currentPrefab.GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();

        if (SurfController.instance.AmICurrentView(gameObject))
        {
            AddEventListener<TimerAppEvent>(TimerAppEventListener);
            
        }


        if(MwsiveSongsData != null && MwsiveSongsData.Count > 0)
        {
            HasFirstPlaylistPlayed = false;
            SurfManagerLogicInitialize();
        }



    }

    private void OnDestroy() {
        
        SurfController.instance.DeleteFromList(gameObject);
    }

    private void OnDisable()
    {
        PoolManager.instance.RecoverPooledObject(MwsiveContainer);
        StopTimer();
        swipeListener.OnSwipe.RemoveListener(OnSwipe);
        RemoveEventListener<TimerAppEvent>(TimerAppEventListener);
        ActiveMwsiveSongs.Clear();

    }



    private void OnSwipe(string swipe)
    {
        if (!canSwipe || !HasSwipeEnded)
            return;

        switch (swipe) {
            case "Right":

                Controller.vertical = false;
                Controller.horizontal = false;
                HasSwipeEnded = false;
                SideScrollSuccess();

                break;
            case "Up":

                Controller.vertical = false;
                Controller.horizontal = false;
                HasSwipeEnded = false;
                UpScrollSuccess();
                break;
            case "Down":

                Controller.vertical = false;
                Controller.horizontal = false;
                HasSwipeEnded = false;
                DownScrollSuccess();
                break;
        }
        //Debug.Log(swipe);
    }

    public void ValChange() {

        if (Controller.transform.position.x > ControllerPostion.x * 1.1) {
            Controller.vertical = false;
            SideScrollAnimation();
        } if (Controller.transform.position.y > ControllerPostion.y * 1.1) {
            Controller.horizontal = false;
            UpScrollAnimation();
        } if (Controller.transform.position.y < ControllerPostion.y * .9) {
            Controller.horizontal = false;
            DownScrollAnimation();
        }


    }



    private void SideScrollAnimation() {
        
        float var = Controller.transform.position.x / ControllerPostion.x * .25f;
        float Fade = ControllerPostion.x / Controller.transform.position.x;

        ActiveMwsiveSongs[1].GetComponent<SurfAni>().SetValues(var, -MaxRotation, Fade, false);
        ActiveMwsiveSongs[1].GetComponent<SurfAni>().Play_SurfSide();

        AddSong.GetComponent<SurfAni>().SetValues(1, null, var);
        AddSong.GetComponent<SurfAni>().Play_SurfAddSong();

        if (CurrentPosition < MwsiveSongsData.Count - 1)
        {
            ActiveMwsiveSongs[2].GetComponent<SurfAni>().SetValues(var, null, 1, null, null, RestPositions[0]);
            ActiveMwsiveSongs[2].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            ActiveMwsiveSongs[3].GetComponent<SurfAni>().SetValues(var, null, 1, null, null, RestPositions[1]);
            ActiveMwsiveSongs[3].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            ActiveMwsiveSongs[4].GetComponent<SurfAni>().SetValues(var, null, 1, null, null, RestPositions[2]);
            ActiveMwsiveSongs[4].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();
        }
        Success = false;
    }


    private void DownScrollAnimation() {
        
        float var = Controller.transform.position.y / ControllerPostion.y;
        float Fade = ControllerPostion.y / Controller.transform.position.y;

        ActiveMwsiveSongs[1].GetComponent<SurfAni>().SetValues(var * -.5f, MaxRotation, Fade, true);
        ActiveMwsiveSongs[1].GetComponent<SurfAni>().Play_VerticalDown1();

        ActiveMwsiveSongs[2].GetComponent<SurfAni>().SetValues(var * .25f, null, 1, null, null, RestPositions[0]);
        ActiveMwsiveSongs[2].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

        ActiveMwsiveSongs[3].GetComponent<SurfAni>().SetValues(var * .25f, null, 1, null, null, RestPositions[1]);
        ActiveMwsiveSongs[3].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();
        
        Success = false;
    }


    private void UpScrollAnimation() {

        float var = Controller.transform.position.y / ControllerPostion.y;
        float Fade = Controller.transform.position.y / ControllerPostion.y;
        float VAR2 = ControllerPostion.y / Controller.transform.position.y;

        ActiveMwsiveSongs[1].GetComponent<SurfAni>().SetValues(var * .5f, MaxRotation, Fade, false);
        ActiveMwsiveSongs[1].GetComponent<SurfAni>().Play_VerticalUp();

        if (CurrentPosition < MwsiveSongsData.Count - 1) {

            ActiveMwsiveSongs[2].GetComponent<SurfAni>().SetValues(VAR2 * .25f, null, 1, null, null, RestPositions[0]);
            ActiveMwsiveSongs[2].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            ActiveMwsiveSongs[3].GetComponent<SurfAni>().SetValues(VAR2 * .25f, null, 1, null, null, RestPositions[1]);
            ActiveMwsiveSongs[3].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            ActiveMwsiveSongs[4].GetComponent<SurfAni>().SetValues(VAR2 * .25f, null, 1, null, null, RestPositions[2]);
            ActiveMwsiveSongs[4].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();
        }


        Success = false;
    }

    public void OnEndDrag() {
        while (HasSwipeEnded && HasSideScrollEnded) {
            if (ActiveMwsiveSongs[1].transform.position.x >= ControllerPostion.x * SurfSuccessSensitivity) {
                if (CurrentPosition > MwsiveSongs.Count - 4) {
                    SideScrollSuccess();
                }

                break;

            } else if (ActiveMwsiveSongs[1].transform.position.y >= ControllerPostion.y * SurfSuccessSensitivity * 1.5) {
                UpScrollSuccess();
                break;

            } else if (ActiveMwsiveSongs[1].transform.position.y <= ControllerPostion.y / SurfSuccessSensitivity) {
                DownScrollSuccess();
                break;

            } else {
                ResetValue();

                break;
            }
        }
        Controller.enabled = true;


    }

    private void SideScrollSuccess() {
        
        Controller.enabled = false;
        Controller.horizontal = true;
        Controller.vertical = true;
        Controller.transform.position = new Vector2(ControllerPostion.x, ControllerPostion.y);
        if (CurrentPosition < MwsiveSongsData.Count -1) {
            if (Challenge)
            {
                GetCurrentPrefab().GetComponentInChildren<ChallengeColorAnimation>().ForceClear();
            }
            SpotifyPreviewAudioManager.instance.StopTrack();

            ActiveMwsiveSongs[1].GetComponent<SurfAni>().SetValues(1, -MaxRotation, 0, true);
            ActiveMwsiveSongs[1].GetComponent<SurfAni>().Play_SurfSide();

            ActiveMwsiveSongs[2].GetComponent<SurfAni>().SetValues(1, null, 1, null, null, RestPositions[0]);
            ActiveMwsiveSongs[2].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            ActiveMwsiveSongs[3].GetComponent<SurfAni>().SetValues(1, null, 1, null, null, RestPositions[1]);
            ActiveMwsiveSongs[3].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            ActiveMwsiveSongs[4].GetComponent<SurfAni>().SetValues(1, null, 1, null, null, RestPositions[2]);
            ActiveMwsiveSongs[4].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            ActiveMwsiveSongs[1].GetComponent<ButtonSurfPlaylist>().ClearData();

            

            AddSong.GetComponent<SurfAni>().SetValues(1, null, 1);
            AddSong.GetComponent<SurfAni>().Play_CompleteAddSurfAddSong();

            GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().AddToPlaylistSwipe(GetCurrentMwsiveData().id, ResetTimer());
            CurrentPosition++;
            SpawnPosition++;





            SurfManagerLogic();
            Success = true;
        } else if (MwsiveSongsData.Count - 1 == CurrentPosition && HasSideScrollEnded) {
            HasSideScrollEnded = false;

            ActiveMwsiveSongs[1].GetComponent<SurfAni>().SetValues(1, -MaxRotation, 0, null, null, RestPositions[0]);
            ActiveMwsiveSongs[1].GetComponent<SurfAni>().Play_SurfSideLasPosition();
            GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().AddToPlaylistSwipe(GetCurrentMwsiveData().id, ResetTimer());

            ResetSideScroll();
            AddSong.SetActive(true);
            AddSong.GetComponent<SurfAni>().SetValues(1, null, 1);
            AddSong.GetComponent<SurfAni>().Play_CompleteAddSurfAddSong();

        } else {
            ResetValue();
        }
        
        HasSwipeEnded = true;
    }
    private void DownScrollSuccess() {
        
        Controller.enabled = false;
        Controller.horizontal = true;
        Controller.vertical = true;
        Controller.transform.position = new Vector2(ControllerPostion.x, ControllerPostion.y);
        
        if (CurrentPosition > 0) {
            ActiveMwsiveSongs[0].GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[CurrentPosition - 1]);
            SpotifyPreviewAudioManager.instance.StopTrack();
            if (Challenge)
            {
                GetCurrentPrefab().GetComponentInChildren<ChallengeColorAnimation>().ForceClear();
            }
            Success = true;

            ActiveMwsiveSongs[1].GetComponent<SurfAni>().SetValues(1, -MaxRotation, 0, true, true, RestPositions[1]);
            ActiveMwsiveSongs[1].GetComponent<SurfAni>().Play_VerticalDown1();

            ActiveMwsiveSongs[0].GetComponent<SurfAni>().SetValues(null, MaxRotation, null, null, null, RestPositions[0]);
            ActiveMwsiveSongs[0].GetComponent<SurfAni>().Play_SurfBackSong();

            ActiveMwsiveSongs[1].GetComponent<SurfAni>().SetValues(null, 1, null, null, null, RestPositions[1]);
            ActiveMwsiveSongs[1].GetComponent<SurfAni>().Play_SurfTransitionBackSongDown();

            ActiveMwsiveSongs[2].GetComponent<SurfAni>().SetValues(1, null, 1, null, null, RestPositions[2]);
            ActiveMwsiveSongs[2].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            ActiveMwsiveSongs[3].GetComponent<SurfAni>().SetValues(1, null, 1, null, null, RestPositions[3]);
            ActiveMwsiveSongs[3].GetComponent<SurfAni>().Play_SurfTransitionBackHideSong();

            ActiveMwsiveSongs[4].GetComponent<ButtonSurfPlaylist>().ClearData();

            AddSong.GetComponent<SurfAni>().Play_SurfAddsongReset();


            string _trackid = GetCurrentMwsiveData().id;
            if (AppManager.instance.isLogInMode && !_trackid.Equals(""))
            {
                MwsiveConnectionManager.instance.PostTrackAction(_trackid, "DOWN", ResetTimer());
            }
            CurrentPosition--;
            SpawnPosition--;
     
            
            ActiveMwsiveSongs[2].GetComponent<ButtonSurfPlaylist>().CheckIfDurationBarCanPlay();
            ActiveMwsiveSongs[3].GetComponent<ButtonSurfPlaylist>().CheckIfDurationBarCanPlay();
            SurfManagerLogicPreviousSong();
        }
        else {
            ResetValue();
        }
        
        HasSwipeEnded = true;
    }
    private void UpScrollSuccess() {
        Controller.enabled = false;
        Controller.horizontal = true;
        Controller.vertical = true;
        Controller.transform.position = new Vector2(ControllerPostion.x, ControllerPostion.y);
        if (CurrentPosition < MwsiveSongsData.Count -1) {
            SpotifyPreviewAudioManager.instance.StopTrack();
            if (Challenge)
            {
                GetCurrentPrefab().GetComponentInChildren<ChallengeColorAnimation>().ForceClear();
            }
            Success = true;

            ActiveMwsiveSongs[1].GetComponent<SurfAni>().SetValues(1, MaxRotation, 0, true);
            ActiveMwsiveSongs[1].GetComponent<SurfAni>().Play_VerticalUp();

            

            ActiveMwsiveSongs[2].GetComponent<SurfAni>().SetValues(1, null, 1, null, null, RestPositions[0]);
            ActiveMwsiveSongs[2].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            ActiveMwsiveSongs[3].GetComponent<SurfAni>().SetValues(1, null, 1, null, null, RestPositions[1]);
            ActiveMwsiveSongs[3].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            ActiveMwsiveSongs[4].GetComponent<SurfAni>().SetValues(1, null, 1, null, null, RestPositions[2]);
            ActiveMwsiveSongs[4].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            ActiveMwsiveSongs[1].GetComponent<ButtonSurfPlaylist>().ClearData();



            string _trackid = GetCurrentMwsiveData().id;
            if (AppManager.instance.isLogInMode && !_trackid.Equals(""))
            {
                MwsiveConnectionManager.instance.PostTrackAction(_trackid, "UP", ResetTimer());
            }

            CurrentPosition++;
            SpawnPosition++;
  

            SurfManagerLogic();
            UIAniManager.instance.SurfAddSongReset(AddSong);
        } else {
            ResetValue();
        }

        if(CurrentPosition >= MwsiveSongsData.Count - 5)
        {
            if (UserPlaylists != null)
            {
                SurfProfileADN();
            }
        }

        HasSwipeEnded = true;
    }

    public void ResetSideScroll() {


        DOTween.Complete(ActiveMwsiveSongs[2]);
        DOTween.Complete(ActiveMwsiveSongs[3]);
        DOTween.Complete(ActiveMwsiveSongs[4]);
        DOTween.Complete(AddSong);

        
        
    }

    public void OnCallback_ResetSideAnimation()
    {
        Reset();
        if (Success)
        {
            Success = false;
        }
    }

    public void ResetValue() {

        if (!Success) {
            ///DOTween.KillAll(false, new object[] { 0, 1 });
            ActiveMwsiveSongs[1].GetComponent<SurfAni>().Play_SurfReset();
            UIAniManager.instance.SurfReset(ActiveMwsiveSongs[1]);
            Reset();
        } else {
            Success = false;
        }

    }
    public void Reset() {

        Controller.horizontal = true;
        Controller.vertical = true;
        HasSwipeEnded = true;

        Controller.transform.position = new Vector2(ControllerPostion.x, ControllerPostion.y);

        AddSong.GetComponent<SurfAni>().Play_SurfAddsongReset();

        ActiveMwsiveSongs[2].GetComponent<SurfAni>().SetValues(null, null, null, null, true, RestPositions[1]);
        ActiveMwsiveSongs[2].GetComponent<SurfAni>().Play_SurfResetfOtherSongs();

        ActiveMwsiveSongs[3].GetComponent<SurfAni>().SetValues(null, null, null, null, true, RestPositions[2]);
        ActiveMwsiveSongs[3].GetComponent<SurfAni>().Play_SurfResetfOtherSongs();

        ActiveMwsiveSongs[4].GetComponent<SurfAni>().SetValues(null, null, null, null, true, RestPositions[3]);
        ActiveMwsiveSongs[4].GetComponent<SurfAni>().Play_SurfResetfOtherSongs();


        UIAniManager.instance.SurfResetOtherSongs(ActiveMwsiveSongs[2], RestPositions[1], true);
        UIAniManager.instance.SurfResetOtherSongs(ActiveMwsiveSongs[3], RestPositions[2], true);
        UIAniManager.instance.SurfResetOtherSongs(ActiveMwsiveSongs[4], RestPositions[3], true);
    }


    public GameObject GetBeforeCurrentPrefab() {
        GameObject _Instance = ActiveMwsiveSongs[0];
        return _Instance;
    }

    public GameObject GetCurrentPrefab() {
        try
        {
            return ActiveMwsiveSongs[1];
        }
        catch (System.ArgumentOutOfRangeException)
        {
            return null;
        }
        
    }
    public MwsiveData GetCurrentMwsiveData()
    {
        return MwsiveSongsData[CurrentPosition];
    }

    public MwsiveData GetBeforeCurrentMwsiveData()
    {
        return MwsiveSongsData[CurrentPosition-1];
    }

    public MwsiveData GetLastPrefab() {

        return MwsiveSongsData[MwsiveSongsData.Count - 1];


    }

    public List<MwsiveData> GetInstances() {
        return MwsiveSongsData;
    }
    public void SetChallengeCallback(ChallengeAppObject _challenge) {
        challenge = _challenge;
    }

    public void CheckChallengeEnd()
    {
        challenge.CheckForPoints();
    }

    public void DynamicPrefabSpawnerRecommendations(object[] _value)
    {
        recommendationsRoot = (RecommendationsRoot)_value[0];
            int SpawnedSongs = 0;
            foreach (var item in recommendationsRoot.tracks)
            {
                if (item != null )
                {
                    if (item.preview_url != null && item.preview_url != "Null")
                    {
                        

                        string artists = "";

                        foreach (Artist artist in item.artists)
                        {
                            artists = artists + artist.name + ", ";
                        }

                        artists = artists.Remove(artists.Length - 2);

                        string currentPlayListName = AppManager.instance.isLogInMode ? AppManager.instance.GetCurrentPlaylist().name : "";

                        MwsiveData instance = new MwsiveData();
                        instance.playlist_name = currentPlayListName;
                        instance.song_name = item.name;
                        instance.album_name = item.album.name;
                        instance.artists = artists;
                        instance.album_image_url = item.album.images[0].url;
                        instance.id = item.id;
                        instance.uri = item.uri;
                        instance.preview_url = item.preview_url;
                        instance.external_url = item.external_urls.spotify;

                        MwsiveSongsData.Add(instance);

                        SpawnedSongs++;
                    }
                }

            }


        SurfManagerLogic(true);
    }

    public void DynamicPrefabSpawnerAlbum(object[] _value)
    {
        albumroot = (AlbumRoot)_value[0];
            

            string image = albumroot.images[0].url;
            string albumname = albumroot.name;
            int SpawnedSongs = 0;

            foreach (var item in albumroot.tracks.items)
            {
                if (item != null)
                {
                    if (item.preview_url != null && item.preview_url != "Null")
                    {

                        

                        string artists = "";

                        foreach (Artist artist in item.artists)
                        {
                            artists = artists + artist.name + ", ";
                        }

                        artists = artists.Remove(artists.Length - 2);

                        string currentPlayListName = AppManager.instance.isLogInMode ? AppManager.instance.GetCurrentPlaylist().name : "";

                        MwsiveData instance = new MwsiveData();
                        instance.playlist_name = currentPlayListName;
                        
                        instance.song_name = item.name;
                        instance.album_name = albumname;
                        instance.artists = artists;
                        instance.album_image_url = image;
                        instance.id = item.id;
                        instance.uri = item.uri;
                        instance.preview_url = item.preview_url;
                        instance.external_url = item.external_urls.spotify;

                        MwsiveSongsData.Add(instance);
                    SpawnedSongs++;
                    }
                }


            }
        SurfManagerLogic(true);
    }


    public void DynamicPrefabSpawnerSong(object[] _value)
    {
        recommendationsRoot = (RecommendationsRoot)_value[0];

            int SpawnedSongs = 0;
            foreach (var item in recommendationsRoot.tracks)
            {
                if (item != null)
                {
                    if (item.preview_url != null)
                    {

                        string artists = "";

                        foreach (Artist artist in item.artists)
                        {
                            artists = artists + artist.name + ", ";
                        }

                        artists = artists.Remove(artists.Length - 2);

                        string currentPlayListName = AppManager.instance.isLogInMode ? AppManager.instance.GetCurrentPlaylist().name : "";

                        MwsiveData instance = new MwsiveData();
                        instance.playlist_name = currentPlayListName;
                        instance.song_name = item.name;
                        instance.album_name = item.album.name;
                        instance.artists = artists;
                        instance.album_image_url = item.album.images[0].url;
                        instance.id = item.id;
                        instance.uri = item.uri;
                        instance.preview_url = item.preview_url;
                        instance.external_url = item.external_urls.spotify;

                        MwsiveSongsData.Add(instance);

                        SpawnedSongs++;

                    }
                }

            }



        SurfManagerLogic(true);

    }
    public void DynamicPrefabSpawnerPLItems(object[] _value, bool PlayFirstTrack = true, bool CanSpawnBuffer = true, string _spotifyid = null, bool? FirstTimeSurf = null)
    {
        PlaylistRoot PageItemPlaylist = (PlaylistRoot)_value[0];
 
            int SpawnedSongs = 0;
            foreach (var item in PageItemPlaylist.items)
            {
                if (item.track != null)
                {
                    if (item.track.preview_url != null && item.preview_url != "Null")
                    {
 
                        string artists = "";

                        foreach (Artist artist in item.track.artists)
                        {
                            artists = artists + artist.name + ", ";
                        }

                        artists = artists.Remove(artists.Length - 2);

                        MwsiveData instance = new MwsiveData();

                        string currentPlaylistName = AppManager.instance.isLogInMode ? AppManager.instance.GetCurrentPlaylist().name : "";

                        
                        instance.playlist_name = currentPlaylistName;
                        instance.song_name = item.track.name;
                        instance.album_name = item.track.album.name;
                        instance.artists = artists;
                        instance.album_image_url = item.track.album.images[0].url;
                        instance.id = item.track.id;
                        instance.uri = item.track.uri;
                        instance.preview_url = item.track.preview_url;
                        instance.external_url = item.track.external_urls.spotify;

                        MwsiveSongsData.Add(instance);

                        
                        SpawnedSongs++;
                    }
                }

            }

        if (PageItemPlaylist.total > 100) {
            PageItemPlaylistPaginate(100, _spotifyid, PageItemPlaylist.total);
        }

        if(FirstTimeSurf == null)
        {
            SurfManagerLogic(true);
        }


    }

    private void PageItemPlaylistPaginate(int _offset, string _spotifyid = null, int total = 0)
    {
        if(_spotifyid != null) {
            idPagingPlItems = _spotifyid;
            offsetPagingPlItems = _offset;
            totalPagingPlItems = total;
        }
        
        if( offsetPagingPlItems != -1)
        {
            if (offsetPagingPlItems > totalPagingPlItems)
            {
                offsetPagingPlItems = totalPagingPlItems;
                SpotifyConnectionManager.instance.GetPlaylistItems(idPagingPlItems, OnCallback_SurfPagingPLitems, "ES", 100, offsetPagingPlItems);
                offsetPagingPlItems = -1;
            }
            else if (offsetPagingPlItems == totalPagingPlItems)
            {
                SpotifyConnectionManager.instance.GetPlaylistItems(idPagingPlItems, OnCallback_SurfPagingPLitems, "ES", 100, offsetPagingPlItems);
                offsetPagingPlItems = -1;
            }
            else
            {
                SpotifyConnectionManager.instance.GetPlaylistItems(idPagingPlItems, OnCallback_SurfPagingPLitems, "ES", 100, offsetPagingPlItems);
                offsetPagingPlItems = offsetPagingPlItems + 100;

            }
        }
        

    }

    private void OnCallback_SurfPagingPLitems(object[] _value)
    {
        
        PlaylistRoot PageItemPlaylist = (PlaylistRoot)_value[1];

        foreach (var item in PageItemPlaylist.items)
        {
            if (item.track != null)
            {
                if (item.track.preview_url != null && item.preview_url != "Null")
                {

                    string artists = "";

                    foreach (Artist artist in item.track.artists)
                    {
                        artists = artists + artist.name + ", ";
                    }

                    artists = artists.Remove(artists.Length - 2);

                    MwsiveData instance = new MwsiveData();

                    string currentPlaylistName = AppManager.instance.isLogInMode ? AppManager.instance.GetCurrentPlaylist().name : "";


                    instance.playlist_name = currentPlaylistName;
                    instance.song_name = item.track.name;
                    instance.album_name = item.track.album.name;
                    instance.artists = artists;
                    instance.album_image_url = item.track.album.images[0].url;
                    instance.id = item.track.id;
                    instance.uri = item.track.uri;
                    instance.preview_url = item.track.preview_url;
                    instance.external_url = item.track.external_urls.spotify;

                    MwsiveSongsData.Add(instance);


                    
                }
            }

        }
        Debug.LogWarning(MwsiveSongsData.Count);
        PageItemPlaylistPaginate(0, null, 0);
    }

    public void DynamicPrefabSpawnerPL(object[] _value, bool PlayFirstTrack = true, bool CanSpawnBuffer = true)
    {

        searchedPlaylist = (SpotifyPlaylistRoot)_value[0];

        int SpawnedSongs = 0;
        foreach (var item in searchedPlaylist.tracks.items)
        {
            if (item.track != null)
            {
                if (item.track.preview_url != null && item.preview_url != "Null")
                {
                        

                    string artists = "";

                    foreach (Artist artist in item.track.artists)
                    {
                        artists = artists + artist.name + ", ";
                    }

                    artists = artists.Remove(artists.Length - 2);
                        

                    string currentPlayListName = AppManager.instance.isLogInMode ? AppManager.instance.GetCurrentPlaylist().name : "";

                    MwsiveData instance = new MwsiveData();
                    instance.playlist_name = currentPlayListName;
                    instance.song_name = item.track.name;
                    instance.album_name = item.track.album.name;
                    instance.artists = artists;
                    instance.album_image_url = item.track.album.images[0].url;
                    instance.id = item.track.id;
                    instance.uri = item.track.uri;
                    instance.preview_url = item.track.preview_url;
                    instance.external_url = item.track.external_urls.spotify;

                    MwsiveSongsData.Add(instance);

                SpawnedSongs++;
                }
            }

        }

        SurfManagerLogic(true);
    }

    public void DynamicPrefabSpawnerSeveralTracks(List<Track> tracks, bool PlayFirstTrack = true, bool CanSpawnBuffer = true, bool FirstTimeSurf = true)
    {
        foreach (var item in tracks)
        {
            
            if (item != null)
            {

                if (item.preview_url != null && item.preview_url != "Null")
                {
                    
                    string artists = "";

                    foreach (Artist artist in item.artists)
                    {
                        artists = artists + artist.name + ", ";
                    }

                    artists = artists.Remove(artists.Length - 2);
                    string currentPlayListName = AppManager.instance.isLogInMode ? AppManager.instance.GetCurrentPlaylist().name : "";

                    MwsiveData instance = new MwsiveData();
                    instance.playlist_name = currentPlayListName;
                    instance.song_name = item.name;
                    instance.album_name = item.album.name;
                    instance.artists = artists;
                    instance.album_image_url = item.album.images[0].url;
                    instance.id = item.id;
                    instance.uri = item.uri;
                    instance.preview_url = item.preview_url;
                    instance.external_url = item.external_urls.spotify;
                    instance.challenge_trackpoints = challenge;

                    MwsiveSongsData.Add(instance);


                    
                }
            }

        }

        if (FirstTimeSurf)
        {
            SurfManagerLogic(true);
        }

        
        if (challenge)
        {
            GetLastPrefab().challenge_AmILastPosition = true;
        }
        

    }
    private void GetMwsiveInfo( )
    {
        if (AppManager.instance.isLogInMode)
        {
            MwsiveConnectionManager.instance.GetTrackInformation_Auth(GetCurrentMwsiveData().id, AppManager.instance.GetCurrentPlaylist().id, Callback_GetTrackInformation);
        }
        else
        {
            MwsiveConnectionManager.instance.GetTrackInformation_NoAuth(GetCurrentMwsiveData().id, Callback_GetTrackInformation);
        }
    }

    private void Callback_GetTrackInformation(object[] _value)
    {
        trackInfoRoot = (TrackInfoRoot)_value[1];

        if (AppManager.instance.isLogInMode)
        {
            if (trackInfoRoot.top_curators != null)
            {
                GetCurrentMwsiveData().top_curators = trackInfoRoot.top_curators;
            }
        }

        GetCurrentMwsiveData().isPicked = trackInfoRoot.is_piked;
        GetCurrentMwsiveData().isRecommended = trackInfoRoot.is_recommended;
        GetCurrentMwsiveData().total_piks = trackInfoRoot.total_piks;
        GetCurrentMwsiveData().total_recommendations = trackInfoRoot.total_recommendations;

        GameObject instance = GetCurrentPrefab();
        try
        {
            GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().InitializeMwsiveDB(GetCurrentMwsiveData());
        }
        catch (System.NullReferenceException)
        {

        }       
        
    }

    private void SurfManagerLogicInitialize()
    {
        swipeListener.OnSwipe.AddListener(OnSwipe);
        Controller.gameObject.SetActive(true);
        if (HasFirstPlaylistPlayed)
        {
            return;
        }
        SpawnPrefab();
        if (MwsiveSongsData.Count == 0)
        {
            UIMessage.instance.UIMessageInstanciate("Vuelve a Intentarlo");
            return;
        }
        if (MwsiveSongsData[CurrentPosition] != null)
        {
            GameObject instance = SpawnPrefab();
            GetMwsiveInfo();
            instance.GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[CurrentPosition]);
            instance.GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
            SpawnPosition = CurrentPosition;

        }
        if (MwsiveSongsData.Count-1 >= CurrentPosition + 1)
        {
             
            SpawnPrefab().GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[CurrentPosition + 1]);
            GetMwsiveInfo();
            SpawnPosition++;
        }
        else
        {
            SpawnPrefab();
        }
        if (MwsiveSongsData.Count-1 >= CurrentPosition + 2)
        {
            SpawnPrefab().GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[CurrentPosition + 2]);
            GetMwsiveInfo();
            SpawnPosition++;
        }
        else
        {
            SpawnPrefab();
        }
        if (MwsiveSongsData.Count-1 >= CurrentPosition + 3)
        {
            SpawnPrefab().GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[CurrentPosition + 3]);
            GetMwsiveInfo();
            SpawnPosition++;
        }
        else
        {
            SpawnPrefab();
        }
        HasFirstPlaylistPlayed = true;
        loadingCard.SetActive(false);
    }



    private void SurfManagerLogicPreviousSong()
    {
        ActiveMwsiveSongs[ActiveMwsiveSongs.Count - 1].GetComponent<SurfAni>().isAvailable = true;
        ActiveMwsiveSongs.RemoveAt(ActiveMwsiveSongs.Count-1);
        SpawnPrefabBack();




        GetMwsiveInfo();
        GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();

    }

    private void SurfManagerLogic(bool _firstTime = false)
    {
        
            if (!_firstTime)
            {

                GameObject instance = SpawnPrefab();
                GetMwsiveInfo();
                if (SpawnPosition < MwsiveSongsData.Count - 1 && SpawnPosition > 3)
                {

                    instance.GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[SpawnPosition]);

                }
                else if (SpawnPosition == MwsiveSongsData.Count - 1 && SpawnPosition > 3)
                {

                    //MwsiveSongsData[CurrentPosition + 3].challenge_AmILastPosition = true;
                    instance.GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[SpawnPosition]);

                }
                GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
            }
            else
            {
                SurfManagerLogicInitialize();
            }
        



        
        

    }

    private GameObject SpawnPrefabBack()
    {

        GameObject Instance = PoolManager.instance.GetPooledObject();
        Debug.LogWarning(Instance.name);
        
        Instance.GetComponent<CanvasGroup>().alpha = 0;
        Instance.transform.SetParent(MwsiveContainer.transform);
        Instance.transform.SetAsLastSibling();
        Instance.GetComponent<SurfAni>().isAvailable = false;
        ActiveMwsiveSongs.Insert(0, Instance);
        Instance.GetComponent<RectTransform>().offsetMin = new Vector2(LeftRightOffset.x, 0);
        Instance.GetComponent<RectTransform>().offsetMax = new Vector2(LeftRightOffset.y, 0);
        Instance.GetComponent<SurfAni>().ResetRestPosition();
        Instance.transform.eulerAngles = new Vector3(0, 0, 0);
        Instance.SetActive(true);
        return Instance;
    }

    private GameObject SpawnPrefab() {
        

        GameObject Instance = PoolManager.instance.GetPooledObject();
        
        if(PrefabPosition == 0)
        {
            Instance.transform.position = RestPositions[4].transform.position;
            Instance.GetComponent<CanvasGroup>().alpha = RestPositions[4].GetComponent<CanvasGroup>().alpha;
            ActiveMwsiveSongs.Insert(0, Instance);
        }
        else if (PrefabPosition <= 4) {
            Instance.transform.position = RestPositions[PrefabPosition-1].transform.position;
            Instance.GetComponent<CanvasGroup>().alpha = RestPositions[PrefabPosition-1].GetComponent<CanvasGroup>().alpha;
            ActiveMwsiveSongs.Add(Instance);

        } else {
            Instance.transform.position = RestPositions[3].transform.position;
            Instance.GetComponent<CanvasGroup>().alpha = 0;
            ActiveMwsiveSongs[0].GetComponent<SurfAni>().isAvailable = true;
            ActiveMwsiveSongs.RemoveAt(0);

            ActiveMwsiveSongs.Add(Instance);

        }
        
        Instance.transform.SetParent(MwsiveContainer.transform);
        Instance.transform.SetAsFirstSibling();
        Instance.SetActive(true);
        Instance.GetComponent<SurfAni>().isAvailable = false;

        //MwsiveSongs.Add(Instance);
        Instance.transform.eulerAngles = new Vector3(0, 0, 0);
        Instance.GetComponent<RectTransform>().offsetMin = new Vector2(LeftRightOffset.x, 0);
        Instance.GetComponent<RectTransform>().offsetMax = new Vector2(LeftRightOffset.y, 0);
        if (PrefabPosition == 0)
        {
            Instance.transform.localScale = new Vector3(1f, 1f, 1f);
            Instance.transform.position = RestPositions[4].transform.position;
        }
        else if (PrefabPosition <= 1) {
            Instance.transform.localScale = new Vector3(1f, 1f, 1f);
            Instance.transform.position = RestPositions[PrefabPosition-1].transform.position;
        } else if (PrefabPosition <= 2) {
            Instance.transform.localScale = new Vector3(.9f, .9f, .9f);
            Instance.transform.position = RestPositions[PrefabPosition-1].transform.position;
        } else if (PrefabPosition <= 3) {
            Instance.transform.localScale = new Vector3(.8f, .8f, .8f);
            Instance.transform.position = RestPositions[PrefabPosition-1].transform.position;
        } else if (PrefabPosition > 3) {
            Instance.transform.localScale = new Vector3(.6f, .6f, .6f);
            Instance.transform.position = RestPositions[3].transform.position;
        }

        Instance.GetComponent<ButtonSurfPlaylist>().SetSurfManager(gameObject);
        

        PrefabPosition++;
        return Instance;
    }

    float touchDuration;
    Touch touch;
    void Update() {
        if (Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null && canSwipe) { //if there is any touch
            touchDuration += Time.deltaTime;
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended && touchDuration < 0.2f) //making sure it only check the touch once && it was a short touch/tap and not a dragging.
                StartCoroutine("singleOrDouble");
        }
        else
            touchDuration = 0.0f;
    }

    IEnumerator singleOrDouble() {
        yield return new WaitForSeconds(0.2f);
        if (touch.tapCount == 1) {
            GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().OnClic_StopAudioPreview();
        }
        else if (touch.tapCount == 2) {
            //this coroutine has been called twice. We should stop the next one here otherwise we get two double tap
            StopCoroutine("singleOrDouble");

            GameObject Instance = Instantiate(MwsiveOla, Vector3.zero, Quaternion.identity);
            Instance.transform.SetParent(GameObject.Find("SpawnableCanvas_Canvas").transform);
            Instance.GetComponent<RectTransform>().offsetMin = new Vector2(100, 250);
            Instance.GetComponent<RectTransform>().offsetMax = new Vector2(-100, -250);

            UIAniManager.instance.DoubleClickOla(Instance);
            if (!OlaButton.GetComponent<MwsiveControllerButtons>().IsItOlaColorButtonActive()) {
                GetCurrentMwsiveData().isPicked = true;
                OlaButton.GetComponent<MwsiveControllerButtons>().OnClickOlaButton();

            }
        }
    }


    public void MainSceneProfile_OnClick() {
        GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
    }

    public void SurfProfileADN(string profileId = null, List<string> value = null) {

        
        if (value != null)
        {
            SpotifyConnectionManager.instance.GetSeveralTracks(value.ToArray(), OnCallBack_SpawnSeveralTracks);
            if (value.Count >= 5)
            {
                
                SpotifyConnectionManager.instance.GetUserPlaylists(profileId, Callback_GetUserPlaylists);
            }
            else
            {
                SpotifyConnectionManager.instance.GetUserPlaylists(profileId, Callback_OnClick_GetUserPlaylistsNotEnoughTracks);
            }           
        }


        if (UserPlaylists != null )
        {
            if (ProfilePlaylistPosition <= UserPlaylists.items.Count)
            {
                SpotifyConnectionManager.instance.GetPlaylistItems(UserPlaylists.items[ProfilePlaylistPosition].id, OnCallback_GetSpotifyPlaylist, "ES", 100, 0);
                
            }
        }

    }

    private void OnCallBack_SpawnSeveralTracks(object [] _value){

        SeveralTrackRoot severaltrack = (SeveralTrackRoot)_value[1];
        bool firstTimeSurf = false;
        if(severaltrack.tracks.Count >= 5)
        {
            firstTimeSurf = true;
            
        }
        DynamicPrefabSpawnerSeveralTracks(severaltrack.tracks, true, false, firstTimeSurf);
    }




    private void OnCallback_GetSpotifyPlaylist(object[] _value){
        ProfileItemsPlaylist = (PlaylistRoot)_value[1];        
        DynamicPrefabSpawnerPLItems(new object[] { ProfileItemsPlaylist }, false, false, UserPlaylists.items[ProfilePlaylistPosition].id, false);
        ProfilePlaylistPosition++;
        if (SpawnPosition + 4 >= MwsiveSongsData.Count)
        {
            SurfProfileADN();
        }
        else
        {
            if (!HasFirstPlaylistPlayed)
            {
                SurfManagerLogicInitialize();
                HasFirstPlaylistPlayed = true;
            }
            
        }


    }


    private void Callback_GetUserPlaylists(object[] _value)
    {

        UserPlaylists = (PlaylistRoot)_value[1];

    }


    private void Callback_OnClick_GetUserPlaylistsNotEnoughTracks(object[] _value){

        UserPlaylists = (PlaylistRoot)_value[1];
        SurfProfileADN();
        
    }
    

    public void StartTimer()
    {
        StartCoroutine("Timer");
    }

    public void StopTimer()
    {
        StopCoroutine("Timer");

    }
    public void KillTimer()
    {
        StopCoroutine("Timer");
        time = 0;
    }

    public float ResetTimer()
    {
        float time2 = time;
        StopCoroutine("Timer");
        time = 0;
        
        return time2;
    }

    IEnumerator Timer()
    {

        while (true)
        {
            yield return new WaitForSeconds(1);
            time++;
        }


    }

    private void TimerAppEventListener(TimerAppEvent _event)
    {
        if (!SurfController.instance.AmICurrentView(gameObject))
        {
            RemoveEventListener<TimerAppEvent>(TimerAppEventListener);
            return;
        }

        if (_event.type == "PAUSE")
        {
            StopTimer();
        }
        if (_event.type == "STOP")
        {
            StopTimer();
        }
        if (_event.type == "START")
        {
            StartTimer();
        }
        if (_event.type == "KILL")
        {
            KillTimer();
        }
    }








}
