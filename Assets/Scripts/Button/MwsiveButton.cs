using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MwsiveButton : AppObject
{
    public GameObject PIKColorButton, PIKContainer;
    private bool IsItPIKColorButtonActive = false;
    public GameObject RecommendColorButton, RecommendContainer;
    private bool IsiTRecommendColorButtonActive = false;
    public GameObject ShareColorButton, ShareContainer;
    private bool IsItShareColorButtonActive = false;

    private float size = .65f;
    

    public bool PIKCallbackEnd = true;

    private void Start()
    {
        
    }


    private const int PIK_PRICE = 1;

    private float AnimationDuration = .5f;

    public void ChangeSizeForSpawn()
    {
        Debug.Log("spawn");
        PIKContainer.transform.localScale = new Vector3(.85f, .85f, .85f);
        RecommendContainer.transform.localScale = new Vector3(size, size, size);
        ShareContainer.transform.localScale = new Vector3(size, size, size);

    }

    public void ChangeSizeToMain()
    {
        Debug.Log("Main");
        PIKContainer.transform.localScale = new Vector3(1,1,1);
        RecommendContainer.transform.localScale = new Vector3(1, 1, 1);
        ShareContainer.transform.localScale = new Vector3(1, 1, 1);

    }

    public void OnClickOlaButton(float _AnimationDuration, string _trackid, float _time = -1)
    {
        
        if (PIKCallbackEnd)
        {
            
            AnimationDuration = _AnimationDuration;
            PIKCallbackEnd = false;
            if (!IsItPIKColorButtonActive)
            {
                if (AppManager.instance.isLogInMode && !_trackid.Equals(""))
                {
                    if (_time > -1)
                    {
                        if (AppManager.instance.currentMwsiveUser.total_disks >= PIK_PRICE)
                        {
                            if (NewScreenManager.instance.TryGetComponent<PF_SurfManager>(out PF_SurfManager pF_SurfManager))
                            {
                                int challenge_id = pF_SurfManager.Challenge ? pF_SurfManager.challenge_id : -1;
                                MwsiveConnectionManager.instance.PostTrackAction(_trackid, "PIK", _time, null, challenge_id, Callback_TrackActionPIK);
                            }
                            else
                            {
                                MwsiveConnectionManager.instance.PostTrackAction(_trackid, "PIK", _time, null, -1, Callback_TrackActionPIK);
                            }
                        }
                        else
                        {

                            CallPopUP(PopUpViewModelTypes.MessageOnly, "Disks insuficientes", "Ups, te quedaste sin PIKCoins, juega desafíos para ganar más y hacer PIKs", "Aceptar");
                            PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);



                            popUpViewModel.SetPopUpAction(() => {

                                NewScreenManager.instance.BackToPreviousView();
                                
                            });

                            PIKCallbackEnd = true;
                        }
                    }
                    else
                    {
                        PIKButtonColorOn();
                    }
                }
            }
            else
            {
                if (_time > -1)
                {
                    if (AppManager.instance.isLogInMode && !_trackid.Equals(""))
                    {
                        if (NewScreenManager.instance.TryGetComponent<PF_SurfManager>(out PF_SurfManager pF_SurfManager))
                        {
                            int challenge_id = pF_SurfManager.Challenge ? pF_SurfManager.challenge_id : -1;
                            MwsiveConnectionManager.instance.PostTrackAction(_trackid, "UNPIK", _time, null, challenge_id, Callback_TrackActionPIK);
                        }
                        else
                        {
                            MwsiveConnectionManager.instance.PostTrackAction(_trackid, "UNPIK", _time, null, -1, Callback_TrackActionUNPIK);
                        }

                    }

                }
            }
        }

    }

    public void PIKOnNoAni()
    {
        
        DOTween.Kill(PIKColorButton);
        IsItPIKColorButtonActive = true;
        PIKColorButton.SetActive(true);
        PIKColorButton.GetComponent<CanvasGroup>().alpha = 1;
        PIKColorButton.transform.localScale = new Vector3(1f, 1f, 1f);

    }


    public void UnPIKNoAni()
    {
        DOTween.Kill(PIKColorButton);
        IsItPIKColorButtonActive = false;
        PIKColorButton.SetActive(false);
        PIKColorButton.GetComponent<CanvasGroup>().alpha = 0;
        PIKColorButton.transform.localScale = new Vector3(0, 0, 0);
    }

    public void PIKButtonColorOn()
    {
        try
        {
            SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().PIKAnimation();
        }
        catch (System.NullReferenceException)
        {
            SurfController.instance.ReturnCurrentView().GetComponent<SurfManager>().PIKAnimation();
        }

        DOTween.Kill(PIKColorButton);
        UIAniManager.instance.FadeIn(PIKColorButton, AnimationDuration);
        PIKColorButton.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .3f).OnComplete(() => { PIKColorButton.transform.DOScale(new Vector3(1f, 1f, 1f), .3f); });
        IsItPIKColorButtonActive = true;
    }

    public void PIKButtonColorOff()
    {
        DOTween.Kill(PIKColorButton);
        UIAniManager.instance.FadeOut(PIKColorButton, AnimationDuration);
        IsItPIKColorButtonActive = false;
        PIKColorButton.transform.DOScale(new Vector3(0f, 0f, 0f), .3f);
    }

    private void Callback_TrackActionPIK(object[] _value)
    {
        RootTrackAction rootTrackAction = (RootTrackAction)_value[1];

        if (rootTrackAction.badges != null)
        {
            foreach (Badge badge in rootTrackAction.badges)
            {
                if (badge.type.Equals("engagement"))
                {
                   
                    UIMessage.instance.UIMessageInstanciate("Conseguiste la insignia " + FixedBadgeGroupString(badge.group));
                    
                }
                else if (badge.type.Equals("track"))
                {
                    UIMessage.instance.UIMessageInstanciate("Conseguiste la insignia Top #" + badge.group + " de esta canción");
                }
            }
        }

        InvokeEvent<ChangeDiskAppEvent>(new ChangeDiskAppEvent(rootTrackAction.disks, "SUBSTRACT"));

        PIKButtonColorOn();
        PIKCallbackEnd = true;
        
        gameObject.GetComponentInParent<ButtonSurfPlaylist>().PlusOrLessOne(true, "PIK");

    }

    private string FixedBadgeGroupString(string _value)
    {
        string fixedBadgeGroup = _value;

        switch (fixedBadgeGroup)
        {
            case "song_master":
                return "Song Master";
            case "wave_master":
                return "Wave Master";
            case "music_master":
                return "Music Master";
            case "track_master":
                return "Track Master";
        }

        return char.ToUpper(fixedBadgeGroup.ToCharArray()[0]) + fixedBadgeGroup.Substring(1);
    }

    private void Callback_TrackActionUNPIK(object[] _value)
    {
        PIKButtonColorOff();
        PIKCallbackEnd = true;
        gameObject.GetComponentInParent<ButtonSurfPlaylist>().PlusOrLessOne(false, "PIK");

        try
        {
            SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().GetCurrentMwsiveData().isPicked = false;

        }
        catch (System.NullReferenceException)
        {
            SurfController.instance.ReturnCurrentView().GetComponent<SurfManager>().GetCurrentMwsiveData().isPicked = false;

        }
    }

    private void CallPopUP(PopUpViewModelTypes _type, string _titleText, string _descriptionText, string _actionButtonText = "")
    {

        NewScreenManager.instance.ChangeToMainView(ViewID.PopUpViewModel, true);
        PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(_type, _titleText, _descriptionText, _actionButtonText);
        popUpViewModel.SetPopUpAction(() => { NewScreenManager.instance.BackToPreviousView(); });


    }

    public bool GetIsItOlaActive()
    {
        return IsItPIKColorButtonActive;
    }

    public bool GetIsItCompartirActive()
    {
        return IsItShareColorButtonActive;
    }

    public void ChangeAddToPlaylistButtonColor(float _AnimationDuration, bool swipe)
    {
        if (!IsiTRecommendColorButtonActive)
        {
            try
            {
                SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().AddSongAnimation(!swipe);
            }
            catch (System.NullReferenceException)
            {
                SurfController.instance.ReturnCurrentView().GetComponent<SurfManager>().AddSongAnimations(!swipe);
            }
            

            UIAniManager.instance.FadeIn(RecommendColorButton, _AnimationDuration);
            RecommendColorButton.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .3f).OnComplete(() => { RecommendColorButton.transform.DOScale(new Vector3(1f, 1f, 1f), .3f); });
            IsiTRecommendColorButtonActive = true;
        }
    }

    public void AddToPlaylistButtonNoAni()
    {
        DOTween.Kill(RecommendColorButton);
        IsiTRecommendColorButtonActive = true;
        RecommendColorButton.GetComponent<CanvasGroup>().alpha = 1;
        RecommendColorButton.SetActive(true);
        RecommendColorButton.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void AddToPlaylistButtonClear()
    {
        DOTween.Kill(RecommendColorButton);
        IsiTRecommendColorButtonActive = false;
        
        RecommendColorButton.GetComponent<CanvasGroup>().alpha = 0;
        RecommendColorButton.SetActive(false);
        RecommendColorButton.transform.localScale = new Vector3(0f, 0f, 0f);
    }

    public void AddToPlaylistButtonColorButtonColorAgain(float _AnimationDuration)
    {
        UIAniManager.instance.FadeOut(RecommendColorButton, _AnimationDuration);
        RecommendColorButton.transform.DOScale(new Vector3(0f, 0f, 0f), .3f);
        IsiTRecommendColorButtonActive = false;

        try
        {
            SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().GetCurrentMwsiveData().isRecommended = false;
        }
        catch (System.NullReferenceException)
        {
            SurfController.instance.ReturnCurrentView().GetComponent<SurfManager>().GetCurrentMwsiveData().isRecommended = false;

        }

    }

    public void OnClickCompartirButton(float _AnimationDuration)
    {
        if (!IsItShareColorButtonActive)
        {

            UIAniManager.instance.FadeIn(ShareColorButton, _AnimationDuration);
            ShareColorButton.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .3f).OnComplete(() => { ShareColorButton.transform.DOScale(new Vector3(1f, 1f, 1f), .3f); });
            IsItShareColorButtonActive = true;
        }
        else
        {
            UIAniManager.instance.FadeOut(ShareColorButton, _AnimationDuration);
            ShareColorButton.transform.DOScale(new Vector3(0f, 0f, 0f), .3f);
            IsItShareColorButtonActive = false;
        }
    }
}
