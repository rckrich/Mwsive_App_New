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
    public Sprite MwsiveCover;
    private bool isTrackinfoEnd = false, isPreviewSongFinishToLoad = false, isImageManagerLoad = false;
    private float time;

    public bool SuccesfulEnded = false;
    public bool AmILastPosition = false;
    private TrackInfoRoot trackInfoRoot;

    private bool isPicked = false;
    private bool isRecommended = false;


    private void Start()
    {
        
    }

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

    public void InitializeMwsiveDB(MwsiveData _data)
    {
       
        if (_data.top_curators != null)
        {
            for (int i = 0; i < _data.top_curators.Count; i++)
            {
                ImageManager.instance.GetImage(_data.top_curators[i].image_url, topCuratorImages[i], (RectTransform)this.transform);
            }
        }
        CalculateKorM(_data.total_piks, trackTotalPicks);
        CalculateKorM(_data.total_recommendations, trackTotalRecommendation);
        CalculateKorM(_data.total_piks_followed, trackTopCuratorsThatVoted, " amigos también votaron por \r\nesta canción");


        
            
        if (_data.isRecommended)
        {
          mwsiveButton.ChangeAddToPlaylistButtonColor(0.5f);
            isRecommended = true;
            //Pintar de morado el que está en playlist
        }
        else
        {
            mwsiveButton.AddToPlaylistButtonClear();
        }
        
        if (_data.isPicked)
        {

            mwsiveButton.PIKButtonColorOn();

        }
        else
        {
            mwsiveButton.PIKButtonColorOff();
        }
    }


    public void InitializeMwsiveSong(MwsiveData _data)
    {
        durationBar.ResetFillAmount();
        if (_data.playlist_name != null)
        {
            playlistText.text = _data.playlist_name;
        }
        if (_data.song_name != null)
        {
            if (_data.song_name.Length > 27)
            {
                string _text2 = "";
                for (int i = 0; i < 27; i++)
                {
                    _text2 = _text2 + _data.song_name[i];
                }
                _text2 = _text2 + "...";
                trackName.text = _text2;
            }
            else
            {
                trackName.text = _data.song_name;
            }
        }

        if (_data.album_name != null)
        {
            albumName.text = _data.album_name;
        }
        if (_data.artists != null)
        {
            artistName.text = _data.artists;
        }

        if (_data.album_image_url != null)
        {
            trackCover.sprite = MwsiveCover;
            ImageManager.instance.GetImage(_data.album_image_url, trackCover, (RectTransform)this.transform, null, Callback_ImageManager);
        }

        if (_data.id != null)
        {
            trackID = _data.id;
        }

            if (_data.uri != null)
        {
            uris.Clear();
            uris.Add(_data.uri);
        }

        if (_data.preview_url != null)
        {
            previewURL = _data.preview_url;
        }

        if (_data.external_url != null)
        {
            externalURL = _data.external_url;
        }
        TrackPoints = _data.challenge_trackpoints;

 

        if (_data.challenge_AmILastPosition)
        {
            AmILastPosition = true;
        }

        SuccesfulEnded = _data.challenge_songeded;

        isTrackinfoEnd = true;
        DisableAnimation();


    }

    public void ClearMwsiveButtons()
    {
        mwsiveButton.AddToPlaylistButtonClear();
        mwsiveButton.PIKButtonColorOff();
    }

    public void ClearData()
    {
        
        playlistText.text = null;       
        trackName.text = null;       
        albumName.text = null;
        artistName.text = null;
        trackCover.sprite = MwsiveCover;
        trackID = null;
        isRecommended = false;
        mwsiveButton.AddToPlaylistButtonClear();
        mwsiveButton.PIKButtonColorOff();
        challengecoloranimation.ForceClear();
        uris.Clear();
        previewURL = null;
        externalURL = null;
        TrackPoints = false;
        trackTotalPicks.text = "-";
        trackTotalRecommendation.text = "-";
        trackTopCuratorsThatVoted.text = "-";
    }


    public void UpdateData()
    {

    }

    public void LastPosition(){
        if(SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().GetCurrentMwsiveData().challenge_AmILastPosition){
            SuccesfulEnded = true;
            SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().CheckChallengeEnd();
        }
    }
    
    public void PlayAudioPreview()
    {
        try
        {
            if(SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().GetCurrentMwsiveData().id == trackID && TrackPoints)
            {
                durationBar.CheckforPoints = true;
            }
            else
            {
                durationBar.CheckforPoints = false;
            }
        }
        catch (System.NullReferenceException)
        {
            
            if (SurfController.instance.ReturnCurrentView().GetComponent<SurfManager>().GetCurrentMwsiveData().id == trackID && TrackPoints)
            {
                durationBar.CheckforPoints = true;
            }
            else
            {
                durationBar.CheckforPoints = false;
            }
            
        }
        
        SpotifyPreviewAudioManager.instance.GetTrack(previewURL, Callback_GetTrack);
    }

    private void Callback_GetTrack(object[] _list)
    {
        CheckIfDurationBarCanPlay();
        isPreviewSongFinishToLoad = true;

        DisableAnimation();

        if (SurfManager.instance.isActiveAndEnabled == false)
        {
            if (NewScreenManager.instance.GetCurrentView().GetViewID() != ViewID.SurfViewModel)
            {
                SpotifyPreviewAudioManager.instance.StopTrack();
            }
            
        }

        
    }

    public void CheckIfDurationBarCanPlay(){
        try
        {
            MwsiveData Current = SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().GetCurrentMwsiveData();
                if (Current.id == trackID)
                {
                    durationBar.canPlay = true;
                    if (Current.challenge_trackpoints)
                    {
                    
                        if (Current.challenge_songeded)
                        {
                            challengecoloranimation.CompleteAnimation();
                        }
                        else
                        {
                            challengecoloranimation.StartAnimation();
                        }
                    
                }
                }
                else
                {
                    durationBar.canPlay = false;
                }
            
            
        }
        catch (System.NullReferenceException)
        {
            
                if (SurfController.instance.ReturnCurrentView().GetComponent<SurfManager>().GetCurrentMwsiveData().id == trackID)
                {
                    durationBar.canPlay = true;
                    
                }
                else
                {
                    durationBar.canPlay = false;
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
            trackID = _trackid;
            time = _time;
            
        }
        else
        {
            isRecommended = false;
            SpotifyConnectionManager.instance.RemoveItemsFromPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, uris, Callback_RemoveToPlaylist);
            if (AppManager.instance.isLogInMode && !trackID.Equals(""))
                MwsiveConnectionManager.instance.PostTrackAction(trackID, "NOT_RECOMMEND", _time, AppManager.instance.GetCurrentPlaylist().id, Callback_PostTrackActionNORecomend);
        }
    }

    private void Callback_PostTrackActionRecomend(object[] _value)
    {
        gameObject.GetComponentInParent<ButtonSurfPlaylist>().PlusOrLessOne(true, "RECOMMEND");
    }

    private void Callback_PostTrackActionRecomendSwipe(object[] _value)
    {
        gameObject.GetComponentInParent<ButtonSurfPlaylist>().PlusOrLessOne(true, "RECOMMEND", true);
    }

    private void Callback_PostTrackActionNORecomend(object[] _value)
    {
        gameObject.GetComponentInParent<ButtonSurfPlaylist>().PlusOrLessOne(false, "RECOMMEND");
    }

    public void AddToPlaylistSwipe(string _trackid, float _time)
    {
        if (!isRecommended)
        {

            SpotifyConnectionManager.instance.AddItemsToPlaylist(ProgressManager.instance.progress.userDataPersistance.current_playlist, uris, Callback_AddToPlaylist);
            if (AppManager.instance.isLogInMode && !trackID.Equals(""))
                MwsiveConnectionManager.instance.PostTrackAction(_trackid, "RECOMMEND", _time, AppManager.instance.GetCurrentPlaylist().id, Callback_PostTrackActionRecomendSwipe);
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
            isRecommended = true;
            InvokeEvent<ChangeColorAppEvent>(new ChangeColorAppEvent(gray, Color.black));
            if (AppManager.instance.isLogInMode && !trackID.Equals(""))
                MwsiveConnectionManager.instance.PostTrackAction(trackID, "RECOMMEND", time, AppManager.instance.GetCurrentPlaylist().id, Callback_PostTrackActionRecomend); ;
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

    public void OnClick_UserThatVoted()
    {
        NewScreenManager.instance.ChangeToSpawnedView("usuariosQueVotaron");
        NewScreenManager.instance.GetCurrentView().GetComponent<UsersThatVotedViewModel>().Initialize(trackID);
    }

    private void CalculateKorM(int? _numberOfVotes, TextMeshProUGUI _textMeshProUGUI, string _text = null)
    {
        
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


    public void PlusOrLessOne(bool _value, string _type, bool isSwipe = false)
    {
        if(_type == "PIK")
        {
            if (_value)
            {
                int data;
                try
                {
                    data = SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().GetCurrentMwsiveData().total_piks++;

                }
                catch (System.NullReferenceException)
                {
                    data = SurfController.instance.ReturnCurrentView().GetComponent<SurfManager>().GetCurrentMwsiveData().total_piks++;
                }
                data++;
                CalculateKorM(data, trackTotalPicks);
            }
            else
            {
                
                int data;
                try
                {
                        data = SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().GetCurrentMwsiveData().total_piks--;

                } catch (System.NullReferenceException)
                {
                        data = SurfController.instance.ReturnCurrentView().GetComponent<SurfManager>().GetCurrentMwsiveData().total_piks--;
                }

                data--;

                if (data < 0)
                {
                    data = 0;
                }
                
                CalculateKorM(data, trackTotalPicks);
                
                
            }
            
        }

        if(_type == "RECOMMEND")
        {
            
            if (!isSwipe)
            {
                if (_value)
                {
                    
                    int data;
                    try
                    {
                        data = SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().GetCurrentMwsiveData().total_recommendations++;

                    }
                    catch (System.NullReferenceException)
                    {
                        data = SurfController.instance.ReturnCurrentView().GetComponent<SurfManager>().GetCurrentMwsiveData().total_recommendations++;
                    }
                    data++;
                    CalculateKorM(data, trackTotalRecommendation);
                }
                else
                {
                    
                    int data;
                    try
                    {
                        data = SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().GetCurrentMwsiveData().total_recommendations--;

                    }
                    catch (System.NullReferenceException)
                    {
                        data = SurfController.instance.ReturnCurrentView().GetComponent<SurfManager>().GetCurrentMwsiveData().total_recommendations--;
                    }
                    data--;
                    if (data < 0)
                    {
                        data = 0;
                    }

                    CalculateKorM(data, trackTotalRecommendation);


                }
            }
            else
            {
                if (_value)
                {
                    
                    int data;
                    try
                    {
                        data = SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().GetBeforeCurrentMwsiveData().total_recommendations++;

                    }
                    catch (System.NullReferenceException)
                    {
                        data = SurfController.instance.ReturnCurrentView().GetComponent<SurfManager>().GetBeforeCurrentMwsiveData().total_recommendations++;
                    }
                    data++;
                    CalculateKorM(data, trackTotalRecommendation);
                }
                else
                {
                    
                    int data;
                    try
                    {
                        data = SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().GetBeforeCurrentMwsiveData().total_recommendations--;

                    }
                    catch (System.NullReferenceException)
                    {
                        data = SurfController.instance.ReturnCurrentView().GetComponent<SurfManager>().GetBeforeCurrentMwsiveData().total_recommendations--;
                    }
                    data--;
                    if (data < 0)
                    {
                        data = 0;
                    }

                    CalculateKorM(data, trackTotalRecommendation);


                }
            }
            
        }
    }

    public void Callback_ImageManager(object[] value)
    {
        isImageManagerLoad = true;
        DisableAnimation();

    }

    private void DisableAnimation()
    {
       if(loadingAnimGameObject != null) {
            if(isTrackinfoEnd && isImageManagerLoad && isPreviewSongFinishToLoad)
                loadingAnimGameObject.SetActive(false);
       }
    }
}