using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MwsiveButton : AppObject
{
    public GameObject OlaColorButton;
    private bool IsItOlaColorButtonActive = false;
    public GameObject AddColorButton;
    private bool IsiTAddColorButtonActive = false;
    public GameObject CompartirColorButton;
    private bool IsItCompartirColorButtonActive = false;

    private const int PIK_PRICE = 1;

    private float AnimationDuration = .5f;

    public void OnClickOlaButton(float _AnimationDuration, string _trackid, float _time = -1)
    {
        AnimationDuration = _AnimationDuration;
        if (!IsItOlaColorButtonActive)
        {
            if (AppManager.instance.isLogInMode && !_trackid.Equals(""))
            {
                if (_time > -1)
                {
                    if (AppManager.instance.currentMwsiveUser.total_disks >= PIK_PRICE)
                    {
                        MwsiveConnectionManager.instance.PostTrackAction(_trackid, "PIK", _time, null, Callback_TrackActionPIK);
                    }
                    else
                    {
                        UIMessage.instance.UIMessageInstanciate("Completa retos para conseguir mas disks");
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
                    MwsiveConnectionManager.instance.PostTrackAction(_trackid, "UNPIK", _time, null, Callback_TrackActionUNPIK);

                }

            }
        }
    }

    public void PIKOnNoAni()
    {
        IsItOlaColorButtonActive = true;
        OlaColorButton.SetActive(true);
        OlaColorButton.GetComponent<CanvasGroup>().alpha = 1;
        OlaColorButton.transform.localScale = new Vector3(1f, 1f, 1f);

    }


    public void UnPIKNoAni()
    {
        IsItOlaColorButtonActive = false;
        OlaColorButton.SetActive(false);
        OlaColorButton.GetComponent<CanvasGroup>().alpha = 0;
        OlaColorButton.transform.localScale = new Vector3(0, 0, 0);
    }

    public void PIKButtonColorOn()
    {
        UIAniManager.instance.FadeIn(OlaColorButton, AnimationDuration);
        OlaColorButton.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .3f).OnComplete(() => { OlaColorButton.transform.DOScale(new Vector3(1f, 1f, 1f), .3f); });
        IsItOlaColorButtonActive = true;
    }

    public void PIKButtonColorOff()
    {
        UIAniManager.instance.FadeOut(OlaColorButton, AnimationDuration);
        IsItOlaColorButtonActive = false;
        OlaColorButton.transform.DOScale(new Vector3(0f, 0f, 0f), .3f);
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
        }

        return char.ToUpper(fixedBadgeGroup.ToCharArray()[0]) + fixedBadgeGroup.Substring(1);
    }

    private void Callback_TrackActionUNPIK(object[] _value)
    {
        PIKButtonColorOff();
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

    public bool GetIsItOlaActive()
    {
        return IsItOlaColorButtonActive;
    }

    public bool GetIsItCompartirActive()
    {
        return IsItCompartirColorButtonActive;
    }

    public void ChangeAddToPlaylistButtonColor(float _AnimationDuration)
    {
        if (!IsiTAddColorButtonActive)
        {
            UIAniManager.instance.FadeIn(AddColorButton, _AnimationDuration);
            AddColorButton.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .3f).OnComplete(() => { AddColorButton.transform.DOScale(new Vector3(1f, 1f, 1f), .3f); });
            IsiTAddColorButtonActive = true;
        }
    }

    public void AddToPlaylistButtonClear()
    {
        IsiTAddColorButtonActive = false;
        AddColorButton.GetComponent<CanvasGroup>().alpha = 0;
        AddColorButton.SetActive(false);
        AddColorButton.transform.localScale = new Vector3(0f, 0f, 0f);
    }

    public void AddToPlaylistButtonColorButtonColorAgain(float _AnimationDuration)
    {
        UIAniManager.instance.FadeOut(AddColorButton, _AnimationDuration);
        AddColorButton.transform.DOScale(new Vector3(0f, 0f, 0f), .3f);
        IsiTAddColorButtonActive = false;

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
        if (!IsItCompartirColorButtonActive)
        {

            UIAniManager.instance.FadeIn(CompartirColorButton, _AnimationDuration);
            CompartirColorButton.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .3f).OnComplete(() => { CompartirColorButton.transform.DOScale(new Vector3(1f, 1f, 1f), .3f); });
            IsItCompartirColorButtonActive = true;
        }
        else
        {
            UIAniManager.instance.FadeOut(CompartirColorButton, _AnimationDuration);
            CompartirColorButton.transform.DOScale(new Vector3(0f, 0f, 0f), .3f);
            IsItCompartirColorButtonActive = false;
        }
    }
}
