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
    public TextMeshProUGUI friendCuratorsText;
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
    public bool SuccesfulEnded = false;
    public bool AmILastPosition = false;
    private bool isPicked = false;
    private bool isRecommended = false;
    private TrackInfoRoot trackInfoRoot;


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
            if (AppManager.instance.SearchTrackOnCurrentPlaylist(_spotifyid))
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
            MwsiveConnectionManager.instance.GetTrackInformation_Auth(trackID, Callback_GetTrackInformation);
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
        

       
        
    }

    public void OnClickForcePausePreview(){
        SpotifyPreviewAudioManager.instance.ForcePause();
        

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
        if (!AppManager.instance.SearchTrackOnCurrentPlaylist(trackID))
        {
            
            SpotifyConnectionManager.instance.AddItemsToPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, uris, Callback_AddToPlaylist);
            if (AppManager.instance.isLogInMode && !trackID.Equals(""))
                MwsiveConnectionManager.instance.PostTrackAction(_trackid, "RECOMMEND", _time, Callback_PostTrackActionRecomend); ;
        }
        else
        {
            SpotifyConnectionManager.instance.RemoveItemsFromPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, uris, Callback_RemoveToPlaylist);
            if (AppManager.instance.isLogInMode && !trackID.Equals(""))
                MwsiveConnectionManager.instance.PostTrackAction(trackID, "NOT_RECOMMEND", _time, Callback_PostTrackActionNORecomend);
        }
    }

    private void Callback_PostTrackActionRecomend(object[] _value)
    {
        Debug.Log("RECOMMEND CALLBACK");
        gameObject.GetComponentInParent<ButtonSurfPlaylist>().PlusOrLessOne(true, "RECOMMEND");
    }

    private void Callback_PostTrackActionNORecomend(object[] _value)
    {
        Debug.Log("NOT_RECOMMEND CALLBACK");
        gameObject.GetComponentInParent<ButtonSurfPlaylist>().PlusOrLessOne(false, "RECOMMEND");
    }

    public void AddToPlaylistSwipe(string _trackid, float _time)
    {
        if (!AppManager.instance.SearchTrackOnCurrentPlaylist(trackID))
        {

            SpotifyConnectionManager.instance.AddItemsToPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, uris, Callback_AddToPlaylist);
            if (AppManager.instance.isLogInMode && !trackID.Equals(""))
                MwsiveConnectionManager.instance.PostTrackAction(_trackid, "RECOMMEND", _time, Callback_PostTrackActionRecomend); ;
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
            AppManager.instance.RefreshCurrentPlaylistInformation((_list) => {
                mwsiveButton.ChangeAddToPlaylistButtonColor(0.5f);
                UIMessage.instance.UIMessageInstanciate("Canción agregada a la playlist");
            });

            AppManager.instance.yours = true;
            InvokeEvent<ChangeColorAppEvent>(new ChangeColorAppEvent(gray, Color.black));
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
        if (AppManager.instance.SearchTrackOnCurrentPlaylist(trackID))
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

        

        isPicked = trackInfoRoot.is_piked;
        isRecommended = trackInfoRoot.is_recommended;

        if (trackInfoRoot.is_piked)
        {
            Debug.Log(trackInfoRoot.is_piked);
            mwsiveButton.OnClickOlaButton(.5f, trackID);
        }

        CalculateKorM(trackInfoRoot.total_piks, trackTotalPicks);
        CalculateKorM(trackInfoRoot.total_recommendations, trackTotalRecommendation);
        CalculateKorM(trackInfoRoot.total_piks_followed, friendCuratorsText);

        
    }

    public void OnClick_UserThatVoted()
    {
        NewScreenManager.instance.ChangeToSpawnedView("usuariosQueVotaron");
        NewScreenManager.instance.GetCurrentView().GetComponent<UsersThatVotedViewModel>().Initialize(trackID);
    }

    private void CalculateKorM(int? _numberOfVotes, TextMeshProUGUI _textMeshProUGUI)
    {
        if(_numberOfVotes == null) { _numberOfVotes = 0; }
        if (_numberOfVotes >= THOUSEND_CONVERT_TO_K && _numberOfVotes < MILLION_CONVERT_TO_M)
        {
            _textMeshProUGUI.text = ((float)_numberOfVotes / (float)THOUSEND_CONVERT_TO_K).ToString() + "K";
        }
        else if (_numberOfVotes > THOUSEND_CONVERT_TO_K && _numberOfVotes >= MILLION_CONVERT_TO_M)
        {
            _textMeshProUGUI.text = ((float)_numberOfVotes / (float)MILLION_CONVERT_TO_M).ToString() + "M";
        }
        else
        {
            _textMeshProUGUI.text = _numberOfVotes.ToString();
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