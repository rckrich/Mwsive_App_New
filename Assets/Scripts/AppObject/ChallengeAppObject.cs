using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeAppObject : AppObject
{
    
    public TextMeshProUGUI disk;
    public Button challengeButton;
    public TextMeshProUGUI challengeName;
    public Image challengeImage;

    public Sprite challengeOnSprite;
    public Sprite challengeOffSprite;


    private SeveralTrackRoot severalTrackRoot;
    private Challenges challenges;
    private bool PointsPosted = false;

   

    public override void Initialize(params object[] list)
    {
        challengeButton.interactable = true;
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
        challengeButton.interactable = false;
        NewScreenManager.instance.ChangeToSpawnedView("surf");
        List<string> tracks = new List<string>();
        foreach (MwsiveTrack item in challenges.mwsive_tracks)
            {
                tracks.Add(item.spotify_track_id);
            }
        SpotifyConnectionManager.instance.GetSeveralTracks(tracks.ToArray(), OpenChallengeCallBack);
    }

    public void OpenChallengeCallBack(object[] _value){
        SeveralTrackRoot severalTrackRoot = (SeveralTrackRoot)_value[1];
      
        
        NewScreenManager.instance.GetCurrentView().GetComponent<PF_SurfViewModel>().Initialize();
        try
        {
            NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().Challenge = true;
            NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().SetChallengeCallback(gameObject.GetComponent<ChallengeAppObject>());
            NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().DynamicPrefabSpawnerSeveralTracks(severalTrackRoot.tracks);
            challengeButton.interactable = true;
        }
        catch (System.NullReferenceException)
        {
            challengeButton.interactable = true;
        }


    }


    public void CheckForPoints(){
        List<MwsiveData> _list = NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().GetInstances();
        float counter = 0;
        for (int i = 0; i < _list.Count-3; i++)
        {
            if(_list[i].challenge_songeded){
                counter++;
            }
        }

        if(counter >= (_list.Count-3)*.9f && !PointsPosted){
            PointsPosted = true;

            if(AppManager.instance.isLogInMode){
                MwsiveConnectionManager.instance.PostChallengeComplete(challenges.id, Callback_PostChallengeComplete);

            }else{
                CallPopUP(PopUpViewModelTypes.OptionChoice, "Neceseitas permiso", "Necesitas crear una cuenta de Mwsive para poder ganar los disks que obtuviste, presiona Crear Cuenta para hacer una.", "Crear Cuenta");
                PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);

                popUpViewModel.SetPopUpCancelAction(() => {
                    NewScreenManager.instance.BackToPreviousView();
                });

                popUpViewModel.SetPopUpAction(() => {
                    LogInManager.instance.StartLogInProcess(Callback_NoLogIn_PostChallengeComplete);
                    NewScreenManager.instance.BackToPreviousView();
                });

            }
        }
    }

    private void Callback_NoLogIn_PostChallengeComplete(object[] value)
    {
        AppManager.instance.StartAppProcessFromOutside();
        MwsiveConnectionManager.instance.PostChallengeComplete(challenges.id, Callback_PostChallengeComplete);
    }

    private void Callback_PostChallengeComplete(object[] value){
        MwsiveCompleteChallengesRoot mwsiveCompleteChallengesRoot = (MwsiveCompleteChallengesRoot)value[1];

        if(mwsiveCompleteChallengesRoot.disks == null)
        {

            UIMessage.instance.UIMessageInstanciate("Ya haz completado este Challenge");
        }
        else
        {
            UIMessage.instance.UIMessageInstanciate("Desafio Completado." + "Haz obtenido " + mwsiveCompleteChallengesRoot.disks + " disks.");
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

    
}
