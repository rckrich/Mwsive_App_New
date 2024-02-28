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
    public GameObject Prefab, AddSong, OlaButton, MwsiveOla, MwsiveContainer, MwsiveControllerButtons, MessageADNPopUp;
    public GameObject[] RestPositions;
    public GameObject loadingCard;
    public GameObject surfviewmodel;
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

    private int InternalTime = 0;
    public float TimeToWaitMwsiveDB = 1;
    private IEnumerator WaitMwsiveDbCo, WaitForMessageADNCo;
    public Sprite ADNWarning;

    [HideInInspector]
    public bool canSwipe = true;
    public float time = 0;
    public bool HasSideScrollEnded = true;
    public bool Challenge = false;
    public int challenge_id = -1;

    private Vector2 ControllerPostion = new Vector2();

    private int PrefabPosition = 0;
    private string idPagingPlItems;
    private int offsetPagingPlItems;
    private int totalPagingPlItems;
    private int ProfilePlaylistPosition = 0;
    private bool HasSwipeEnded = true;
    private bool Success = false;
    private bool ResetEndDrag = true;
    private bool lastSongMessage = false;

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
        SurfController.instance.AddToList(surfviewmodel, gameObject);





        GameObject currentPrefab = GetCurrentPrefab();

        if (currentPrefab != null && SurfController.instance.AmICurrentView(gameObject))
            currentPrefab.GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();

        if (SurfController.instance.AmICurrentView(gameObject))
        {
            AddEventListener<TimerAppEvent>(TimerAppEventListener);
            if (MwsiveSongsData != null && MwsiveSongsData.Count > 0)
            {
                HasFirstPlaylistPlayed = false;

                SurfManagerLogicInitialize();
            }
        }






    }

    private void OnDestroy()
    {

        try
        {
            SurfController.instance.DeleteFromList(gameObject, surfviewmodel);
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("Can not delete from SurfController");
        }

    }

    private void OnDisable()
    {
        try
        {
            PoolManager.instance.RecoverPooledObject(MwsiveContainer);
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("Can not return objects to pool");
        }
        
        StopTimer();
        swipeListener.OnSwipe.RemoveListener(OnSwipe);
        RemoveEventListener<TimerAppEvent>(TimerAppEventListener);
        ActiveMwsiveSongs.Clear();


    }



    private void OnSwipe(string swipe)
    {
        if (!canSwipe || !HasSwipeEnded || !Controller.enabled)
        {
            return;
        }
            
        MwsiveControllerButtons.SetActive(false);
        switch (swipe)
        {
            case "Right":
                if (Controller.horizontal)
                {
                    Controller.vertical = false;
                    Controller.horizontal = false;
                    HasSwipeEnded = false;
                    SideScrollSuccess();
                }


                break;
            case "Up":
                if (Controller.vertical)
                {
                    Controller.vertical = false;
                    Controller.horizontal = false;
                    HasSwipeEnded = false;
                    UpScrollSuccess();

                }
                break;
            case "Down":
                if (Controller.vertical)
                {
                    Controller.vertical = false;
                    Controller.horizontal = false;
                    HasSwipeEnded = false;
                    DownScrollSuccess();
                }

                break;
        }
        //Debug.Log(swipe);
    }

    public void ValChange()
    {
        
        if (Controller.transform.position.x > ControllerPostion.x * 1.20f)
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

        while (HasSwipeEnded && HasSideScrollEnded && ResetEndDrag)
        {
            if (ActiveMwsiveSongs[1].transform.position.x >= ControllerPostion.x * SurfSuccessSensitivity)
            {
                HasSwipeEnded = false;
                SideScrollSuccess();
                MwsiveControllerButtons.SetActive(false);
                break;

            }
            else if (ActiveMwsiveSongs[1].transform.position.y >= ControllerPostion.y * SurfSuccessSensitivity)
            {
                HasSwipeEnded = false;
                UpScrollSuccess();
                MwsiveControllerButtons.SetActive(false);
                break;

            }
            else if (ActiveMwsiveSongs[1].transform.position.y <= ControllerPostion.y / SurfSuccessSensitivity)
            {
                HasSwipeEnded = false;
                DownScrollSuccess();
                MwsiveControllerButtons.SetActive(false);
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

    public void SideScrollSuccess(bool button =false)
    {
        ResetEndDrag = false;
        Controller.enabled = false;
        Controller.horizontal = true;
        Controller.vertical = true;
        Controller.transform.position = new Vector2(ControllerPostion.x, ControllerPostion.y);
        if (CurrentPosition < MwsiveSongsData.Count - 1)
        {

            if (Challenge)
            {
                GetCurrentPrefab().GetComponentInChildren<_ChallengeColorAnimation>().ForceClear();
                GetCurrentPrefab().GetComponentInChildren<_ChallengeColorAnimation>().isCompleted = false;
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




            AddSongAnimation(button);
            GetCurrentPrefab().GetComponent<MwsiveButton>().PIKCallbackEnd = true;
            GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().AddToPlaylistSwipe(GetCurrentMwsiveData().id, ResetTimer());
            CurrentPosition++;
            SpawnPosition++;





            SurfManagerLogic();
            Success = true;
        }
        else if (MwsiveSongsData.Count - 1 == CurrentPosition && HasSideScrollEnded)
        {
            HasSideScrollEnded = false;

            ActiveMwsiveSongs[1].GetComponent<SurfAni>().SetValues(1, -MaxRotation, 0, null, null, RestPositions[0]);
            ActiveMwsiveSongs[1].GetComponent<SurfAni>().Play_SurfSideLasPosition();
            GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().AddToPlaylistSwipeLastPosition(GetCurrentMwsiveData().id, ResetTimer());

            ResetSideScroll();
            AddSong.SetActive(true);
            AddSongAnimation();
           

        }
        else
        {
            ResetValue();
        }

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            UIMessage.instance.UIMessageInstanciate("Error, no se ha agregado a playlist");
        }

        Controller.enabled = true;
        HasSwipeEnded = true;
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
            if (Challenge)
            {
                GetCurrentPrefab().GetComponentInChildren<_ChallengeColorAnimation>().ForceClear();
                GetCurrentPrefab().GetComponentInChildren<_ChallengeColorAnimation>().isCompleted = false;
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

            GetCurrentPrefab().GetComponent<MwsiveButton>().PIKCallbackEnd = true;

            if (AddSong.activeSelf)
            {
                AddSong.GetComponent<SurfAni>().Play_SurfAddsongReset();
            }

            if (ResetInternalTimer() >= TimeToWaitMwsiveDB)
            {
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
            }
            StartInternalTimer();

            CurrentPosition--;
            SpawnPosition--;


            ActiveMwsiveSongs[2].GetComponent<ButtonSurfPlaylist>().CheckIfDurationBarCanPlay();
            ActiveMwsiveSongs[3].GetComponent<ButtonSurfPlaylist>().CheckIfDurationBarCanPlay();
            SurfManagerLogicPreviousSong();
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
        if (CurrentPosition < MwsiveSongsData.Count - 1)
        {
            SpotifyPreviewAudioManager.instance.StopTrack();

            if (Challenge)
            {
                GetCurrentPrefab().GetComponentInChildren<_ChallengeColorAnimation>().ForceClear();
                GetCurrentPrefab().GetComponentInChildren<_ChallengeColorAnimation>().isCompleted = false;
            }
            Success = true;

            ActiveMwsiveSongs[1].GetComponent<SurfAni>().SetValues(1, MaxRotation, 0, true);
            ActiveMwsiveSongs[1].GetComponent<SurfAni>().Play_VerticalUp();

            GetCurrentPrefab().GetComponent<MwsiveButton>().PIKCallbackEnd = true;

            ActiveMwsiveSongs[2].GetComponent<SurfAni>().SetValues(1, null, 1, null, null, RestPositions[0]);
            ActiveMwsiveSongs[2].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            ActiveMwsiveSongs[3].GetComponent<SurfAni>().SetValues(1, null, 1, null, null, RestPositions[1]);
            ActiveMwsiveSongs[3].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            ActiveMwsiveSongs[4].GetComponent<SurfAni>().SetValues(1, null, 1, null, null, RestPositions[2]);
            ActiveMwsiveSongs[4].GetComponent<SurfAni>().Play_SurfTransitionOtherSongs();

            if (AddSong.activeSelf)
            {
                AddSong.GetComponent<SurfAni>().Play_SurfAddsongReset();
            }

            if (ResetInternalTimer() >= TimeToWaitMwsiveDB)
            {
                string _trackid = GetCurrentMwsiveData().id;
                if (AppManager.instance.isLogInMode && !_trackid.Equals(""))
                {
                    if (NewScreenManager.instance.TryGetComponent<PF_SurfManager>(out PF_SurfManager pF_SurfManager))
                    {
                        int challenge_id = pF_SurfManager.Challenge ? pF_SurfManager.challenge_id : -1;
                        MwsiveConnectionManager.instance.PostTrackAction(_trackid, "UP", ResetTimer(), null, challenge_id);
                    }
                    else
                    {
                        MwsiveConnectionManager.instance.PostTrackAction(_trackid, "UP", ResetTimer(), null, -1);
                    }
                }
            }
            StartInternalTimer();


            CurrentPosition++;
            SpawnPosition++;


            SurfManagerLogic();
            UIAniManager.instance.SurfAddSongReset(AddSong);
        }
        else
        {
            
            ResetValue();
        }

        if (CurrentPosition >= MwsiveSongsData.Count - 5)
        {
            if (UserPlaylists != null)
            {
                SurfProfileADN();
            }
        }
        Controller.enabled = true;
        HasSwipeEnded = true;
    }

    public void ResetSideScroll()
    {


        DOTween.Complete(ActiveMwsiveSongs[2]);
        DOTween.Complete(ActiveMwsiveSongs[3]);
        DOTween.Complete(ActiveMwsiveSongs[4]);
        DOTween.Complete(AddSong);



    }

    public void AddSongAnimation(bool button = false)
    {
        DOTween.Kill(AddSong);
        if (button)
        {
            AddSong.GetComponent<CanvasGroup>().alpha = 1;
        }
        AddSong.GetComponent<SurfAni>().SetValues(1, null, 1);
        AddSong.GetComponent<SurfAni>().Play_CompleteAddSurfAddSong();
    }

    public void ResetChallengeAnimation()
    {
        if (Challenge)
        {

            GetCurrentPrefab().GetComponentInChildren<_ChallengeColorAnimation>().ResetMask();
        }
    }

    public void OnCallback_ResetSideAnimation()
    {

        if (Challenge)
        {

           GetCurrentPrefab().GetComponentInChildren<_ChallengeColorAnimation>().FirstReset();
        }
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
    public MwsiveData GetCurrentMwsiveData()
    {
        return MwsiveSongsData[CurrentPosition];
    }

    public MwsiveData GetBeforeCurrentMwsiveData()
    {
        try
        {
            return MwsiveSongsData[CurrentPosition - 1];
        }
        catch (System.ArgumentOutOfRangeException)
        {
            return MwsiveSongsData[CurrentPosition];
        }

        
    }

    public MwsiveData GetLastPrefab()
    {

        return MwsiveSongsData[MwsiveSongsData.Count - 1];


    }

    public List<MwsiveData> GetInstances()
    {
        return MwsiveSongsData;
    }
    public void SetChallengeCallback(ChallengeAppObject _challenge)
    {
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

    public void DynamicPrefabSpawnerRecommend(object[] _value, object[] _valuesong)
    {

        TrackRoot trackroot = (TrackRoot)_valuesong[0];

        if (trackroot != null)
        {
            if (trackroot.preview_url != null)
            {

                string artists = "";

                foreach (Artist artist in trackroot.artists)
                {
                    artists = artists + artist.name + ", ";
                }

                artists = artists.Remove(artists.Length - 2);

                string currentPlayListName = AppManager.instance.isLogInMode ? AppManager.instance.GetCurrentPlaylist().name : "";

                MwsiveData instance = new MwsiveData();
                instance.playlist_name = currentPlayListName;
                instance.song_name = trackroot.name;
                instance.album_name = trackroot.album.name;
                instance.artists = artists;
                instance.album_image_url = trackroot.album.images[0].url;
                instance.id = trackroot.id;
                instance.uri = trackroot.uri;
                instance.preview_url = trackroot.preview_url;
                instance.external_url = trackroot.external_urls.spotify;

                MwsiveSongsData.Add(instance); 

            }
        }

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

        if (PageItemPlaylist.total > 100)
        {
            PageItemPlaylistPaginate(100, _spotifyid, PageItemPlaylist.total);
        }

        if (FirstTimeSurf == null)
        {
            SurfManagerLogic(true);
        }


    }

    private void PageItemPlaylistPaginate(int _offset, string _spotifyid = null, int total = 0)
    {
        if (_spotifyid != null)
        {
            idPagingPlItems = _spotifyid;
            offsetPagingPlItems = _offset;
            totalPagingPlItems = total;
        }

        if (offsetPagingPlItems != -1)
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
    private void CallPopUP(PopUpViewModelTypes _type, string _titleText, string _descriptionText, string _actionButtonText = "")
    {
        NewScreenManager.instance.ChangeToMainView(ViewID.PopUpViewModel, true);
        PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(_type, _titleText, _descriptionText, _actionButtonText);
        popUpViewModel.SetPopUpAction(() => { NewScreenManager.instance.BackToPreviousView(); });
    }

    private void Callback_GetTrackInformation(object[] _value)
    {
        if (Challenge)
        {
            if (((long)_value[0]).Equals(WebCallsUtils.GATEWAY_TIMEOUT_CODE) || ((long)_value[0]).Equals(WebCallsUtils.REQUEST_TIMEOUT_CODE) || ((long)_value[0]).Equals(WebCallsUtils.TOO_MANY_REQUEST_CODE))
            {
                CallPopUP(PopUpViewModelTypes.MessageOnly, "Ocurrió un error", "Por favor, intentalo de nuevo más tarde", "Salir");
                PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
                popUpViewModel.SetPopUpCancelAction(() => {

                    NewScreenManager.instance.BackToPreviousView();
                    NewScreenManager.instance.BackToPreviousView();
                });
                return;
            }
        }
        


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
        if(Controller.gameObject != null)
        {
            Controller.gameObject.SetActive(true);
        }
        
        PrefabPosition = 0;
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

            WaitMwsiveDbCo = WaitMwsiveDb();
            StartCoroutine(WaitMwsiveDbCo);

            instance.GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[CurrentPosition]);
            instance.GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
            StartInternalTimer();
            SpawnPosition = CurrentPosition;

        }
        if (MwsiveSongsData.Count - 1 >= CurrentPosition + 1)
        {

            SpawnPrefab().GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[CurrentPosition + 1]);
            
            SpawnPosition++;
        }
        else
        {
            SpawnPrefab();
        }
        if (MwsiveSongsData.Count - 1 >= CurrentPosition + 2)
        {
            SpawnPrefab().GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[CurrentPosition + 2]);
            
            SpawnPosition++;
        }
        else
        {
            SpawnPrefab();
        }
        if (MwsiveSongsData.Count - 1 >= CurrentPosition + 3)
        {
            SpawnPrefab().GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(MwsiveSongsData[CurrentPosition + 3]);
            
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
        ActiveMwsiveSongs.RemoveAt(ActiveMwsiveSongs.Count - 1);
        SpawnPrefabBack();




        WaitMwsiveDbCo = WaitMwsiveDb();
        StartCoroutine(WaitMwsiveDbCo);
        GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();

    }

    private void SurfManagerLogic(bool _firstTime = false)
    {

        if (!_firstTime)
        {

            if (CurrentPosition == MwsiveSongsData.Count - 1 && !lastSongMessage)
            {
                UIMessage.instance.UIMessageInstanciate("Llegaste a la última canción");
                
                if (Challenge)
                {
                    CheckChallengeEnd();
                }

                lastSongMessage = true;
            }
            SpotifyPreviewAudioManager.instance.StopTrack();
            GameObject instance = SpawnPrefab();

            WaitMwsiveDbCo = WaitMwsiveDb();
            StartCoroutine(WaitMwsiveDbCo);

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

        Instance.GetComponent<SurfAni>().isAvailable = false;

        if (ActiveMwsiveSongs.Contains(Instance))
        {

            Instance.GetComponent<SurfAni>().isAvailable = false;
            while (true)
            {

                GameObject Instance2 = PoolManager.instance.GetPooledObject();
                if (!ActiveMwsiveSongs.Contains(Instance2))
                {
                    Instance2.GetComponent<SurfAni>().isAvailable = false;
                    Instance = Instance2;
                    break;
                }
                else
                {
                    Instance2.GetComponent<SurfAni>().isAvailable = false;
                }

                if (Instance2 == null)
                {
                    Debug.LogWarning("Surf overflow");
                    break;
                }
            }


        }

        Instance.GetComponent<CanvasGroup>().alpha = 0;
        Instance.transform.SetParent(MwsiveContainer.transform);
        Instance.transform.SetAsLastSibling();
        ActiveMwsiveSongs.Insert(0, Instance);
        Instance.GetComponent<RectTransform>().offsetMin = new Vector2(LeftRightOffset.x, 0);
        Instance.GetComponent<RectTransform>().offsetMax = new Vector2(LeftRightOffset.y, 0);
        Instance.GetComponent<SurfAni>().ResetRestPosition();
        Instance.transform.eulerAngles = new Vector3(0, 0, 0);
        Instance.SetActive(true);
        return Instance;
    }

    private GameObject SpawnPrefab()
    {


        GameObject Instance = PoolManager.instance.GetPooledObject();

        Instance.GetComponent<SurfAni>().isAvailable = false;

        if (ActiveMwsiveSongs.Contains(Instance))
        {

            Instance.GetComponent<SurfAni>().isAvailable = false;
            while (true)
            {

                GameObject Instance2 = PoolManager.instance.GetPooledObject();
                if (!ActiveMwsiveSongs.Contains(Instance2))
                {
                    Instance2.GetComponent<SurfAni>().isAvailable = false;
                    Instance = Instance2;
                    break;
                }
                else
                {
                    Instance2.GetComponent<SurfAni>().isAvailable = false;
                }

                if (Instance2 == null)
                {
                    Debug.LogWarning("Surf overflow");
                    break;
                }
            }


        }


        Instance.GetComponent<ButtonSurfPlaylist>().ClearData();
        Instance.GetComponent<MwsiveButton>().ChangeSizeForSpawn();
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
        

        Instance.transform.eulerAngles = new Vector3(0, 0, 0);
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

            if (touch.phase == TouchPhase.Ended && touchDuration < 0.2f) //making sure it only check the touch once && it was a short touch/tap and not a dragging.
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
                GetCurrentMwsiveData().isPicked = true;
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
        Instance.GetComponent<RectTransform>().offsetMin = new Vector2(100, -350);
        Instance.GetComponent<RectTransform>().offsetMax = new Vector2(-100, 150);

        UIAniManager.instance.DoubleClickOla(Instance);
    }



    public void MainSceneProfile_OnClick()
    {
        GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
    }

    public void SurfProfileADN(string profileId = null, List<string> value = null)
    {


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


        if (UserPlaylists != null)
        {
            if (ProfilePlaylistPosition < UserPlaylists.items.Count)
            {
                SpotifyConnectionManager.instance.GetPlaylistItems(UserPlaylists.items[ProfilePlaylistPosition].id, OnCallback_GetSpotifyPlaylist, "ES", 100, 0);
            }
            else
            {
                if(MwsiveSongsData.Count == 0)
                {
                    
                    NewScreenManager.instance.ChangeToMainView(ViewID.PopUpViewModel, true);
                    PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
                    popUpViewModel.Initialize(PopUpViewModelTypes.MessageOnly, " ", "´Parece que no hay canciones por surfear", "aceptar", ADNWarning);
                    popUpViewModel.SetPopUpAction(() => { NewScreenManager.instance.BackToPreviousView(); NewScreenManager.instance.BackToPreviousView();
                        #if PLATFORM_ANDROID
                        if (Application.platform == RuntimePlatform.Android)
                        {
                            AppManager.instance.SetAndroidBackAction(() => {
                                NewScreenManager.instance.BackToPreviousView();
                            });
                        }
                        #endif
                    


                    });
                }
            }
        }

    }

    private void OnCallBack_SpawnSeveralTracks(object[] _value)
    {

        SeveralTrackRoot severaltrack = (SeveralTrackRoot)_value[1];
        bool firstTimeSurf = false;
        if (severaltrack.tracks.Count >= 5)
        {
            firstTimeSurf = true;

        }
        DynamicPrefabSpawnerSeveralTracks(severaltrack.tracks, true, false, firstTimeSurf);
    }




    private void OnCallback_GetSpotifyPlaylist(object[] _value)
    {
        ProfileItemsPlaylist = (PlaylistRoot)_value[1];
        if(ProfileItemsPlaylist.items.Count != 0)
        {
            DynamicPrefabSpawnerPLItems(new object[] { ProfileItemsPlaylist }, false, false, UserPlaylists.items[ProfilePlaylistPosition].id, false);
        }
        
        ProfilePlaylistPosition++;

        if (SpawnPosition + 4 >= MwsiveSongsData.Count)
        {
            SurfProfileADN();
            if (WaitForMessageADNCo == null)
            {
                WaitForMessageADNCo = WaitForMessageADN();
                StartCoroutine(WaitForMessageADNCo);
            }
        }
        else
        {
            if (!HasFirstPlaylistPlayed)
            {
                SurfManagerLogicInitialize();
                if(WaitForMessageADNCo != null)
                {
                    StopCoroutine(WaitForMessageADNCo);
                    WaitForMessageADNCo = null;
                }
                
                HasFirstPlaylistPlayed = true;
            }

        }
        
        

    }

    IEnumerator WaitForMessageADN()
    {
        yield return new WaitForSeconds(4);
        MessageADNPopUp.SetActive(true);
    }

    private void Callback_GetUserPlaylists(object[] _value)
    {

        UserPlaylists = (PlaylistRoot)_value[1];

    }


    private void Callback_OnClick_GetUserPlaylistsNotEnoughTracks(object[] _value)
    {

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
    IEnumerator WaitMwsiveDb()
    {
        yield return new WaitForSeconds(TimeToWaitMwsiveDB);
        GetMwsiveInfo();
        WaitMwsiveDbCo = null;
    }

    public float ResetInternalTimer()
    {
        float time2 = InternalTime;
        StopCoroutine("InternalTimer");
        InternalTime = 0;
        if (WaitMwsiveDbCo != null)
        {
            StopCoroutine(WaitMwsiveDbCo);
        }
        return time2;
    }

    public void StartInternalTimer()
    {
        StartCoroutine("InternalTimer");
    }

    IEnumerator InternalTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            InternalTime++;
        }
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
