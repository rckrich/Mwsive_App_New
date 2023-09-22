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
    

    public float MaxRotation = 18f;
    public float SurfSuccessSensitivity = 2.2f;
    public Vector2 LeftRightOffset;
    public bool CanGetRecomendations = false;
    private SpotifyPlaylistRoot searchedPlaylist, PagePlaylist;
    private PlaylistRoot ProfileItemsPlaylist, PageItemPlaylist;
    private RecommendationsRoot recommendationsRoot, PageRecommendationRoot;
    private PlaylistRoot UserPlaylists;
    private AlbumRoot albumroot, PageAlbumroot;
    private ChallengeAppObject challenge;

    private List<MwsiveData> MwsiveSongsData = new List<MwsiveData>();
    public List<GameObject> ActiveMwsiveSongs = new List<GameObject>();

    [HideInInspector]
    public bool canSwipe = true;
    public float time = 0;
    public bool HasSideScrollEnded = true;
    public bool Challenge = false;

    private Vector2 ControllerPostion = new Vector2();
    private int CurrentPosition = 0;
    private int PrefabPosition = 0;
    private string idPagingPlItems;
    private int ProfilePlaylistPosition = 0;
    private bool HasSwipeEnded = true;
    private bool Success = false;
    private bool SpawnedBuffer = false;
    private bool SurfProfile = false;
    private int SurfProfileOffsetPosition;
    private int trackstospawn = 0;
    private int trackstospawntotal = 0;
    private bool HasFirstPlaylistPlayed = false;
    
    private bool SurfPaged = false;
    private bool FirstSongplayed = false;


    [SerializeField]
    private int SongsMaxToPaginate;

    private void Start()
    {
        ControllerPostion = new Vector2(Controller.transform.position.x, Controller.transform.position.y);
    }


    private void OnEnable()
    {
        SurfController.instance.AddToList(gameObject);

        swipeListener.OnSwipe.AddListener(OnSwipe);

        GameObject currentPrefab = GetCurrentPrefab();

        if (currentPrefab != null && SurfController.instance.AmICurrentView(gameObject))
            currentPrefab.GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();

        if (SurfController.instance.AmICurrentView(gameObject))
        {
            AddEventListener<TimerAppEvent>(TimerAppEventListener);
            
        }
        



    }

    private void OnDestroy() {
        SurfController.instance.DeleteFromList(gameObject);
    }

    private void OnDisable()
    {
        StopTimer();
        swipeListener.OnSwipe.RemoveListener(OnSwipe);
        RemoveEventListener<TimerAppEvent>(TimerAppEventListener);

    }



    private void OnSwipe(string swipe)
    {
        if (!canSwipe)
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

        DOTween.Kill(MwsiveSongs[CurrentPosition]);
        DOTween.Kill(MwsiveSongs[CurrentPosition + 1]);
        DOTween.Kill(MwsiveSongs[CurrentPosition + 2]);
        DOTween.Kill(MwsiveSongs[CurrentPosition + 3]);
        float var = Controller.transform.position.x / ControllerPostion.x * .25f;
        float Fade = ControllerPostion.x / Controller.transform.position.x;

        UIAniManager.instance.SurfSide(MwsiveSongs[CurrentPosition], var, -MaxRotation, Fade, false);
        UIAniManager.instance.SurfAddSong(AddSong, var);

        if (CurrentPosition < MwsiveSongs.Count - 4) {
            UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition + 1], RestPositions[0], var);
            UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition + 2], RestPositions[1], var);
            UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition + 3], RestPositions[2], var);
        }

        Success = false;
    }


    private void DownScrollAnimation() {
        DOTween.Kill(MwsiveSongs[CurrentPosition]);
        DOTween.Kill(MwsiveSongs[CurrentPosition + 1]);
        DOTween.Kill(MwsiveSongs[CurrentPosition + 2]);
        DOTween.Kill(MwsiveSongs[CurrentPosition + 3]);
        float var = Controller.transform.position.y / ControllerPostion.y;
        float Fade = ControllerPostion.y / Controller.transform.position.y;

        UIAniManager.instance.SurfVerticalDown(MwsiveSongs[CurrentPosition], var * -.5f, MaxRotation, Fade, true);

        UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition + 1], RestPositions[1], var * .25f);
        UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition + 2], RestPositions[2], var * .25f);
        Success = false;


        Debug.Log("DownScrollAnimation");
    }


    private void UpScrollAnimation() {
        DOTween.Kill(MwsiveSongs[CurrentPosition]);
        DOTween.Kill(MwsiveSongs[CurrentPosition + 1]);
        DOTween.Kill(MwsiveSongs[CurrentPosition + 2]);
        DOTween.Kill(MwsiveSongs[CurrentPosition + 3]);

        float var = Controller.transform.position.y / ControllerPostion.y;
        float Fade = Controller.transform.position.y / ControllerPostion.y;
        float VAR2 = ControllerPostion.y / Controller.transform.position.y;

        UIAniManager.instance.SurfVerticalUp(MwsiveSongs[CurrentPosition], var * .5f, MaxRotation, Fade, false);


        if (CurrentPosition < MwsiveSongs.Count - 4) {
            UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition + 1], RestPositions[0], VAR2 * .25f);
            UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition + 2], RestPositions[1], VAR2 * .25f);
            UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition + 3], RestPositions[2], VAR2 * .25f);
        }


        Success = false;

        Debug.Log("UpScrollAnimation");
    }

    public void OnEndDrag() {
        while (HasSwipeEnded) {
            if (MwsiveSongs[CurrentPosition].transform.position.x >= ControllerPostion.x * SurfSuccessSensitivity) {
                if (CurrentPosition > MwsiveSongs.Count - 4) {
                    SideScrollSuccess();
                }

                break;

            } else if (MwsiveSongs[CurrentPosition].transform.position.y >= ControllerPostion.y * SurfSuccessSensitivity * 1.5) {
                UpScrollSuccess();
                break;

            } else if (MwsiveSongs[CurrentPosition].transform.position.y <= ControllerPostion.y / SurfSuccessSensitivity) {
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
        if (CurrentPosition < MwsiveSongs.Count - 4) {
            SpotifyPreviewAudioManager.instance.StopTrack();
            
            UIAniManager.instance.SurfSide(MwsiveSongs[CurrentPosition], 1, -MaxRotation, 0, true);

            UIAniManager.instance.CompleteSurfAddSong(AddSong, 1.5f);

            UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition + 1], RestPositions[0], 1);
            UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition + 2], RestPositions[1], 1);
            UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition + 3], RestPositions[2], 1);

            GetCurrentPrefab().GetComponentInChildren<ChallengeColorAnimation>().PauseTopMove();

            GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().AddToPlaylistSwipe(GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().trackID, ResetTimer());
            CurrentPosition++;

            if (Challenge)
            {
                GetCurrentPrefab().GetComponentInChildren<ChallengeColorAnimation>().ForceRestart();
            }



            GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
            Success = true;
        } else if (MwsiveSongs.Count - 4 == CurrentPosition && HasSideScrollEnded) {
            HasSideScrollEnded = false;
            UIAniManager.instance.SurfSideLastPosition(MwsiveSongs[CurrentPosition], RestPositions[0], 1, -MaxRotation, 0, gameObject);
            ResetSideScroll();
            UIAniManager.instance.SurfAddSongLastPosition(AddSong, 1.5f);
        } else {
            ResetValue();
        }





        if (CurrentPosition == PrefabPosition - 4) {
            if (SurfPaged)
            {
                SurfPaging();
            }
            else if (CanGetRecomendations)
            {
                SpawnRecommendations();
            } else if (SurfProfile) {
                SurfProfileADN();
            } else if (!CanGetRecomendations && !SpawnedBuffer && !SurfProfile) {
                SpawnPrefab();
                SpawnPrefab();
                SpawnPrefab();
                SpawnedBuffer = true;
            }

        }


        Debug.Log("SideScrollSucces");

        HasSwipeEnded = true;
    }
    private void DownScrollSuccess() {
        
        Controller.enabled = false;
        Controller.horizontal = true;
        Controller.vertical = true;
        Controller.transform.position = new Vector2(ControllerPostion.x, ControllerPostion.y);
        if (CurrentPosition > 0) {
            SpotifyPreviewAudioManager.instance.StopTrack();
            Success = true;
            UIAniManager.instance.SurfVerticalDown(MwsiveSongs[CurrentPosition], 1, -MaxRotation, 0, true);

            UIAniManager.instance.SurfTransitionBackSong(MwsiveSongs[CurrentPosition - 1], RestPositions[0], MaxRotation);
            UIAniManager.instance.SurfTransitionBackSongDown(MwsiveSongs[CurrentPosition], RestPositions[1]);
            UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition + 1], RestPositions[2], 1);
            UIAniManager.instance.SurfTransitionBackHideSong(MwsiveSongs[CurrentPosition + 2], RestPositions[3], 1);


            UIAniManager.instance.SurfAddSongReset(AddSong);



            GetCurrentPrefab().GetComponentInChildren<ChallengeColorAnimation>().PauseTopMove();

            string _trackid = GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().trackID;
            if (AppManager.instance.isLogInMode && !_trackid.Equals(""))
            {
                MwsiveConnectionManager.instance.PostTrackAction(_trackid, "DOWN", ResetTimer());
            }
            CurrentPosition--;
            if (Challenge)
            {
                GetCurrentPrefab().GetComponentInChildren<ChallengeColorAnimation>().ForceRestart();
            }
            GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
            MwsiveSongs[CurrentPosition + 1].GetComponent<ButtonSurfPlaylist>().CheckIfDurationBarCanPlay();
            MwsiveSongs[CurrentPosition + 2].GetComponent<ButtonSurfPlaylist>().CheckIfDurationBarCanPlay();

        }
        else {
            ResetValue();
        }


        Debug.Log("DownScrollSucess");
        HasSwipeEnded = true;
    }
    private void UpScrollSuccess() {
        
        Controller.enabled = false;
        Controller.horizontal = true;
        Controller.vertical = true;
        Controller.transform.position = new Vector2(ControllerPostion.x, ControllerPostion.y);
        if (CurrentPosition < MwsiveSongs.Count - 4) {
            SpotifyPreviewAudioManager.instance.StopTrack();
            Success = true;

            UIAniManager.instance.SurfVerticalUp(MwsiveSongs[CurrentPosition], 1, MaxRotation, 0, true);


            UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition + 1], RestPositions[0], 1);
            UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition + 2], RestPositions[1], 1);

            UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition + 3], RestPositions[2], 1);

            
            string _trackid = GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().trackID;
            if (AppManager.instance.isLogInMode && !_trackid.Equals(""))
            {
                MwsiveConnectionManager.instance.PostTrackAction(GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().trackID, "UP", ResetTimer());
            }

            GetCurrentPrefab().GetComponentInChildren<ChallengeColorAnimation>().PauseTopMove();
            CurrentPosition++;
            if (Challenge)
            {
                GetCurrentPrefab().GetComponentInChildren<ChallengeColorAnimation>().ForceRestart();
            }

            GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
            UIAniManager.instance.SurfAddSongReset(AddSong);
        } else {
            ResetValue();
        }

        
        if (CurrentPosition == PrefabPosition - 4) {
            
            if (SurfPaged)
            {
                SurfPaging();
               

            }else if (CanGetRecomendations) {
                SpawnRecommendations();
            } else if (SurfProfile) {
                SurfProfileADN();
            } else if (!CanGetRecomendations && !SpawnedBuffer && !SurfProfile) {
                SpawnPrefab();
                SpawnPrefab();
                SpawnPrefab();
                SpawnedBuffer = true;
            }

        }

        Debug.Log("UpScrollSuccess");
        HasSwipeEnded = true;
    }


    public void SpawnRecommendations() {
        if (CanGetRecomendations) {
            List<string> _artists = new List<string>();
            List<string> _tracks = new List<string>();

            if (recommendationsRoot == null) {
                for (int i = searchedPlaylist.tracks.items.Count - 1; i > searchedPlaylist.tracks.items.Count - 3; i--)
                {
                    _artists.Add(searchedPlaylist.tracks.items[i].track.artists[0].id);
                    _tracks.Add(searchedPlaylist.tracks.items[i].track.id);

                }
            } else {
                for (int i = recommendationsRoot.tracks.Count - 1; i > recommendationsRoot.tracks.Count - 3; i--)
                {
                    _artists.Add(recommendationsRoot.tracks[i].artists[0].id);
                    _tracks.Add(recommendationsRoot.tracks[i].id);

                }
            }


            SpotifyConnectionManager.instance.GetRecommendations(_artists.ToArray(), _tracks.ToArray(), Callback_SpawnRecommendations, 20);
        }


    }
    private void Callback_SpawnRecommendations(object[] _value)
    {
        recommendationsRoot = (RecommendationsRoot)_value[1];
        DynamicPrefabSpawnerRecommendations(new object[] { recommendationsRoot });

    }

    public void ResetSideScroll() {


        DOTween.Complete(MwsiveSongs[CurrentPosition + 1]);
        DOTween.Complete(MwsiveSongs[CurrentPosition + 2]);
        DOTween.Complete(MwsiveSongs[CurrentPosition + 3]);
        DOTween.Complete(AddSong);
        Reset();
        if (Success) {
            Success = false;
        }
    }
    public void ResetValue() {

        if (!Success) {
            DOTween.KillAll(false, new object[] { 0, 1 });
            UIAniManager.instance.SurfReset(MwsiveSongs[CurrentPosition]);
            Reset();
        } else {
            Success = false;
        }

    }
    public void Reset() {
        Debug.Log("Reset");

        Controller.horizontal = true;
        Controller.vertical = true;
        HasSwipeEnded = true;

        Controller.transform.position = new Vector2(ControllerPostion.x, ControllerPostion.y);

        
        UIAniManager.instance.SurfAddSongReset(AddSong);

        UIAniManager.instance.SurfResetOtherSongs(MwsiveSongs[CurrentPosition + 1], RestPositions[1], true);
        UIAniManager.instance.SurfResetOtherSongs(MwsiveSongs[CurrentPosition + 2], RestPositions[2], true);
        UIAniManager.instance.SurfResetOtherSongs(MwsiveSongs[CurrentPosition + 3], RestPositions[3], false);
    }





    public GameObject GetBeforeCurrentPrefab() {
        GameObject _Instance = MwsiveSongs[CurrentPosition - 1];
        return _Instance;
    }

    public GameObject GetCurrentPrefab() {
        try
        {
            return ActiveMwsiveSongs[0];
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
    public MwsiveData GetLastPrefab() {

        return MwsiveSongsData[MwsiveSongsData.Count - 1];


    }

    public List<GameObject> GetInstances() {
        return MwsiveSongs;
    }
    public void SetChallengeCallback(ChallengeAppObject _challenge) {
        challenge = _challenge;
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

                        MwsiveData instance = null;
                        instance.playlist_name = AppManager.instance.GetCurrentPlaylist().name;
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

                        MwsiveData instance = null;

                        string artists = "";

                        foreach (Artist artist in item.artists)
                        {
                            artists = artists + artist.name + ", ";
                        }

                        artists = artists.Remove(artists.Length - 2);

                        instance.playlist_name = AppManager.instance.GetCurrentPlaylist().name;
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

                        string currentPlaylistName = AppManager.instance.isLogInMode ? AppManager.instance.GetCurrentPlaylist().name : "";

                        MwsiveData instance = null;
                        instance.playlist_name = AppManager.instance.GetCurrentPlaylist().name;
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
    public void DynamicPrefabSpawnerPLItems(object[] _value, bool PlayFirstTrack = true, bool CanSpawnBuffer = true, string _spotifyid = null)
    {
        PageItemPlaylist = (PlaylistRoot)_value[0];
           
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

                        MwsiveData instance = null;

                        instance.playlist_name = AppManager.instance.GetCurrentPlaylist().name;
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
                       
                    MwsiveData instance = null;
                    instance.playlist_name = AppManager.instance.GetCurrentPlaylist().name;
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

    public void DynamicPrefabSpawnerSeveralTracks(List<Track> tracks, bool PlayFirstTrack = true, bool CanSpawnBuffer = true)
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

                    MwsiveData instance = new MwsiveData();
                    instance.playlist_name = AppManager.instance.GetCurrentPlaylist().name;
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
        SurfManagerLogic(true);

        /*
        if (challenge)
        {
            GetLastPrefab().GetComponent<ButtonSurfPlaylist>().SetCallbackLastPosition(challenge);
        }
        

        
        */
    }

    private void SurfManagerLogic(bool _firstTime = false)
    {

        if (_firstTime)
        {
            if(MwsiveSongsData[CurrentPosition] != null)
            {
                GameObject instance = SpawnPrefab();
                instance.GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[CurrentPosition]);
                instance.GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
            }
            if (MwsiveSongsData.Count > CurrentPosition +1)
            {
                SpawnPrefab().GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[CurrentPosition + 1]);
            }
            if (MwsiveSongsData.Count >= CurrentPosition + 2)
            {
                SpawnPrefab().GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[CurrentPosition + 2]);
            }
            if (MwsiveSongsData.Count >= CurrentPosition + 3)
            {
                SpawnPrefab().GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[CurrentPosition + 3]);
            }
            if (MwsiveSongsData.Count < 4 && MwsiveSongsData.Count > 0)
            {
                SpawnBuffer();
            }
            else if(MwsiveSongsData.Count == 0)
            {
                UIMessage.instance.UIMessageInstanciate("Vuelve a Intentarlo");
            }

        }
        else if (CurrentPosition + 3 < MwsiveSongsData.Count)
        {
            SpawnPrefab().GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[CurrentPosition + 3]);
        }
        else if(CurrentPosition + 3 == MwsiveSongsData.Count)
        {
            MwsiveSongsData[CurrentPosition + 3].challenge_AmILastPosition = true;
            SpawnPrefab().GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[CurrentPosition + 3]);
            
        }
        else if(SpawnedBuffer)
        {
            SpawnBuffer();

        }

    }

    private void SpawnBuffer()
    {
        SpawnPrefab();
        SpawnPrefab();
        SpawnPrefab();
        SpawnedBuffer = true;
    }

    private GameObject SpawnBackBuffer()
    {
        GameObject Instance = PoolManager.instance.GetPooledObject();
        //Something something, spawnearlo en la posición de atrás
        return Instance;
    }

    private GameObject SpawnPrefab() {
        

        GameObject Instance = PoolManager.instance.GetPooledObject();
        
        if (PrefabPosition < 4) {
            Instance.transform.position = RestPositions[PrefabPosition].transform.position;
            Instance.GetComponent<CanvasGroup>().alpha = RestPositions[PrefabPosition].GetComponent<CanvasGroup>().alpha;
            ActiveMwsiveSongs.Add(Instance);

        } else {
            Instance.transform.position = RestPositions[3].transform.position;
            Instance.GetComponent<CanvasGroup>().alpha = 0;
            ActiveMwsiveSongs.RemoveAt(0);
            ActiveMwsiveSongs.Add(Instance);

        }
        
        Instance.transform.SetParent(MwsiveContainer.transform);
        Instance.transform.SetAsFirstSibling();
        Instance.SetActive(true);

        //MwsiveSongs.Add(Instance);

        Instance.GetComponent<RectTransform>().offsetMin = new Vector2(LeftRightOffset.x, 0);
        Instance.GetComponent<RectTransform>().offsetMax = new Vector2(LeftRightOffset.y, 0);

        if (PrefabPosition < 1) {
            Instance.transform.localScale = new Vector3(1f, 1f, 1f);
            Instance.transform.position = RestPositions[PrefabPosition].transform.position;
        } else if (PrefabPosition < 2) {
            Instance.transform.localScale = new Vector3(.9f, .9f, .9f);
            Instance.transform.position = RestPositions[PrefabPosition].transform.position;
        } else if (PrefabPosition < 3) {
            Instance.transform.localScale = new Vector3(.8f, .8f, .8f);
            Instance.transform.position = RestPositions[PrefabPosition].transform.position;
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
            Instance.transform.SetParent(GameObject.Find("SpawnableCanvas").transform);
            Instance.GetComponent<RectTransform>().offsetMin = new Vector2(100, 250);
            Instance.GetComponent<RectTransform>().offsetMax = new Vector2(-100, -250);

            UIAniManager.instance.DoubleClickOla(Instance);
            if (!OlaButton.GetComponent<MwsiveControllerButtons>().IsItOlaColorButtonActive()) {
                OlaButton.GetComponent<MwsiveControllerButtons>().OnClickOlaButton();

            }
        }
    }


    public void MainSceneProfile_OnClick() {
        GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
    }

    public void SurfPaging()
    {
        
        if(PageRecommendationRoot != null)
        {
            SurfPagingRecomendations();
        }
        else if (PageAlbumroot != null)
        {
            SurfPagingAlbum();
        }else if (PagePlaylist != null)
        {
            SurfPagingPL();
        }else if (PageItemPlaylist!= null)
        {
            SurfPagingPLitems();
        }
    }

    public void SurfPagingRecomendations(RecommendationsRoot _value = null)
    {

        if (_value != null)
        {
            PageRecommendationRoot = _value;
            SurfPaged = true;
        }
            if (trackstospawn + SongsMaxToPaginate < PageRecommendationRoot.tracks.Count)
            {
                trackstospawn += SongsMaxToPaginate;
            }
            else
            {
                trackstospawn = PageRecommendationRoot.tracks.Count;
                
            }
        
            
            List<Track> ListOfTracksToSpawn = new List<Track>();

            for (int i = SurfProfileOffsetPosition; i < trackstospawn; i++)
            {
                ListOfTracksToSpawn.Add(PageRecommendationRoot.tracks[i]);
            }
            if (trackstospawn == PageRecommendationRoot.tracks.Count)
            {
                DynamicPrefabSpawnerSeveralTracks(ListOfTracksToSpawn, true, true);
                trackstospawn = 0;
                SurfPaged = false;

            }
            else
            {
                DynamicPrefabSpawnerSeveralTracks(ListOfTracksToSpawn, true, false);
            }

        SurfProfileOffsetPosition = trackstospawn;

    }

    public void SurfPagingAlbum(AlbumRoot _value = null)
    {

        if (_value != null)
        {
            PageAlbumroot = _value;
            SurfPaged = true;
        }
        
            if (trackstospawn + SongsMaxToPaginate < PageAlbumroot.tracks.items.Count)
            {
                trackstospawn += SongsMaxToPaginate;
            }
            else
            {
                trackstospawn = PageAlbumroot.tracks.items.Count;
                
            }

            List<string> ListOfTracksToSpawn = new List<string>();

            for (int i = SurfProfileOffsetPosition; i < trackstospawn; i++)
            {
                ListOfTracksToSpawn.Add(PageAlbumroot.tracks.items[i].id);
            }

            SpotifyConnectionManager.instance.GetSeveralTracks(ListOfTracksToSpawn.ToArray(), OnCallback_SurfPagingAlbum);


        SurfProfileOffsetPosition = trackstospawn;

    }
    public void OnCallback_SurfPagingAlbum(object[] _value)
    {
        SeveralTrackRoot tracks = (SeveralTrackRoot)_value[1];
        if (trackstospawn == PageAlbumroot.tracks.items.Count)
        {
            DynamicPrefabSpawnerSeveralTracks(tracks.tracks, true, true);
            trackstospawn = 0;
            SurfPaged = false;
        }
        else
        {
            DynamicPrefabSpawnerSeveralTracks(tracks.tracks, true, false);
        }
    }

    public void SurfPagingPL(SpotifyPlaylistRoot _value = null)
    {
       
        if (_value != null) {
            PagePlaylist = _value;
            SurfPaged = true;
        }

        
        if (trackstospawn + SongsMaxToPaginate < PagePlaylist.tracks.items.Count)
        {
                trackstospawn += SongsMaxToPaginate;
        }
            else
            {
                trackstospawn = PagePlaylist.tracks.items.Count;
                
            }


        
        List<Track> ListOfTracksToSpawn = new List<Track>();

            for (int i = SurfProfileOffsetPosition; i < trackstospawn; i++)
            {
                ListOfTracksToSpawn.Add(PagePlaylist.tracks.items[i].track);
            }
        
            if(trackstospawn == PagePlaylist.tracks.items.Count)
            {
                DynamicPrefabSpawnerSeveralTracks(ListOfTracksToSpawn, true, true);
                trackstospawn = 0;
                SurfPaged = false;
        }
            else
            {
                DynamicPrefabSpawnerSeveralTracks(ListOfTracksToSpawn, true, false);
            }

        SurfProfileOffsetPosition = trackstospawn;


    }

    public void SurfPagingPLitems(PlaylistRoot _value = null, string _id = null)
    {
        
        

        if (_value != null && _id != null)
        {
            PageItemPlaylist = _value;
            SurfPaged = true;
            idPagingPlItems = _id;
        }

        if (trackstospawn >= PageItemPlaylist.total)
        {
            SurfPaged = false;
            return;
        }

        trackstospawn += SongsMaxToPaginate;

        if (trackstospawn > PageItemPlaylist.items.Count)
        {
            trackstospawn = PageItemPlaylist.items.Count;
            
        }



        

        List<Track> ListOfTracksToSpawn = new List<Track>();


        for (int i = SurfProfileOffsetPosition; i < trackstospawn; i++)
        {
            ListOfTracksToSpawn.Add(PageItemPlaylist.items[i].track);
        }

        if (trackstospawn == PageItemPlaylist.items.Count)
        {
            DynamicPrefabSpawnerSeveralTracks(ListOfTracksToSpawn, true, true);
            trackstospawn = 0;
            trackstospawntotal += PageItemPlaylist.items.Count;
            SpotifyConnectionManager.instance.GetPlaylistItems(idPagingPlItems, OnCallback_SurfPagingPLitems, "ES", 100, trackstospawntotal);
        }
        else
        {
            DynamicPrefabSpawnerSeveralTracks(ListOfTracksToSpawn, true, false);
        }

        SurfProfileOffsetPosition = trackstospawn;


    }

    private void OnCallback_SurfPagingPLitems(object[] _value)
    {
        PageItemPlaylist = (PlaylistRoot)_value[1];
    }

    public void SurfProfileADN( string profileId = null, List<string> value = null){

        
        if (value != null)
        {
            if (value.Count > 4)
            {
                SpotifyConnectionManager.instance.GetSeveralTracks(value.ToArray(), OnCallBack_SpawnSeveralTracks);
            }
            else
            {
                
                SpotifyConnectionManager.instance.GetSeveralTracks(value.ToArray(), OnCallback_SpawnSeveralTracksNotEnoughTracks);

            }

        }
        if (profileId != null){
           
            if(value != null){
                if (value.Count > 4)
                {
                    SpotifyConnectionManager.instance.GetUserPlaylists(profileId, Callback_OnClick_GetUserPlaylists);
                }
                else
                {
                    SpotifyConnectionManager.instance.GetUserPlaylists(profileId, Callback_OnClick_GetUserPlaylistsNoADN);
                }
                
            }else{
                SpotifyConnectionManager.instance.GetUserPlaylists(profileId, Callback_OnClick_GetUserPlaylistsNoADN);
            }
            
            
        }

        if(SurfProfile){



            
            if(ProfilePlaylistPosition <= UserPlaylists.items.Count){

                //Checar cuantos items tienes si es mayor de 50 Iniciar Paginacion sino solo spawnear
                if(UserPlaylists.items[ProfilePlaylistPosition].tracks.total == 0) {
                    ProfilePlaylistPosition++;
                    SpotifyConnectionManager.instance.GetPlaylistItems(UserPlaylists.items[ProfilePlaylistPosition].id, OnCallback_GetSpotifyPlaylistNotEnoughTracks, "ES", 100, 0);
                    return;
                }

                if (ProfileItemsPlaylist.items.Count < SongsMaxToPaginate) {
                    if (ProfileItemsPlaylist.items.Count > 4) {
                        if (!HasFirstPlaylistPlayed) {
                            HasFirstPlaylistPlayed = true;
                            DynamicPrefabSpawnerPLItems(new object[] { ProfileItemsPlaylist }, true, false);
                        } else {
                            DynamicPrefabSpawnerPLItems(new object[] { ProfileItemsPlaylist }, false, false);
                        }

                        ProfilePlaylistPosition++;
                        SpotifyConnectionManager.instance.GetPlaylistItems(UserPlaylists.items[ProfilePlaylistPosition].id, OnCallback_GetSpotifyPlaylist, "ES", 100, 0);
                    }
                    else {
                        if (ProfileItemsPlaylist.items.Count != 0)
                        {
                            if (!HasFirstPlaylistPlayed) {
                                HasFirstPlaylistPlayed = true;
                                DynamicPrefabSpawnerPLItems(new object[] { ProfileItemsPlaylist }, true, false);
                            } else {
                                DynamicPrefabSpawnerPLItems(new object[] { ProfileItemsPlaylist }, false, false);
                            }
                        }
                        ProfilePlaylistPosition++;

                        if (CurrentPosition >= MwsiveSongs.Count - 4)
                        {
                            SpotifyConnectionManager.instance.GetPlaylistItems(UserPlaylists.items[ProfilePlaylistPosition].id, OnCallback_GetSpotifyPlaylistNotEnoughTracks, "ES", 100, 0);
                        }
                        else
                        {
                            SpotifyConnectionManager.instance.GetPlaylistItems(UserPlaylists.items[ProfilePlaylistPosition].id, OnCallback_GetSpotifyPlaylist, "ES", 100, 0);
                        }


                    }


                } else {

                    bool NextBatch = false;

                    Debug.LogWarning("Trackstospawn: " + trackstospawn + " spawntotal: " + trackstospawntotal + " PlaylistCount " + ProfileItemsPlaylist.items.Count + "Position" + ProfilePlaylistPosition);

                    trackstospawn += SongsMaxToPaginate;
                    if (trackstospawn  < ProfileItemsPlaylist.items.Count)
                    {

                    } else {

                        trackstospawntotal += ProfileItemsPlaylist.items.Count;

                        if (trackstospawntotal >= UserPlaylists.items[ProfilePlaylistPosition].tracks.total)
                        {
                            ProfilePlaylistPosition++;
                            trackstospawntotal = 0;

                        }
                        NextBatch = true;
                        


                    }
                    if (trackstospawn > ProfileItemsPlaylist.items.Count)
                    {
                        trackstospawn = ProfileItemsPlaylist.items.Count;
                    }

                    List<Track> ListOfTracksToSpawn = new List<Track>();
                    for (int i = SurfProfileOffsetPosition; i < trackstospawn; i++)
                    {
                        ListOfTracksToSpawn.Add(ProfileItemsPlaylist.items[i].track);
                    }

                    if (!HasFirstPlaylistPlayed)
                    {
                        HasFirstPlaylistPlayed = true;
                        DynamicPrefabSpawnerSeveralTracks(ListOfTracksToSpawn, true, false);
                    }
                    else
                    {
                        DynamicPrefabSpawnerSeveralTracks(ListOfTracksToSpawn, false, false);
                    }

                    if (NextBatch)
                    {
                        SpotifyConnectionManager.instance.GetPlaylistItems(UserPlaylists.items[ProfilePlaylistPosition].id, OnCallback_GetSpotifyPlaylist, "ES", 100, trackstospawntotal);
                        trackstospawn = 0;
                    }
                    

                   


                    SurfProfileOffsetPosition = trackstospawn;
                    
                    
                    
                }

            }
            else if(!SpawnedBuffer)
            {
                SpawnPrefab();
                SpawnPrefab();
                SpawnPrefab();
                SpawnedBuffer = true;
            }
        }
        
        


    }

    private void OnCallBack_SpawnSeveralTracks(object [] _value){

        SeveralTrackRoot severaltrack = (SeveralTrackRoot)_value[1];
        DynamicPrefabSpawnerSeveralTracks(severaltrack.tracks, true, false);
        HasFirstPlaylistPlayed = true;
    }
    private void OnCallback_SpawnSeveralTracksNotEnoughTracks(object[] _value)
    {
        SeveralTrackRoot severaltrack = (SeveralTrackRoot)_value[1];
        DynamicPrefabSpawnerSeveralTracks(severaltrack.tracks, true, false);
        HasFirstPlaylistPlayed = true;
        
    }



    private void OnCallback_GetSpotifyPlaylist(object[] _value){
        ProfileItemsPlaylist = (PlaylistRoot)_value[1];
        SurfProfile = true;
    }

    

    private void OnCallback_GetSpotifyPlaylistNotEnoughTracks(object[] _value)
    {
        ProfileItemsPlaylist = (PlaylistRoot)_value[1];
        SurfProfile = true;
        SurfProfileADN();

    }

    private void OnCallback_GetSpotifyPlaylist_Next(object[] _value)
    {
        ProfileItemsPlaylist = (PlaylistRoot)_value[1];
        UpScrollSuccess();

    }


    private void Callback_OnClick_GetUserPlaylists(object[] _value){

        UserPlaylists = (PlaylistRoot)_value[1];
        
        SpotifyConnectionManager.instance.GetPlaylistItems(UserPlaylists.items[ProfilePlaylistPosition].id, OnCallback_GetSpotifyPlaylist, "ES", 100, 0);

    }

    private void Callback_OnClick_GetUserPlaylistsNoADN(object[] _value){

        UserPlaylists = (PlaylistRoot)_value[1];
        
        SpotifyConnectionManager.instance.GetPlaylistItems(UserPlaylists.items[ProfilePlaylistPosition].id, OnCallback_GetSpotifyPlaylistNotEnoughTracks, "ES", 100, 0);
        
    }

    public void UpdateDisk(){
        
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
        Debug.Log(_event.type.ToString());
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
