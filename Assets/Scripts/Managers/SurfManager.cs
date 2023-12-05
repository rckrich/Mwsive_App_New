using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using GG.Infrastructure.Utils.Swipe;


public class SurfManager : Manager
{
    private static SurfManager _instance;

    public static SurfManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SurfManager>();
            }
            return _instance;
        }
    }

    public SwipeListener swipeListener;
    public ScrollRect Controller;
    public GameObject Prefab, MainCanvas, AddSong, OlaButton, MwsiveOla, MwsiveContainer, MwsiveControllerButtons, SurfShimmer;
    public List<MwsiveData> MwsiveSongsData = new List<MwsiveData>();
    public List<GameObject> ActiveMwsiveSongs = new List<GameObject>();
    public GameObject[] RestPositions;
    public GameObject surfviewmodel;
    public int SpawnPosition;
    public int CurrentPosition = 0;

    public float MaxRotation = 18f;
    public float SurfSuccessSensitivity = 2.2f;
    public Vector2 LeftRightOffset;
    public bool CanGetRecomendations = false;
    private SpotifyPlaylistRoot searchedPlaylist;
    private RecommendationsRoot recommendationsRoot;
    private AlbumRoot albumroot;
    private TrackInfoRoot trackInfoRoot;

    [HideInInspector]
    public bool canSwipe = true;
    public float time = 0;

    private Vector2 ControllerPostion = new Vector2();

    private int PrefabPosition = 0;
    private bool HasSwipeEnded = true;
    private bool Success = false;
    private bool ResetEndDrag = true;

    private bool WaitingForSongs = false;

    private void Start()
    {
        if (UIAniManager.instance.MainCanvas == null)
        {
            UIAniManager.instance.MainCanvas = MainCanvas;
        }
        ControllerPostion = new Vector2(Controller.transform.position.x, Controller.transform.position.y);

    }


    private void OnEnable()
    {

        SurfController.instance.AddToList(surfviewmodel, gameObject, true);


        GameObject currentPrefab = GetCurrentPrefab();

        if (currentPrefab != null && SurfController.instance.AmICurrentView(gameObject))
            currentPrefab.GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();

        if (SurfController.instance.AmICurrentView(gameObject) && this.enabled && NewScreenManager.instance.GetCurrentView().viewID == ViewID.SurfViewModel)
        {
            AddEventListener<TimerAppEvent>(TimerAppEventListener);
            if (MwsiveSongsData != null && MwsiveSongsData.Count > 0)
            {
                try
                {
                    SurfManagerLogicInitialize();
                }
                catch (MissingReferenceException)
                {

                }
                
            }
        }



    }

    private void OnDestroy()
    {
        if(gameObject != null)
        {
            SurfController.instance.DeleteFromList(gameObject, surfviewmodel);
        }
        
    }

    private void OnDisable()
    {
        if (this.enabled)
        {
            StopTimer();
            swipeListener.OnSwipe.RemoveListener(OnSwipe);
            try
            {
                if(MwsiveContainer != null)
                {
                    PoolManager.instance.RecoverPooledObject(MwsiveContainer);
                }
                else
                {
                    Debug.Log("Not able to return objects to poolmanager");
                }
               
            }
            catch (System.NullReferenceException)
            {
                Debug.Log("Not able to return objects to poolmanager");
            }


            RemoveEventListener<TimerAppEvent>(TimerAppEventListener);
            ActiveMwsiveSongs.Clear();
        }
    }



    private void OnSwipe(string swipe)
    {
        if (!canSwipe || !Controller.enabled)
        {
            return;
        }

        
        switch (swipe)
        {
            
            case "Right":
                if (Controller.horizontal)
                {
                    Controller.vertical = false;
                    Controller.horizontal = false;
                    HasSwipeEnded = false;
                    MwsiveControllerButtons.SetActive(false);
                    SideScrollSuccess();
                }


                break;
            case "Up":
                if (Controller.vertical)
                {
                    Controller.vertical = false;
                    Controller.horizontal = false;
                    HasSwipeEnded = false;
                    MwsiveControllerButtons.SetActive(false);
                    UpScrollSuccess();
                }

                break;
            case "Down":
                if (Controller.vertical)
                {
                    Controller.vertical = false;
                    Controller.horizontal = false;
                    HasSwipeEnded = false;
                    MwsiveControllerButtons.SetActive(false);
                    DownScrollSuccess();
                }

                break;
        }
    }

    public void ValChange()
    {
        
        if (Controller.transform.position.x > ControllerPostion.x * 1.10f)
        {
            Controller.vertical = false;
            MwsiveControllerButtons.SetActive(false);
            SideScrollAnimation();
        }
        if (Controller.transform.position.y > ControllerPostion.y * 1.10f)
        {
            Controller.horizontal = false;
            MwsiveControllerButtons.SetActive(false);
            UpScrollAnimation();
        }
        if (Controller.transform.position.y < ControllerPostion.y * .9f)
        {
            Controller.horizontal = false;
            MwsiveControllerButtons.SetActive(false);
            DownScrollAnimation();
        }


    }



    private void SideScrollAnimation()
    {

        float var = Controller.transform.position.x / ControllerPostion.x * .25f;
        float Fade = ControllerPostion.x / Controller.transform.position.x;

        ActiveMwsiveSongs[1].GetComponent<SurfAni>().SetValues(var, -MaxRotation, Fade, false);
        ActiveMwsiveSongs[1].GetComponent<SurfAni>().Play_SurfSide();

        AddSong.GetComponent<SurfAni>().SetValues(1, null, Fade);
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


    private void DownScrollAnimation()
    {
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


    private void UpScrollAnimation()
    {
        float var = Controller.transform.position.y / ControllerPostion.y;
        float Fade = Controller.transform.position.y / ControllerPostion.y;
        float VAR2 = ControllerPostion.y / Controller.transform.position.y;

        ActiveMwsiveSongs[1].GetComponent<SurfAni>().SetValues(Mathf.Clamp(var * .3f, 0, 1), MaxRotation, Fade, false);
        ActiveMwsiveSongs[1].GetComponent<SurfAni>().Play_VerticalUp();

        if (CurrentPosition < MwsiveSongsData.Count - 1)
        {

            ActiveMwsiveSongs[2].GetComponent<SurfAni>().SetValues(VAR2 * .25f, null, 1, null, null, RestPositions[0]);
            ActiveMwsiveSongs[2].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            ActiveMwsiveSongs[3].GetComponent<SurfAni>().SetValues(VAR2 * .25f, null, 1, null, null, RestPositions[1]);
            ActiveMwsiveSongs[3].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            ActiveMwsiveSongs[4].GetComponent<SurfAni>().SetValues(VAR2 * .25f, null, 1, null, null, RestPositions[2]);
            ActiveMwsiveSongs[4].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();
        }


        Success = false;
    }

    public void StartDrag()
    {
        ResetEndDrag = true;
    }

    public void OnEndDrag()
    {
        
        while (HasSwipeEnded && ResetEndDrag)
        {

            if (ActiveMwsiveSongs[1].transform.position.x >= ControllerPostion.x * SurfSuccessSensitivity)
            {
                if (CurrentPosition < MwsiveSongsData.Count - 1)
                {
                    HasSwipeEnded = false;
                    MwsiveControllerButtons.SetActive(false);
                    SideScrollSuccess();
                }

                break;

            }
            else if (ActiveMwsiveSongs[1].transform.position.y >= ControllerPostion.y * SurfSuccessSensitivity)
            {
                HasSwipeEnded = false;
                MwsiveControllerButtons.SetActive(false);
                UpScrollSuccess();
                break;

            }
            else if (ActiveMwsiveSongs[1].transform.position.y <= ControllerPostion.y / SurfSuccessSensitivity)
            {
                HasSwipeEnded = false;
                MwsiveControllerButtons.SetActive(false);
                DownScrollSuccess();
                break;

            }
            else
            {
                Success = false;
                ResetValue();
                

                break;
            }
        }
        


    }

    public void SideScrollSuccess(bool button = false)
    {
        
        ResetEndDrag = false;
        Controller.enabled = false;
        Controller.horizontal = true;
        Controller.vertical = true;
        Controller.transform.position = new Vector2(ControllerPostion.x, ControllerPostion.y);
        if (CurrentPosition < MwsiveSongsData.Count - 1 && !WaitingForSongs)
        {
            SpotifyPreviewAudioManager.instance.StopTrack();

            ActiveMwsiveSongs[1].GetComponent<SurfAni>().SetValues(1, -MaxRotation, 0, true);
            ActiveMwsiveSongs[1].GetComponent<SurfAni>().Play_SurfSide();

            //AddSongAnimations(button);

            ActiveMwsiveSongs[2].GetComponent<SurfAni>().SetValues(1, null, 1, null, null, RestPositions[0]);
            ActiveMwsiveSongs[2].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            ActiveMwsiveSongs[3].GetComponent<SurfAni>().SetValues(1, null, 1, null, null, RestPositions[1]);
            ActiveMwsiveSongs[3].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            ActiveMwsiveSongs[4].GetComponent<SurfAni>().SetValues(1, null, 1, null, null, RestPositions[2]);
            ActiveMwsiveSongs[4].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();



            GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().AddToPlaylistSwipe(GetCurrentMwsiveData().id, ResetTimer());

            CurrentPosition++;
            SpawnPosition++;



            SurfManagerLogic();
            Success = true;
        }
        else
        {
            ResetValue();
        }

        if (SpawnPosition >= MwsiveSongsData.Count - 8)
        {
            if (CanGetRecomendations)
            {
               SpawnRecommendations();
            }
        }

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            UIMessage.instance.UIMessageInstanciate("Error, no se ha agregado a playlist");
        }

        HasSwipeEnded = true;
        Controller.enabled = true;

    }
    private void DownScrollSuccess()
    {
        ResetEndDrag = false;
        Controller.enabled = false;
        Controller.horizontal = true;
        Controller.vertical = true;
        Controller.transform.position = new Vector2(ControllerPostion.x, ControllerPostion.y);
        if (CurrentPosition > 0)
        {
            ActiveMwsiveSongs[0].GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[CurrentPosition - 1]);
            SpotifyPreviewAudioManager.instance.StopTrack();
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

            if (AddSong.activeSelf)
            {
                AddSong.GetComponent<SurfAni>().Play_SurfAddsongReset();
            }


            string _trackid = GetCurrentMwsiveData().id;

            if (AppManager.instance.isLogInMode && !_trackid.Equals(""))
            {
                if (NewScreenManager.instance.TryGetComponent<PF_SurfManager>(out PF_SurfManager pF_SurfManager))
                {
                    int challenge_id = pF_SurfManager.Challenge ? pF_SurfManager.challenge_id : -1;
                    MwsiveConnectionManager.instance.PostTrackAction(_trackid, "DOWN", ResetTimer(), null, challenge_id);
                }
                else
                {
                    MwsiveConnectionManager.instance.PostTrackAction(_trackid, "DOWN", ResetTimer(), null, -1);
                }
            }
            CurrentPosition--;

            SpawnPosition--;


            ActiveMwsiveSongs[2].GetComponent<ButtonSurfPlaylist>().CheckIfDurationBarCanPlay();
            ActiveMwsiveSongs[3].GetComponent<ButtonSurfPlaylist>().CheckIfDurationBarCanPlay();
            SurfManagerLogicPreviousSong();

            if (WaitingForSongs)
            {
                SurfShimmer.SetActive(false);
                WaitingForSongs = false;
            }
        }
        else
        {
            ResetValue();
        }
        HasSwipeEnded = true;
        Controller.enabled = true;
    }

    private void UpScrollSuccess()
    {
        ResetEndDrag = false;
        Controller.enabled = false;
        Controller.horizontal = true;
        Controller.vertical = true;
        Controller.transform.position = new Vector2(ControllerPostion.x, ControllerPostion.y);
        if (CurrentPosition < MwsiveSongsData.Count - 1 && !WaitingForSongs)
        {
            SpotifyPreviewAudioManager.instance.StopTrack();
            Success = true;

            ActiveMwsiveSongs[1].GetComponent<SurfAni>().SetValues(1, MaxRotation, 0, true);
            ActiveMwsiveSongs[1].GetComponent<SurfAni>().Play_VerticalUp();

            

            ActiveMwsiveSongs[2].GetComponent<SurfAni>().SetValues(1, null, 1, null, null, RestPositions[0]);
            ActiveMwsiveSongs[2].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            ActiveMwsiveSongs[3].GetComponent<SurfAni>().SetValues(1, null, 1, null, null, RestPositions[1]);
            ActiveMwsiveSongs[3].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            ActiveMwsiveSongs[4].GetComponent<SurfAni>().SetValues(1, null, 1, null, null, RestPositions[2]);
            ActiveMwsiveSongs[4].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();


            string _trackid = GetCurrentMwsiveData().id;


            if (AppManager.instance.isLogInMode && !_trackid.Equals(""))
            {
                if (NewScreenManager.instance.TryGetComponent<PF_SurfManager>(out PF_SurfManager pF_SurfManager))
                {
                    int challenge_id = pF_SurfManager.Challenge ? pF_SurfManager.challenge_id : -1;
                    MwsiveConnectionManager.instance.PostTrackAction(GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().trackID, "UP", ResetTimer(), null, challenge_id);
                }
                else
                {
                    MwsiveConnectionManager.instance.PostTrackAction(GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().trackID, "UP", ResetTimer(), null, -1);
                }
            }

            if (AddSong.activeSelf)
            {
                AddSong.GetComponent<SurfAni>().Play_SurfAddsongReset();
            }

            CurrentPosition++;
            SpawnPosition++;
            SurfManagerLogic();
            
        }
        else
        {
            ResetValue();
        }
        if (SpawnPosition >= MwsiveSongsData.Count - 8)
        {
            if (CanGetRecomendations)
            {
                SpawnRecommendations();
            }
        }
        HasSwipeEnded = true;
        Controller.enabled = true;
    }

    public void AddSongAnimations(bool button = false)
    {
        DOTween.Kill(AddSong);
        if (button)
        {
            AddSong.GetComponent<CanvasGroup>().alpha = 1;
        }

        AddSong.GetComponent<SurfAni>().SetValues(1, null, 1);
        AddSong.GetComponent<SurfAni>().Play_CompleteAddSurfAddSong();
    }


    public void SpawnRecommendations()
    {
        if (CanGetRecomendations)
        {
            List<string> _artists = new List<string>();
            List<string> _tracks = new List<string>();

            if (recommendationsRoot == null)
            {
                for (int i = searchedPlaylist.tracks.items.Count - 1; i > searchedPlaylist.tracks.items.Count - 3; i--)
                {
                    _artists.Add(searchedPlaylist.tracks.items[i].track.artists[0].id);
                    _tracks.Add(searchedPlaylist.tracks.items[i].track.id);

                }
            }
            else
            {
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
        DynamicPrefabSpawnerRecommendations(new object[] { recommendationsRoot }, false, false);

    }

    public void ResetSideScroll()
    {


        DOTween.Complete(ActiveMwsiveSongs[2]);
        DOTween.Complete(ActiveMwsiveSongs[3]);
        DOTween.Complete(ActiveMwsiveSongs[4]);
        DOTween.Complete(AddSong);
        Reset();
        if (Success)
        {
            Success = false;
        }
    }
    public void ResetValue()
    {
        if (!Success)
        {
            ///DOTween.KillAll(false, new object[] { 0, 1 });
            ActiveMwsiveSongs[1].GetComponent<SurfAni>().SetValues(null,null,null,null,null,RestPositions[0]);
            ActiveMwsiveSongs[1].GetComponent<SurfAni>().Play_SurfReset();
            
            Reset();
        }
        else
        {
            Success = false;
        }

    }
    public void Reset()
    {
        Controller.horizontal = true;
        Controller.vertical = true;
        Controller.enabled = true;
        HasSwipeEnded = true;
        MwsiveControllerButtons.SetActive(true);
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


    public GameObject GetBeforeCurrentPrefab()
    {
        GameObject _Instance = ActiveMwsiveSongs[0];
        return _Instance;
    }

    public GameObject GetCurrentPrefab()
    {
        try
        {
            return ActiveMwsiveSongs[1];
        }
        catch (System.ArgumentOutOfRangeException)
        {
            return null;
        }

    }

    public bool IsManagerEmpty()
    {
        return MwsiveSongsData.Count == 0;
    }

    public MwsiveData GetBeforeCurrentMwsiveData()
    {
        return MwsiveSongsData[CurrentPosition - 1];
    }

    public MwsiveData GetCurrentMwsiveData()
    {
        return MwsiveSongsData[CurrentPosition];
    }

    public MwsiveData GetMwsiveData(int i)
    {
        return MwsiveSongsData[i];
    }

    public MwsiveData GetLastPrefab()
    {

        return MwsiveSongsData[MwsiveSongsData.Count - 1];


    }


    public void DynamicPrefabSpawnerRecommendations(object[] _value, bool FirsTimeSurf = true, bool? FirstTimeSurf = true)
    {
        recommendationsRoot = (RecommendationsRoot)_value[0];
        int SpawnedSongs = 0;
        foreach (var item in recommendationsRoot.tracks)
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

                    MwsiveSongsData.Add(instance);

                    SpawnedSongs++;
                }
            }

        }

        if (WaitingForSongs)
        {
            SurfShimmer.SetActive(false);
            WaitingForSongs = false;
        }


        if (FirsTimeSurf)
        {
            SurfManagerLogic(true);
        }


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
    public void DynamicPrefabSpawnerPLItems(object[] _value, bool PlayFirstTrack = true, bool CanSpawnBuffer = true, string _spotifyid = null)
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
    private void GetMwsiveInfo()
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
        GetCurrentMwsiveData().total_piks_followed = trackInfoRoot.followed_piks;

        try
        {
            GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().InitializeMwsiveDB(GetCurrentMwsiveData());
        }
        catch (System.NullReferenceException)
        {

        }

    }

    public void SurfManagerLogicInitialize()
    {
        swipeListener.OnSwipe.AddListener(OnSwipe);
        PrefabPosition = 0;
        Controller.gameObject.SetActive(true);
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
        if (MwsiveSongsData.Count - 1 > CurrentPosition + 1)
        {
            SpawnPrefab().GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[CurrentPosition + 1]);
            SpawnPosition++;
        }
        if (MwsiveSongsData.Count - 1 >= CurrentPosition + 2)
        {
            SpawnPrefab().GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[CurrentPosition + 2]);
            SpawnPosition++;
        }
        if (MwsiveSongsData.Count - 1 >= CurrentPosition + 3)
        {
            SpawnPrefab().GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[CurrentPosition + 3]);
            SpawnPosition++;
        }

    }



    private void SurfManagerLogicPreviousSong()
    {

        ActiveMwsiveSongs[ActiveMwsiveSongs.Count - 1].GetComponent<SurfAni>().isAvailable = true;
        ActiveMwsiveSongs.RemoveAt(ActiveMwsiveSongs.Count - 1);
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
            SpotifyPreviewAudioManager.instance.StopTrack();
            if (SpawnPosition < MwsiveSongsData.Count - 1)
            {

                instance.GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[SpawnPosition]);
                GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();

            }
            else
            {
                SurfShimmer.SetActive(true);
                WaitingForSongs = true;
                SpawnRecommendations();
            }
            

            
           
            
        }
        else
        {
            if (ActiveMwsiveSongs.Count == 0)
            {
                SurfManagerLogicInitialize();
            }



        }



    }

    private GameObject SpawnPrefabBack()
    {

        GameObject Instance = PoolManager.instance.GetPooledObject();


        Instance.GetComponent<CanvasGroup>().alpha = 0;
        Instance.transform.SetParent(MwsiveContainer.transform);
        Instance.transform.SetAsLastSibling();
        Instance.GetComponent<SurfAni>().isAvailable = false;
        ActiveMwsiveSongs.Insert(0, Instance);
        Instance.GetComponent<RectTransform>().offsetMin = new Vector2(LeftRightOffset.x, 0);
        Instance.GetComponent<RectTransform>().offsetMax = new Vector2(LeftRightOffset.y, 0);
        Instance.GetComponent<SurfAni>().ResetRestPosition();
        Instance.SetActive(true);
        Instance.transform.eulerAngles = new Vector3(0, 0, 0);
        return Instance;
    }


    public void SpawnSharePrefab(string SpotifyID)
    {
        SpotifyConnectionManager.instance.GetTrack(SpotifyID, Callback_SpawnSharePrefab);

    }

    private void Callback_SpawnSharePrefab(object[] _value)
    {
        
        TrackRoot trackRoot = (TrackRoot)_value[1];
        if(trackRoot != null)
        {
            if (trackRoot.preview_url != null)
            {
                MwsiveData instance = new MwsiveData();
                string artists = "";

                foreach (Artist artist in trackRoot.artists)
                {
                    artists = artists + artist.name + ", ";
                }

                artists = artists.Remove(artists.Length - 2);
                string currentPlayListName = AppManager.instance.isLogInMode ? AppManager.instance.GetCurrentPlaylist().name : "";

                instance.playlist_name = currentPlayListName;
                instance.song_name = trackRoot.name;
                instance.album_name = trackRoot.album.name;
                instance.artists = artists;
                instance.album_image_url = trackRoot.album.images[0].url;
                instance.id = trackRoot.id;
                instance.uri = trackRoot.uri;
                instance.preview_url = trackRoot.preview_url;
                instance.external_url = trackRoot.external_urls.spotify;

                MwsiveSongsData.Insert(CurrentPosition, instance);
                SpotifyPreviewAudioManager.instance.ForcePause();
                PoolManager.instance.RecoverPooledObject(MwsiveContainer);
                SurfManagerLogicInitialize();
                
            }
            else
            {
                UIMessage.instance.UIMessageInstanciate("Esta canción no esta disponible");
            }
        }
        else
        {
            UIMessage.instance.UIMessageInstanciate("Esta canción no esta disponible");
        }


    }


    private GameObject SpawnPrefab()
    {

        GameObject Instance = PoolManager.instance.GetPooledObject();
        Instance.GetComponent<ButtonSurfPlaylist>().ClearData();
        if (PrefabPosition == 0)
        {
            Instance.transform.position = RestPositions[4].transform.position;
            Instance.GetComponent<CanvasGroup>().alpha = RestPositions[4].GetComponent<CanvasGroup>().alpha;
            ActiveMwsiveSongs.Insert(0, Instance);
        }
        else if (PrefabPosition <= 4)
        {
            Instance.transform.position = RestPositions[PrefabPosition - 1].transform.position;
            Instance.GetComponent<CanvasGroup>().alpha = RestPositions[PrefabPosition - 1].GetComponent<CanvasGroup>().alpha;
            ActiveMwsiveSongs.Add(Instance);

        }
        else
        {
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
        Instance.transform.eulerAngles = new Vector3(0, 0, 0);
        //MwsiveSongs.Add(Instance);

        Instance.GetComponent<RectTransform>().offsetMin = new Vector2(LeftRightOffset.x, 0);
        Instance.GetComponent<RectTransform>().offsetMax = new Vector2(LeftRightOffset.y, 0);
        if (PrefabPosition == 0)
        {
            Instance.transform.localScale = new Vector3(1f, 1f, 1f);
            Instance.transform.position = RestPositions[4].transform.position;
        }
        else if (PrefabPosition <= 1)
        {
            Instance.transform.localScale = new Vector3(1f, 1f, 1f);
            Instance.transform.position = RestPositions[PrefabPosition - 1].transform.position;
        }
        else if (PrefabPosition <= 2)
        {
            Instance.transform.localScale = new Vector3(.9f, .9f, .9f);
            Instance.transform.position = RestPositions[PrefabPosition - 1].transform.position;
        }
        else if (PrefabPosition <= 3)
        {
            Instance.transform.localScale = new Vector3(.8f, .8f, .8f);
            Instance.transform.position = RestPositions[PrefabPosition - 1].transform.position;
        }
        else if (PrefabPosition > 3)
        {
            Instance.transform.localScale = new Vector3(.6f, .6f, .6f);
            Instance.transform.position = RestPositions[3].transform.position;
        }

        Instance.GetComponent<ButtonSurfPlaylist>().SetSurfManager(gameObject);

        PrefabPosition++;
        return Instance;
    }

    float touchDuration;
    Touch touch;
    void Update()
    {

        if (Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null && canSwipe)
        { //if there is any touch
            touchDuration += Time.deltaTime;
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended && touchDuration < 0.2f && Controller.isActiveAndEnabled) //making sure it only check the touch once && it was a short touch/tap and not a dragging.
                StartCoroutine("singleOrDouble");
        }
        else
            touchDuration = 0.0f;
    }

    IEnumerator singleOrDouble()
    {

        yield return new WaitForSeconds(0.2f);
        if (touch.tapCount == 1)
        {
            GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().OnClic_StopAudioPreview();
        }
        else if (touch.tapCount == 2)
        {
            //this coroutine has been called twice. We should stop the next one here otherwise we get two double tap
            StopCoroutine("singleOrDouble");

            if (!OlaButton.GetComponent<MwsiveControllerButtons>().IsItOlaColorButtonActive())
            {
                OlaButton.GetComponent<MwsiveControllerButtons>().OnClickOlaButton();

            }
            else
            {
                PIKAnimation();
            }
        }
    }

    public void PIKAnimation()
    {
        GameObject Instance = Instantiate(MwsiveOla, Vector3.zero, Quaternion.identity);
        Instance.transform.SetParent(GameObject.Find("SpawnableCanvas_Canvas").transform);
        Instance.GetComponent<RectTransform>().offsetMin = new Vector2(100, 250);
        Instance.GetComponent<RectTransform>().offsetMax = new Vector2(-100, -250);

        UIAniManager.instance.DoubleClickOla(Instance);
    }


    public void MainSceneProfile_OnClick()
    {
        GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
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
