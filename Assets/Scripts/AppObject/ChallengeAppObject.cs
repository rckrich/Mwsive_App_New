using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeAppObject : AppObject
{
    private const int NETWORKLOADINGPANEL_ANIMATION_LAYER = 0;
    private const float MINIMUM_SECONDS_FOR_END_SEARCH_PANEL = 1.0f;

    public TextMeshProUGUI disk;
    public Button challengeButton;
    public TextMeshProUGUI challengeName;
    public Image challengeImage;

    public Sprite challengeOnSprite;
    public Sprite challengeOffSprite;


    private SeveralTrackRoot severalTrackRoot;
    private Challenges challenges;
    private bool PointsPosted = false;
    private int registered_challenge_id;
    private GameObject networkLoadingCanvas;


    public override void Initialize(params object[] list)
    {
        ButtonInteractable(true);
        challenges = (Challenges)list[0];
        
        if (challenges.name.Length > 27)
        {
            string _text2 = "";
            for (int k = 0; k < 27; k++)
            {

                _text2 = _text2 + challenges.name[k];

            }
            _text2 = _text2 + "...";
            challengeName.text = _text2;
        }
        else
        {
            challengeName.text = challenges.name;
        }

        if(challengeImage != null)
        {
            bool isCompleted = (bool)list[1];
            challengeImage.sprite = isCompleted ? challengeOffSprite : challengeOnSprite;

        }

        disk.text = challenges.disks.ToString();
    }

    public void OnClick_OpenChallenge(){
        ButtonInteractable(false);
        NewScreenManager.instance.ChangeToSpawnedView("surf");

        MwsiveConnectionManager.instance.PostChallengeStarted(challenges.id, Callback_PostStartedChallenge);      
    }

    private void Callback_PostStartedChallenge(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        MwsiveStartedChallengeRoot mwsiveStartedChallengeRoot = (MwsiveStartedChallengeRoot)_value[1];

        registered_challenge_id = mwsiveStartedChallengeRoot.registered_challenge_id;

        List<string> tracks = new List<string>();
        foreach (MwsiveTrack item in challenges.mwsive_tracks)
        {
            tracks.Add(item.spotify_track_id);
        }
        SpotifyConnectionManager.instance.GetSeveralTracks(tracks.ToArray(), OpenChallengeCallBack);
    }

    public void OpenChallengeCallBack(object[] _value){
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        SeveralTrackRoot severalTrackRoot = (SeveralTrackRoot)_value[1];
        
        NewScreenManager.instance.GetCurrentView().GetComponent<PF_SurfViewModel>().Initialize();
        try
        {
            NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().Challenge = true;
            NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().challenge_id = challenges.id;
            NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().SetChallengeCallback(gameObject.GetComponent<ChallengeAppObject>());
            NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().DynamicPrefabSpawnerSeveralTracks(severalTrackRoot.tracks);
            ButtonInteractable(true);
        }
        catch (System.NullReferenceException)
        {
            ButtonInteractable(true);
        }
    }


    public void CheckForPoints(){
            if(AppManager.instance.isLogInMode){
                if(Application.internetReachability != NetworkReachability.NotReachable)
                {
                    StartCoroutine(CR_StartSearch());

                    MwsiveConnectionManager.instance.PostChallengeComplete(challenges.id, registered_challenge_id, Callback_PostChallengeComplete);
                }
                else
                {
                    CallPopUP(PopUpViewModelTypes.MessageOnly, "Ocurrió un error", "Por favor, intentalo de nuevo más tarde", "Salir");
                    PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
                    popUpViewModel.SetPopUpCancelAction(() => {

                        NewScreenManager.instance.BackToPreviousView();
                        NewScreenManager.instance.BackToPreviousView();
                    });
                }
                

            }else{
                CallPopUP(PopUpViewModelTypes.OptionChoice, "Neceseitas permiso", "Necesitas crear una cuenta de Mwsive para poder ganar los disks que obtuviste, presiona Crear Cuenta para hacer una.", "Crear Cuenta");
                PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);

                

                popUpViewModel.SetPopUpAction(() => {
                    LogInManager.instance.StartLogInProcess(Callback_NoLogIn_PostChallengeComplete);
                    NewScreenManager.instance.BackToPreviousView();
                });

            }
        
        
    }

    private void Callback_NoLogIn_PostChallengeComplete(object[] value)
    {
        AppManager.instance.StartAppProcessFromOutside();
        MwsiveConnectionManager.instance.PostChallengeComplete(challenges.id, registered_challenge_id, Callback_PostChallengeComplete);
    }

    private void Callback_PostChallengeComplete(object[] _value){

        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

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

        MwsiveCompleteChallengesRoot mwsiveCompleteChallengesRoot = (MwsiveCompleteChallengesRoot)_value[1];

        StartCoroutine(CR_EndSearch());

        if (mwsiveCompleteChallengesRoot.disks == null)
        {
            CallPopUP(PopUpViewModelTypes.OptionChoice, "Desafío Completo", "Ya haz completado este Desafío", "Salir");
            PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);

            popUpViewModel.SetPopUpAction(() => {

                NewScreenManager.instance.BackToPreviousView();
                NewScreenManager.instance.BackToPreviousView();
            });
            
        }
        else
        {
            CallPopUP(PopUpViewModelTypes.OptionChoice, "Desafío Completado", "Haz obtenido " + mwsiveCompleteChallengesRoot.disks + " disks.", "Salir");
            PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);

            popUpViewModel.SetPopUpAction(() => {

                NewScreenManager.instance.BackToPreviousView();
                NewScreenManager.instance.BackToPreviousView();
            });

        }
        

        AppManager.instance.RefreshUser();
    }

    private void CallPopUP(PopUpViewModelTypes _type, string _titleText, string _descriptionText, string _actionButtonText = "")
    {
        NewScreenManager.instance.ChangeToMainView(ViewID.PopUpViewModel, true);
        
        PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(_type, _titleText, _descriptionText, _actionButtonText);
        popUpViewModel.SetPopUpAction(() => { NewScreenManager.instance.BackToPreviousView(); });
    }

    
    public void ButtonInteractable(bool value) 
    {
        challengeButton.interactable = value;
    }

    private IEnumerator CR_StartSearch()
    {

        SurfController.instance.ReturnCurrentSurfViewModel().GetComponent<PF_SurfViewModel>().canGoBack = false;
        SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().canSwipe = false;
        SpotifyPreviewAudioManager.instance.audioSource.Play();
        
        if (networkLoadingCanvas == null)
        {
            networkLoadingCanvas = GameObject.FindWithTag("NetworkLoadingCanvas");
        }
            

        if (networkLoadingCanvas != null)
        {
            
            networkLoadingCanvas.transform.GetChild(0).gameObject.SetActive(true);
        }

        yield return null;
    }

    private IEnumerator CR_EndSearch()
    {
        SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().canSwipe = true;

        if (networkLoadingCanvas == null)
            networkLoadingCanvas = GameObject.FindWithTag("NetworkLoadingCanvas");

        if (networkLoadingCanvas != null)
        {
            SurfController.instance.ReturnCurrentSurfViewModel().GetComponent<PF_SurfViewModel>().canGoBack = true;
            GameObject networkLoadingPanel = networkLoadingCanvas.transform.GetChild(0).gameObject;
            networkLoadingPanel.SetActive(false);
            if (networkLoadingPanel.GetComponent<Animator>().GetCurrentAnimatorClipInfo(NETWORKLOADINGPANEL_ANIMATION_LAYER).Length > 0)
            {
                float animationLenght = networkLoadingPanel.GetComponent<Animator>().GetCurrentAnimatorClipInfo(NETWORKLOADINGPANEL_ANIMATION_LAYER)[0].clip.length;
                yield return new WaitForSeconds(animationLenght);
            }
            else
            {
                yield return new WaitForSeconds(MINIMUM_SECONDS_FOR_END_SEARCH_PANEL);
            }

        }
        yield return null;
    }
}
