using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonSurfPlaylist : ViewModel
{
    private const int THOUSEND_CONVERT_TO_K = 1000;
    private const int MILLION_CONVERT_TO_M = 1000000;

    public TMP_Text playlistText;
    public TMP_Text trackName;
    public TMP_Text artistName;
    public TMP_Text albumName;
    public Image trackCover;
    public Image[] topCuratorImages;
    public string playlistName;
    public AppManager appManager;
    public Transform transformImage;
    public string trackID;
    public List<string> uris = new List<string>();
    public string previewURL;
    public string mp3URL;
    public bool isAdd = false;
    public string externalURL;
    public bool changeColor = false;

    public MwsiveButton mwsiveButton;
    public ChallengeColorAnimation challengecoloranimation;
    public GameObject loadingAnimGameObject;
    public DurationBar durationBar;
    public GameObject buttonColor;
    public bool TrackPoints;

    [Header("Track Info")]
    public TextMeshProUGUI trackTotalPicks;
    public TextMeshProUGUI trackTotalRecommendation;
    public TextMeshProUGUI trackTopCuratorsThatVoted;

    private Color redNew = new Color(0.9411765f, 0.2941177f, 0.4156863f);
    private Color gray = new Color(0.8f, 0.8f, 0.8f);
    private GameObject Surf;
    private ChallengeAppObject Challenge;
    private string trackId;
    private float time;

    public bool SuccesfulEnded = false;
    public bool AmILastPosition = false;
    private TrackInfoRoot trackInfoRoot;

    private bool isPicked = false;
    private bool isRecommended = false;


    public void SetSelectedPlaylistNameAppEvent(string _playlistName)
    {
        playlistName = _playlistName;

    }

    public void SetChangeColorAppEvent(Color _color, Color _colorText)
    {
        buttonColor.GetComponent<Image>().color = _color;
        playlistText.color = _colorText;
    }
    public string GetSelectedPlaylistNameAppEvent() { return playlistName; }

    public string GetChangeColorAppEvent() { return playlistText.text;  }

    private void OnEnable()
    {
        playlistText.text = playlistName;
        AddEventListener<ChangeColorAppEvent>(ChangeEventListener);
        AddEventListener<SelectedPlaylistNameAppEvent>(SelectedPlaylistNameEventListener);
        string currentPLayListName = AppManager.instance.isLogInMode ? AppManager.instance.GetCurrentPlaylist().name : "";
            playlistText.text = currentPLayListName;
        if (!AppManager.instance.yours)
        {
            buttonColor.GetComponent<Image>().color = redNew;
            playlistText.color = redNew;
        }
        else
        {
            buttonColor.GetComponent<Image>().color = gray;
            playlistText.color = Color.black;
        }
    }

    private void OnDisable()
    {
        RemoveEventListener<SelectedPlaylistNameAppEvent>(SelectedPlaylistNameEventListener);
        RemoveEventListener<ChangeColorAppEvent>(ChangeEventListener);
        durationBar.CheckforPoints = false;
        
        
    }

    public void InitializeMwsiveSong(string _playlistName, string _trackname, string _album, string _artist, string _image, string _spotifyid, string _url, string _previewURL, string _externalURL, bool _trackPoints = false)
    {
        if(_playlistName != null){
            playlistText.text = _playlistName;
        }
        if(_trackname != null){
            if (_trackname.Length > 27)
            {
                string _text2 = "";
                for (int i = 0; i < 27; i++)
                {
                    _text2 = _text2 + _trackname[i];
                }
                _text2 = _text2 + "...";
                trackName.text = _text2;
            }
            else
            {
                trackName.text = _trackname;
            }
        }
        
        if(_album != null){
            albumName.text = _album;
        }
        if(_artist != null){
            artistName.text = _artist;
        }
        
        if(_image != null){
            ImageManager.instance.GetImage(_image, trackCover, (RectTransform)this.transform);
        }
        
        if(_spotifyid != null){
            trackID = _spotifyid;
            if (isRecommended)
            {
                mwsiveButton.ChangeAddToPlaylistButtonColor(0.5f);
                //Pintar de morado el que está en playlist
            }
        }
        
        if(_url != null){
            uris[0] = _url;
        }
        
        if(_previewURL != null){
            previewURL = _previewURL;
        }
        
        if(_externalURL != null){
            externalURL = _externalURL;
        }
        TrackPoints = _trackPoints;

        
    }

    public void LastPosition(){
        if(AmILastPosition && SuccesfulEnded){
            Challenge.CheckForPoints();
        }
    }

    public void SetCallbackLastPosition(ChallengeAppObject _challenge){
        Challenge = _challenge;
        AmILastPosition = true;
    }

    public void PlayAudioPreview()
    {
        try
        {
            if(transform.IsChildOf(Surf.GetComponent<PF_SurfManager>().GetCurrentPrefab().transform) && TrackPoints){
               durationBar.SetCallBack(gameObject.GetComponent<ButtonSurfPlaylist>());
                durationBar.CheckforPoints = true;
            }else{
                durationBar.CheckforPoints = false;
            }
        }
        catch (System.NullReferenceException)
        {
            if(transform.IsChildOf(Surf.GetComponent<SurfManager>().GetCurrentPrefab().transform) && TrackPoints){
                durationBar.SetCallBack(gameObject.GetComponent<ButtonSurfPlaylist>());
                durationBar.CheckforPoints = true;
            }
            else{
                durationBar.CheckforPoints = false;
            }
        }
        
        SpotifyPreviewAudioManager.instance.GetTrack(previewURL, Callback_GetTrack);
    }

    private void Callback_GetTrack(object[] _list)
    {
        CheckIfDurationBarCanPlay();
        if(loadingAnimGameObject != null)
        {
            loadingAnimGameObject.SetActive(false);
        }
        

        
        if (SurfManager.instance.isActiveAndEnabled == false)
        {
            if (NewScreenManager.instance.GetCurrentView().GetViewID() != ViewID.SurfViewModel)
            {
                SpotifyPreviewAudioManager.instance.StopTrack();
            }
            
        }

        if (AppManager.instance.isLogInMode)
        {
            MwsiveConnectionManager.instance.GetTrackInformation_Auth(trackID, AppManager.instance.GetCurrentPlaylist().id, Callback_GetTrackInformation);
        }
        else
        {
            MwsiveConnectionManager.instance.GetTrackInformation_NoAuth(trackID, Callback_GetTrackInformation);
        }
    }

    public void CheckIfDurationBarCanPlay(){
        try
        {
            if(Surf != null)
            {
                if (transform.IsChildOf(Surf.GetComponent<PF_SurfManager>().GetCurrentPrefab().transform))
                {
                    durationBar.canPlay = true;
                    if (TrackPoints)
                    {
                        challengecoloranimation.Initialize();
                    }
                }
                else
                {
                    durationBar.canPlay = false;
                }
            }
            
        }
        catch (System.NullReferenceException)
        {
            if(Surf != null)
            {
                if (transform.IsChildOf(Surf.GetComponent<SurfManager>().GetCurrentPrefab().transform))
                {
                    durationBar.canPlay = true;
                    if (TrackPoints)
                    {
                        challengecoloranimation.Initialize();
                    }
                    
                }
                else
                {
                    durationBar.canPlay = false;
                }
            }
           
        }
    }

    public void OnClic_StopAudioPreview()
    {
        SpotifyPreviewAudioManager.instance.Pause();

        challengecoloranimation.PauseAnimation();



    }

    public void OnClickForcePausePreview(){

        SpotifyPreviewAudioManager.instance.ForcePause();
        challengecoloranimation.ForcePauseAnimation();

    }
  
    public void OnClick_OpenPlaylist()
    {
        Surf.gameObject.SetActive(false);
        AppManager.instance.yours = true;
        InvokeEvent<ChangeColorAppEvent>(new ChangeColorAppEvent(gray, Color.black));
        NewScreenManager.instance.ChangeToSpawnedView("surfMiPlaylist");
    }

    public void SetSurfManager(GameObject _surf){
        Surf = _surf;
    }


    public void AddToPlaylistButton(string _trackid, float _time)
    {
        if (!isRecommended)
        {
            
            SpotifyConnectionManager.instance.AddItemsToPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, uris, Callback_AddToPlaylist);
            trackId = _trackid;
            time = _time;
            
        }
        else
        {
            SpotifyConnectionManager.instance.RemoveItemsFromPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, uris, Callback_RemoveToPlaylist);
            if (AppManager.instance.isLogInMode && !trackID.Equals(""))
                MwsiveConnectionManager.instance.PostTrackAction(trackID, "NOT_RECOMMEND", _time, AppManager.instance.GetCurrentPlaylist().id, Callback_PostTrackActionNORecomend);
        }
    }

    private void Callback_PostTrackActionRecomend(object[] _value)
    {
        Debug.Log("RECOMMEND CALLBACK");
        gameObject.GetComponentInParent<ButtonSurfPlaylist>().PlusOrLessOne(true, "RECOMMEND");
        isRecommended = true;
    }

    private void Callback_PostTrackActionNORecomend(object[] _value)
    {
        Debug.Log("NOT_RECOMMEND CALLBACK");
        gameObject.GetComponentInParent<ButtonSurfPlaylist>().PlusOrLessOne(false, "RECOMMEND");
        isRecommended = false;
    }

    public void AddToPlaylistSwipe(string _trackid, float _time)
    {
        if (!isRecommended)
        {

            SpotifyConnectionManager.instance.AddItemsToPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, uris, Callback_AddToPlaylist);
            if (AppManager.instance.isLogInMode && !trackID.Equals(""))
                MwsiveConnectionManager.instance.PostTrackAction(_trackid, "RECOMMEND", _time, AppManager.instance.GetCurrentPlaylist().id, Callback_PostTrackActionRecomend);
        }
    }

    private void Callback_AddToPlaylist(object[] _value)
    {
        string webcode = ((long)_value[0]).ToString();
        if(webcode == "404" || webcode == "403")
        {
            UIMessage.instance.UIMessageInstanciate("Playlist no propia o inexistente");
            AppManager.instance.yours = false;
            InvokeEvent<ChangeColorAppEvent>(new ChangeColorAppEvent(redNew, redNew));
        }
        else
        {
            InvokeEvent<ChangeColorAppEvent>(new ChangeColorAppEvent(gray, Color.black));
            if (AppManager.instance.isLogInMode && !trackID.Equals(""))
                MwsiveConnectionManager.instance.PostTrackAction(trackId, "RECOMMEND", time, AppManager.instance.GetCurrentPlaylist().id, Callback_PostTrackActionRecomend); ;
           AppManager.instance.RefreshCurrentPlaylistInformation((_list) => {
              mwsiveButton.ChangeAddToPlaylistButtonColor(0.5f);
              UIMessage.instance.UIMessageInstanciate("Canción agregada a la playlist");
           });
           AppManager.instance.yours = true;
           
        }

    }

    private void Callback_RemoveToPlaylist(object[] _value)
    {
        AppManager.instance.RefreshCurrentPlaylistInformation((_list) => {
            mwsiveButton.AddToPlaylistButtonColorButtonColorAgain(0.5f);
            UIMessage.instance.UIMessageInstanciate("Canción eliminada de la playlist");
        });
    }


    public void SelectedPlaylistNameEventListener(SelectedPlaylistNameAppEvent _event)
    {
        playlistText.text = _event.playlistName;
        if (isRecommended)
        {
            mwsiveButton.ChangeAddToPlaylistButtonColor(0.5f);
        }
        else
        {
            mwsiveButton.AddToPlaylistButtonColorButtonColorAgain(0.5f);
        }
        
    }

    public void OnClick_PlayOnSpotify()
    {
        Application.OpenURL(externalURL);
    }

    public void ChangeEventListener(ChangeColorAppEvent _event)
    {
        if (!AppManager.instance.yours)
        {
            buttonColor.GetComponent<Image>().color = redNew;
            playlistText.color = redNew;
        }
        else
        {
            buttonColor.GetComponent<Image>().color = gray;
            playlistText.color = Color.black;
        }
    }

    public void Callback_GetTrackInformation(object[] _value)
    {
        trackInfoRoot = (TrackInfoRoot)_value[1];

        if (AppManager.instance.isLogInMode)
        {
            if(trackInfoRoot.top_curators != null)
            {
                for(int i = 0; i < trackInfoRoot.top_curators.Count; i++)
                {
                    ImageManager.instance.GetImage(trackInfoRoot.top_curators[i].image, topCuratorImages[i], (RectTransform)this.transform, "PROFILEIMAGE");
                }
            }
        }

        isPicked = trackInfoRoot.is_piked;
        isRecommended = trackInfoRoot.is_recommended;

        if (trackInfoRoot.is_piked)
        {
            Debug.Log(trackInfoRoot.is_piked);
            mwsiveButton.OnClickOlaButton(.5f, trackID);
        }

        if (trackInfoRoot.is_recommended)
        {
            mwsiveButton.ChangeAddToPlaylistButtonColor(.5f);
        }
        

        CalculateKorM(trackInfoRoot.total_piks, trackTotalPicks);
        CalculateKorM(trackInfoRoot.total_recommendations, trackTotalRecommendation);
        CalculateKorM(trackInfoRoot.total_piks_followed, trackTopCuratorsThatVoted, " amigos también votaron por \r\nesta canción");

        
    }

    public void OnClick_UserThatVoted()
    {
        NewScreenManager.instance.ChangeToSpawnedView("usuariosQueVotaron");
        NewScreenManager.instance.GetCurrentView().GetComponent<UsersThatVotedViewModel>().Initialize(trackID);
    }

    private void CalculateKorM(int? _numberOfVotes, TextMeshProUGUI _textMeshProUGUI, string _text = null)
    {
        Debug.Log(_numberOfVotes.ToString());
        if(_numberOfVotes == null) { _numberOfVotes = 0; }
        if (_numberOfVotes >= THOUSEND_CONVERT_TO_K && _numberOfVotes < MILLION_CONVERT_TO_M)
        {
            if(_text != null)
            {
                _textMeshProUGUI.text = ((float)_numberOfVotes / (float)THOUSEND_CONVERT_TO_K).ToString() + "K" + _text;
            }
            else
            {
                _textMeshProUGUI.text = ((float)_numberOfVotes / (float)THOUSEND_CONVERT_TO_K).ToString() + "K";
            }
            

        }
        else if (_numberOfVotes > THOUSEND_CONVERT_TO_K && _numberOfVotes >= MILLION_CONVERT_TO_M)
        {
            if(_text != null)
            {
                _textMeshProUGUI.text = ((float)_numberOfVotes / (float)MILLION_CONVERT_TO_M).ToString() + "M" + _text;
            }
            else
            {
                _textMeshProUGUI.text = ((float)_numberOfVotes / (float)MILLION_CONVERT_TO_M).ToString() + "M";
            }
        }
        else
        {
            if(_text != null)
            {
                _textMeshProUGUI.text = _numberOfVotes.ToString() + _text;
            }
            else
            {
                _textMeshProUGUI.text = _numberOfVotes.ToString();
            }
        }
    }


    public void PlusOrLessOne(bool _value, string _type)
    {
        if(_type == "PIK")
        {
            if (_value)
            {
                trackInfoRoot.total_piks++;
                CalculateKorM(trackInfoRoot.total_piks, trackTotalPicks);
            }
            else
            {
                if(trackInfoRoot.total_piks > 0)
                {
                    trackInfoRoot.total_piks--;
                    CalculateKorM(trackInfoRoot.total_piks, trackTotalPicks);
                }
                
            }
            
        }

        if(_type == "RECOMMEND")
        {
            if (_value)
            {
                trackInfoRoot.total_recommendations++;
                CalculateKorM(trackInfoRoot.total_recommendations, trackTotalRecommendation);
            }
            else
            {
                if(trackInfoRoot.total_recommendations > 0)
                {
                    trackInfoRoot.total_recommendations--;
                    CalculateKorM(trackInfoRoot.total_recommendations, trackTotalRecommendation);
                }
                
            }
        }
    }
}