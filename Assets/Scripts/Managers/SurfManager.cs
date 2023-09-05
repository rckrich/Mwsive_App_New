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
    public GameObject Prefab, MainCanvas, AddSong, OlaButton, MwsiveOla, MwsiveContainer;
    public List <GameObject> MwsiveSongs = new List<GameObject>();
    public GameObject[ ] RestPositions;

    public float MaxRotation = 18f;
    public float SurfSuccessSensitivity = 2.2f;
    public Vector2 LeftRightOffset;
    public bool CanGetRecomendations = false;
    private SpotifyPlaylistRoot searchedPlaylist, ProfilePlaylist;
    private RecommendationsRoot recommendationsRoot;
    private PlaylistRoot UserPlaylists;
    private AlbumRoot albumroot;

    [HideInInspector]
    public bool canSwipe = true;
    private Vector2 ControllerPostion = new Vector2();
    private int CurrentPosition = 0;
    private int PrefabPosition = 0;
    private int ProfilePlaylistPosition = 0;
    private bool HasSwipeEnded = true;
    private bool Success = false;
    private bool SpawnedBuffer = false;
    private bool SurfProfile = false;
    private int SurfProfileOffsetPosition;
    private int trackstospawn = 0;
    private bool HasFirstPlaylistPlayed = false;
  
    private void Start()
    {
        if(UIAniManager.instance.MainCanvas == null){
            UIAniManager.instance.MainCanvas = MainCanvas;
        }
        ControllerPostion = new Vector2(Controller.transform.position.x, Controller.transform.position.y); 
    }

   
    private void OnEnable()
    {
        SurfController.instance.AddToList(gameObject);
        
        swipeListener.OnSwipe.AddListener(OnSwipe);

        GameObject currentPrefab = GetCurrentPrefab();

        if(currentPrefab != null && SurfController.instance.AmICurrentView(gameObject))
            currentPrefab.GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
        
        
        
        
        
        
    }

    private void OnDestroy() {
        SurfController.instance.DeleteFromList(gameObject);
    }

    private void OnDisable()
    {
        swipeListener.OnSwipe.RemoveListener(OnSwipe);
        
    }

    

    private void OnSwipe(string swipe)
    {
        if (!canSwipe)
            return;

        switch (swipe){
            case "Right":
                
                Controller.vertical =false;
                Controller.horizontal =false;
                HasSwipeEnded = false;
                Debug.Log(gameObject.name + "asdfg");
                SideScrollSuccess();
                
            break;
            case "Up":
                
                Controller.vertical =false;
                Controller.horizontal =false;
                HasSwipeEnded = false;
                UpScrollSuccess();
            break;
            case "Down":
                
                Controller.vertical =false;
                Controller.horizontal =false;
                HasSwipeEnded = false;
                DownScrollSuccess();
            break;
        }
        //Debug.Log(swipe);
    }
    
    public void ValChange(){
        
        if(Controller.transform.position.x > ControllerPostion.x*1.1){
            Controller.vertical =false;
           SideScrollAnimation();
        }if(Controller.transform.position.y > ControllerPostion.y*1.1){
            Controller.horizontal =false;
            UpScrollAnimation();
        }if(Controller.transform.position.y < ControllerPostion.y*.9){
            Controller.horizontal =false;
            DownScrollAnimation();
        }
        

    }



    private void SideScrollAnimation(){
        
        DOTween.Kill(MwsiveSongs[CurrentPosition], true);
        DOTween.Kill(MwsiveSongs[CurrentPosition+1], true);
        DOTween.Kill(MwsiveSongs[CurrentPosition+2], true);
        DOTween.Kill(MwsiveSongs[CurrentPosition+3], true);  
            float var = Controller.transform.position.x/ControllerPostion.x*.25f;
            float Fade =ControllerPostion.x/Controller.transform.position.x;

            UIAniManager.instance.SurfSide(MwsiveSongs[CurrentPosition],var, -MaxRotation, Fade,false);
            UIAniManager.instance.SurfAddSong(AddSong, var);

            if(CurrentPosition < MwsiveSongs.Count-4){
                UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition+1], RestPositions[0], var);
                UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition+2], RestPositions[1], var);
                UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition+3], RestPositions[2], var);
            }
            
        Success = false;
    }


    private void DownScrollAnimation(){
        DOTween.Kill(MwsiveSongs[CurrentPosition], true);
        DOTween.Kill(MwsiveSongs[CurrentPosition+1], true);
        DOTween.Kill(MwsiveSongs[CurrentPosition+2], true);
        DOTween.Kill(MwsiveSongs[CurrentPosition+3], true); 
            float var = Controller.transform.position.y/ControllerPostion.y;
            float Fade =ControllerPostion.y/Controller.transform.position.y;

            UIAniManager.instance.SurfVerticalDown(MwsiveSongs[CurrentPosition],var*-.5f, MaxRotation, Fade,true);
            
            UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition+1], RestPositions[1], var*.25f);
            UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition+2], RestPositions[2], var*.25f);
        Success = false;
            

         Debug.Log("DownScrollAnimation");     
    }


    private void UpScrollAnimation(){
        DOTween.Kill(MwsiveSongs[CurrentPosition], true);
        DOTween.Kill(MwsiveSongs[CurrentPosition+1], true);
        DOTween.Kill(MwsiveSongs[CurrentPosition+2], true);
        DOTween.Kill(MwsiveSongs[CurrentPosition+3], true); 
        
            float var = Controller.transform.position.y/ControllerPostion.y;
            float Fade = Controller.transform.position.y/ControllerPostion.y;
            float VAR2  =ControllerPostion.y/Controller.transform.position.y;
            
            UIAniManager.instance.SurfVerticalUp(MwsiveSongs[CurrentPosition],var*.5f, MaxRotation, Fade,false);
            

            if(CurrentPosition < MwsiveSongs.Count-4){
                UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition+1], RestPositions[0], VAR2*.25f);
                UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition+2], RestPositions[1], VAR2*.25f);
                UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition+3], RestPositions[2], VAR2*.25f);
            }

            
        Success = false;
        
          Debug.Log("UpScrollAnimation");     
    }

    public void OnEndDrag(){
        while (HasSwipeEnded){
            if(MwsiveSongs[CurrentPosition].transform.position.x >= ControllerPostion.x*SurfSuccessSensitivity){
                if(CurrentPosition > MwsiveSongs.Count-4){
                    SideScrollSuccess();
                }
                
                break;

            }else if(MwsiveSongs[CurrentPosition].transform.position.y >= ControllerPostion.y*SurfSuccessSensitivity*1.5){
                UpScrollSuccess();
                break;

            }else if(MwsiveSongs[CurrentPosition].transform.position.y <= ControllerPostion.y/SurfSuccessSensitivity){
                DownScrollSuccess();
                break;

            }else{
                ResetValue();
                break;
            }
        }
        Controller.enabled =true;    
        
        
    }

    private void SideScrollSuccess(){
       
        Controller.enabled =false;
        Controller.horizontal =true;
        Controller.vertical =true;
        Controller.transform.position = new Vector2(ControllerPostion.x,ControllerPostion.y);
        if(CurrentPosition < MwsiveSongs.Count-4){
            DOTween.CompleteAll(true);
            UIAniManager.instance.SurfSide(MwsiveSongs[CurrentPosition],1, -MaxRotation,0,true);
            Debug.Log("CompleteSurfadd" + gameObject.name);
            UIAniManager.instance.CompleteSurfAddSong(AddSong, 1.5f);

            UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition+1], RestPositions[0], 1);
            UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition+2], RestPositions[1], 1);
            UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition+3], RestPositions[2], 1);

            
            CurrentPosition++;
            GetBeforeCurrentPrefab().GetComponent<ButtonSurfPlaylist>().Swipe();
            GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
            Success = true;
        }else{
            ResetValue();
        }




        
        if(CurrentPosition == PrefabPosition-4){
            if(CanGetRecomendations){
                SpawnRecommendations();
            }else if(SurfProfile){
                SurfProfileADN();
            }else if(!CanGetRecomendations && !SpawnedBuffer && !SurfProfile){
                SpawnPrefab();
                SpawnPrefab();
                SpawnPrefab();
                SpawnedBuffer = true;
            }
        }

        

        
        HasSwipeEnded = true;
    }
    private void DownScrollSuccess(){
        Controller.enabled =false;
        Controller.horizontal =true;
        Controller.vertical =true;
        Controller.transform.position = new Vector2(ControllerPostion.x,ControllerPostion.y);
        if(CurrentPosition > 0){
            Success = true;
            UIAniManager.instance.SurfVerticalDown(MwsiveSongs[CurrentPosition],1, -MaxRotation, 0,true);
            
            UIAniManager.instance.SurfTransitionBackSong(MwsiveSongs[CurrentPosition-1], RestPositions[0], MaxRotation);
            UIAniManager.instance.SurfTransitionBackSongDown(MwsiveSongs[CurrentPosition], RestPositions[1]);
            UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition+1], RestPositions[2], 1);
            UIAniManager.instance.SurfTransitionBackHideSong(MwsiveSongs[CurrentPosition+2], RestPositions[3], 1);
            

            UIAniManager.instance.SurfAddSongReset(AddSong);

            GetBeforeCurrentPrefab().GetComponent<ButtonSurfPlaylist>().BackSwipe();
            CurrentPosition--;           
            GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
            

        }
        else{
            ResetValue();
        }

         Debug.Log("DownScrollSucess");  
        HasSwipeEnded = true;
    }
    private void UpScrollSuccess(){

            Controller.enabled =false;
            Controller.horizontal =true;
            Controller.vertical =true;
            Controller.transform.position = new Vector2(ControllerPostion.x,ControllerPostion.y);
            if(CurrentPosition < MwsiveSongs.Count-4){
                Success = true;
                
                UIAniManager.instance.SurfVerticalUp(MwsiveSongs[CurrentPosition],1, MaxRotation, 0,true);
                

                UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition+1], RestPositions[0], 1);
                UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition+2], RestPositions[1], 1);
                
                UIAniManager.instance.SurfTransitionOtherSongs(MwsiveSongs[CurrentPosition+3], RestPositions[2], 1);
                
                
                CurrentPosition++;
                GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
                UIAniManager.instance.SurfAddSongReset(AddSong);
            }else{
                ResetValue();
            }
            if(CurrentPosition == PrefabPosition-4){
                if(CanGetRecomendations){
                    SpawnRecommendations();
                }else if(SurfProfile){
                    SurfProfileADN();
                }else if(!CanGetRecomendations && !SpawnedBuffer && !SurfProfile){
                    SpawnPrefab();
                    SpawnPrefab();
                    SpawnPrefab();
                    SpawnedBuffer = true;
                }
        }
            
            Debug.Log("UpScrollSuccess"); 
            HasSwipeEnded = true;
    }
           

    public void SpawnRecommendations(){
        if(CanGetRecomendations){
            List<string> _artists = new List<string>();
            List<string> _tracks = new List<string>();

            if(recommendationsRoot == null){
                for (int i = searchedPlaylist.tracks.items.Count-1; i > searchedPlaylist.tracks.items.Count-3   ; i--)
                {
                    _artists.Add(searchedPlaylist.tracks.items[i].track.artists[0].id); 
                    _tracks.Add(searchedPlaylist.tracks.items[i].track.id);
                    
                }
            }else{
                for (int i = recommendationsRoot.tracks.Count-1; i > recommendationsRoot.tracks.Count-3   ; i--)
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

    public void ResetValue(){

        if(!Success){
            Debug.Log("Reset");
            DOTween.CompleteAll(true);
            Controller.horizontal =true;
            Controller.vertical =true;
            HasSwipeEnded = true;
            Controller.transform.position = new Vector2(ControllerPostion.x,ControllerPostion.y);
            UIAniManager.instance.SurfReset(MwsiveSongs[CurrentPosition]);
            UIAniManager.instance.SurfAddSongReset(AddSong);
            
            UIAniManager.instance.SurfResetOtherSongs(MwsiveSongs[CurrentPosition+1], RestPositions[1], true);
            UIAniManager.instance.SurfResetOtherSongs(MwsiveSongs[CurrentPosition+2], RestPositions[2], true);
            UIAniManager.instance.SurfResetOtherSongs(MwsiveSongs[CurrentPosition+3], RestPositions[3], false);
        }else{
            Success = false;
        }

    }
    public GameObject GetBeforeCurrentPrefab(){
        GameObject _Instance = MwsiveSongs[CurrentPosition-1];
        return _Instance;
    }

    public GameObject GetCurrentPrefab(){
        GameObject _Instance = null;

        if (MwsiveSongs.Count > 0)
        {
            _Instance = MwsiveSongs[CurrentPosition];
        }

        return _Instance;
    }

    public void DynamicPrefabSpawnerRecommendations(object[] _value)
    {
        recommendationsRoot = (RecommendationsRoot)_value[0];

        GameObject FirstInstance = null;
        int SpawnedSongs = 0;
        foreach (var item in recommendationsRoot.tracks)
        {
            if(item != null){
                if (item.preview_url != null)
                {
                    GameObject instance = SpawnPrefab();
                    if (FirstInstance == null)
                    {
                        FirstInstance = instance;
                    }

                    string artists = "";

                    foreach (Artist artist in item.artists)
                    {
                        artists = artists + artist.name + ", ";
                    }

                    artists = artists.Remove(artists.Length - 2);

                    instance.GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(AppManager.instance.GetCurrentPlaylist().name, item.name, item.album.name, artists, item.album.images[0].url, item.id, item.uri, item.preview_url, item.external_urls.spotify);
                    
                    SpawnedSongs++;
                }
            }
            
        }
        
        if(SpawnedSongs < 5 && SpawnedSongs > 0){
            SpawnPrefab();
            SpawnPrefab();
            SpawnPrefab();
            SpawnedBuffer = true;
        }
    }

    public void DynamicPrefabSpawnerAlbum(object[] _value)
    {
        albumroot = (AlbumRoot)_value[0];

        GameObject FirstInstance = null;
        
        string image = albumroot.images[0].url;
        string albumname = albumroot.name;
        int SpawnedSongs = 0;
        
        foreach (var item in albumroot.tracks.items)
        {
            if(item != null){
                if (item.preview_url != null)
                {
                    
                    GameObject instance = SpawnPrefab();
                    Debug.Log(FirstInstance);
                    if (FirstInstance == null)
                    {
                        Debug.Log("aaaaaaaaaaaaaaaaaaaa");
                        FirstInstance = instance;
                    }

                    string artists = "";

                    foreach (Artist artist in item.artists)
                    {
                        artists = artists + artist.name + ", ";
                    }

                    artists = artists.Remove(artists.Length - 2);

                    instance.GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(AppManager.instance.GetCurrentPlaylist().name, item.name, albumname, artists, image, item.id, item.uri, item.preview_url, item.external_urls.spotify);
                    
                    SpawnedSongs++;
                }
            }
            
           
        }
        Debug.Log(FirstInstance);
        
        FirstInstance.GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
        if(SpawnedSongs < 5 && SpawnedSongs > 0){
            SpawnPrefab();
            SpawnPrefab();
            SpawnPrefab();
            SpawnedBuffer = true;
        }
    }


    public void DynamicPrefabSpawnerSong(object[] _value)
    {
        recommendationsRoot = (RecommendationsRoot)_value[0];

        GameObject FirstInstance = null;
        
        int SpawnedSongs = 0;
        foreach (var item in recommendationsRoot.tracks)
        {
            if(item != null){
                if (item.preview_url != null)
                {
                    GameObject instance = SpawnPrefab();
                    if (FirstInstance == null)
                    {
                        FirstInstance = instance;
                    }

                    string artists = "";

                    foreach (Artist artist in item.artists)
                    {
                        artists = artists + artist.name + ", ";
                    }

                    artists = artists.Remove(artists.Length - 2);

                    string currentPlaylistName = AppManager.instance.isLogInMode ? AppManager.instance.GetCurrentPlaylist().name : "";

                    //TODO filtrar cuando no tienen un elemento
                    instance.GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(currentPlaylistName, item.name, item.album.name, artists, item.album.images[0].url, item.id, item.uri, item.preview_url, item.external_urls.spotify);
                    
                    SpawnedSongs++;

                }
            }
            
        }
        
        FirstInstance.GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
        if(SpawnedSongs < 5 && SpawnedSongs > 0){
            SpawnPrefab();
            SpawnPrefab();
            SpawnPrefab();
            SpawnedBuffer = true;
        } 
    }

    public void DynamicPrefabSpawnerPL(object[] _value, bool PlayFirstTrack =true, bool CanSpawnBuffer = true)
    {
        
        searchedPlaylist = (SpotifyPlaylistRoot)_value[0];

        Debug.Log("----------------------------" + searchedPlaylist.tracks.items.Count);
        GameObject FirstInstance = null;

        int SpawnedSongs = 0;
        foreach (var item in searchedPlaylist.tracks.items)
        {
            if(item.track != null)
            {
                if (item.track.preview_url != null)
                {
                    GameObject instance = SpawnPrefab();
                    if (FirstInstance == null)
                    {
                        FirstInstance = instance;
                    }

                    string artists = "";

                    foreach (Artist artist in item.track.artists)
                    {
                        artists = artists + artist.name + ", ";
                    }

                    artists = artists.Remove(artists.Length - 2);

                    string currentPlayListName = AppManager.instance.isLogInMode ? AppManager.instance.GetCurrentPlaylist().name : "";
                    instance.GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(currentPlayListName, item.track.name, item.track.album.name, artists, item.track.album.images[0].url, item.track.id, item.track.uri, item.track.preview_url, item.track.external_urls.spotify);
                    
                    SpawnedSongs++;
                }
            }
            
        }
        
        if(SpawnedSongs < 5 && SpawnedSongs > 0 && SpawnedBuffer == false && CanSpawnBuffer == true){
            SpawnPrefab();
            SpawnPrefab();
            SpawnPrefab();
            SpawnedBuffer = true;
        }
        if(PlayFirstTrack){
            FirstInstance.GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
        }
        
    }

    public void DynamicPrefabSpawnerSeveralTracks(List<Track> tracks, bool PlayFirstTrack =true, bool CanSpawnBuffer = true )
    {
        
        GameObject FirstInstance = null;

        int SpawnedSongs = 0;
        foreach (var item in tracks)
        {
            if(item != null)
            {
                if (item.preview_url != null)
                {
                    GameObject instance = SpawnPrefab();
                    if (FirstInstance == null)
                    {
                        FirstInstance = instance;
                    }

                    string artists = "";

                    foreach (Artist artist in item.artists)
                    {
                        artists = artists + artist.name + ", ";
                    }

                    artists = artists.Remove(artists.Length - 2);

                    instance.GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(AppManager.instance.GetCurrentPlaylist().name, item.name, item.album.name, artists, item.album.images[0].url, item.id, item.uri, item.preview_url, item.external_urls.spotify);
                    
                    SpawnedSongs++;
                }
            }
            
        }
        
        if(SpawnedSongs < 5 && SpawnedSongs > 0 && SpawnedBuffer == false && CanSpawnBuffer == true){
            SpawnPrefab();
            SpawnPrefab();
            SpawnPrefab();
            SpawnedBuffer = true;
        }
        if(PlayFirstTrack){
            FirstInstance.GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
        }
    }


    public void SpawnSharePrefab(string SpotifyID){
        SpotifyConnectionManager.instance.GetTrack(SpotifyID, Callback_SpawnSharePrefab);
        
    }

    private void Callback_SpawnSharePrefab(object[] _value){
        TrackRoot trackRoot = (TrackRoot)_value[1];
        int SiblingIndex = GetCurrentPrefab().transform.GetSiblingIndex();
        if (trackRoot != null || trackRoot.preview_url != null){
            GameObject Instance;
            

            Instance = Instantiate(Prefab,new Vector3(0,0,0), Quaternion.identity);
            Instance.SetActive(false);
            Instance.transform.SetParent(MwsiveContainer.transform);
            Instance.transform.localScale = new Vector3 (1f,1f,1f);
            Instance.GetComponent<RectTransform>().offsetMin = new Vector2 (LeftRightOffset.x,0);
            Instance.GetComponent<RectTransform>().offsetMax = new Vector2 (LeftRightOffset.y,0);

            MwsiveSongs.Insert(MwsiveSongs.IndexOf(GetCurrentPrefab()), Instance);
          
            Instance.transform.SetSiblingIndex(SiblingIndex+1);
            
            

            string artists = "";
            foreach (Artist artist in trackRoot.artists)
            {
                artists = artists + artist.name + ", ";
            }

            artists = artists.Remove(artists.Length - 2);
            Instance.GetComponent<ButtonSurfPlaylist>().InitializeMwsiveSong(AppManager.instance.GetCurrentPlaylist().name, trackRoot.name, trackRoot.album.name, artists, trackRoot.album.images[0].url, trackRoot.id, trackRoot.uri, trackRoot.preview_url, trackRoot.external_urls.spotify);
            PrefabPosition++;

            UIAniManager.instance.SurfShareSpawn(Instance);
            if(CurrentPosition > 0)
            {
               ResetValue();
                UIAniManager.instance.SurfTransitionBackSong(Instance, RestPositions[0], MaxRotation);
                Instance.GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();

            }
            else
            {
                DownScrollSuccess();
                UIAniManager.instance.SurfTransitionBackSong(Instance, RestPositions[0], MaxRotation);
                Instance.GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
            }
            
            Instance.GetComponent<ButtonSurfPlaylist>().SetSurfManager(gameObject);
        }
        else{
            UIMessage.instance.UIMessageInstanciate("Esta canción no esta disponible");
        }
        
    }


    private GameObject SpawnPrefab(){
        GameObject Instance;
        if(PrefabPosition < 4){
            Instance = Instantiate(Prefab,RestPositions[PrefabPosition].transform.position, Quaternion.identity);
            Instance.SetActive(true);
            Instance.GetComponent<CanvasGroup>().alpha = RestPositions[PrefabPosition].GetComponent<CanvasGroup>().alpha;

        }else{
            Instance = Instantiate(Prefab,RestPositions[3].transform.position, Quaternion.identity);
            Instance.SetActive(false);
            Instance.GetComponent<CanvasGroup>().alpha = 0;
            
        }
        Instance.name = "PF_Mwsive_Song " + PrefabPosition;
        
        Instance.transform.SetParent(MwsiveContainer.transform);
        Instance.transform.SetAsFirstSibling();
        MwsiveSongs.Add(Instance);
        Instance.GetComponent<RectTransform>().offsetMin = new Vector2 (LeftRightOffset.x,0);
        Instance.GetComponent<RectTransform>().offsetMax = new Vector2 (LeftRightOffset.y,0);
        if(PrefabPosition < 1){
                Instance.transform.localScale = new Vector3 (1f,1f,1f);
                Instance.transform.position = RestPositions[PrefabPosition].transform.position;
            }else if (PrefabPosition < 2){
                Instance.transform.localScale = new Vector3 (.9f,.9f,.9f);
                Instance.transform.position = RestPositions[PrefabPosition].transform.position;
            }else if (PrefabPosition < 3){
                Instance.transform.localScale = new Vector3 (.8f,.8f,.8f);
                Instance.transform.position = RestPositions[PrefabPosition].transform.position;
            }else if (PrefabPosition >3){
                Instance.transform.localScale = new Vector3 (.6f,.6f,.6f);
                Instance.transform.position = RestPositions[3].transform.position;
            }
        Instance.GetComponent<ButtonSurfPlaylist>().SetSurfManager(gameObject);
        PrefabPosition++;
        return Instance;
    }

    float touchDuration;
    Touch touch;
    void Update() {
        
        if (Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null && canSwipe){ //if there is any touch
            touchDuration += Time.deltaTime;
            touch = Input.GetTouch(0);
 
            if(touch.phase == TouchPhase.Ended && touchDuration < 0.2f) //making sure it only check the touch once && it was a short touch/tap and not a dragging.
                StartCoroutine("singleOrDouble");
        }
        else
            touchDuration = 0.0f;
    }
 
    IEnumerator singleOrDouble(){
        
        yield return new WaitForSeconds(0.2f);
        if(touch.tapCount == 1){
            GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().OnClic_StopAudioPreview();
        }    
        else if(touch.tapCount == 2){
            //this coroutine has been called twice. We should stop the next one here otherwise we get two double tap
            StopCoroutine("singleOrDouble");

           GameObject Instance = Instantiate(MwsiveOla, Vector3.zero, Quaternion.identity);
            Instance.transform.SetParent(GameObject.Find("SpawnableCanvas").transform);
            Instance.GetComponent<RectTransform>().offsetMin = new Vector2(100,250);
            Instance.GetComponent<RectTransform>().offsetMax = new Vector2(-100,-250);

            UIAniManager.instance.DoubleClickOla(Instance);
            if(!OlaButton.GetComponent<MwsiveControllerButtons>().IsItOlaColorButtonActive()){
                OlaButton.GetComponent<MwsiveControllerButtons>().OnClickOlaButton();
                
            }
        }
    }
    

    public void MainSceneProfile_OnClick(){
        GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().PlayAudioPreview();
    }


    public void SurfProfileADN( string profileId = null, object[] value = null){

       
        if(profileId != null){
            Debug.Log("a");
            if(value != null){
                SpotifyConnectionManager.instance.GetUserPlaylists(profileId, Callback_OnClick_GetUserPlaylists);
            }else{
                SpotifyConnectionManager.instance.GetUserPlaylists(profileId, Callback_OnClick_GetUserPlaylistsNoADN);
            }
            
            
        }
        if(SurfProfile){
            Debug.Log(UserPlaylists.items.Count);
            Debug.Log(ProfilePlaylistPosition);
            if(ProfilePlaylistPosition <= UserPlaylists.items.Count){



                if(ProfilePlaylist.tracks.items.Count < 50){
                    if(ProfilePlaylist.tracks.items.Count > 4){
                        if(!HasFirstPlaylistPlayed){
                            HasFirstPlaylistPlayed = true;
                            DynamicPrefabSpawnerPL(new object[] { ProfilePlaylist }, true, false);
                        }else{
                            DynamicPrefabSpawnerPL(new object[] { ProfilePlaylist }, false, false);
                        }
                        
                        ProfilePlaylistPosition++;
                        SpotifyConnectionManager.instance.GetPlaylist(UserPlaylists.items[ProfilePlaylistPosition].id, OnCallBack_SpawnUserPlaylists);
                    }
                    else{
                        if(ProfilePlaylist.tracks.items.Count != 0)
                        {
                            if(!HasFirstPlaylistPlayed){
                                HasFirstPlaylistPlayed = true;
                                DynamicPrefabSpawnerPL(new object[] { ProfilePlaylist }, true, false);
                        }   else{
                                DynamicPrefabSpawnerPL(new object[] { ProfilePlaylist }, false, false);
                            }
                        }
                        ProfilePlaylistPosition++;
                        SpotifyConnectionManager.instance.GetPlaylist(UserPlaylists.items[ProfilePlaylistPosition].id, OnCallBack_SpawnUserPlaylistsNoPlaylist);

                    }

                    
                }else{
                    
                    if(trackstospawn + 50 < ProfilePlaylist.tracks.items.Count){
                        trackstospawn += 50;
                    }else{
                        trackstospawn = ProfilePlaylist.tracks.items.Count;
                        ProfilePlaylistPosition++;
                        SpotifyConnectionManager.instance.GetPlaylist(UserPlaylists.items[ProfilePlaylistPosition].id, OnCallBack_SpawnUserPlaylists);
                        trackstospawn = 0;
                    }

                    List<Track> ListOfTracksToSpawn = new List<Track>();
                    for (int i = SurfProfileOffsetPosition; i < trackstospawn; i++)
                    {
                        ListOfTracksToSpawn.Add(ProfilePlaylist.tracks.items[i].track);
                    }

                     if(!HasFirstPlaylistPlayed){
                            HasFirstPlaylistPlayed = true;
                            DynamicPrefabSpawnerSeveralTracks(ListOfTracksToSpawn,true, false);
                        }else{
                            DynamicPrefabSpawnerSeveralTracks(ListOfTracksToSpawn,false, false);
                        }
                   

                    if (ProfilePlaylist.tracks.items.Count - trackstospawn + 50 < 4)
                    {
                        SpotifyConnectionManager.instance.GetPlaylist(UserPlaylists.items[ProfilePlaylistPosition].id, OnCallBack_SpawnUserPlaylistsNoPlaylist);
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
        if(value != null){
            SurfProfile = true;
            //Bla bla spawnear cosas de ADN Musical;
        }
        


    }



    private void OnCallBack_SpawnUserPlaylists(object[] _value){
        ProfilePlaylist = (SpotifyPlaylistRoot)_value[1];

    }
    private void OnCallBack_SpawnUserPlaylistsNoPlaylist(object[] _value)
    {
        ProfilePlaylist = (SpotifyPlaylistRoot)_value[1];
        SurfProfileADN();

    }


    private void Callback_OnClick_GetUserPlaylists(object[] _value){

        UserPlaylists = (PlaylistRoot)_value[1];
        Debug.Log(UserPlaylists.items.Count);
        SpotifyConnectionManager.instance.GetPlaylist(UserPlaylists.items[ProfilePlaylistPosition].id, OnCallBack_SpawnUserPlaylists);

    }

    private void Callback_OnClick_GetUserPlaylistsNoADN(object[] _value){

        UserPlaylists = (PlaylistRoot)_value[1];
        Debug.Log(UserPlaylists.items.Count);
        SpotifyConnectionManager.instance.GetPlaylist(UserPlaylists.items[ProfilePlaylistPosition].id, OnCallBack_SpawnUserPlaylistsNoPlaylist);
        SurfProfile = true;
    }


}
