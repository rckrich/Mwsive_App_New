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

    

    private const int PIK_PRICE = 10;

    private float AnimationDuration = .5f;

    public void OnClickOlaButton(float _AnimationDuration, string _trackid, float _time = -1){
        AnimationDuration = _AnimationDuration;
        if(!IsItOlaColorButtonActive){
            if (AppManager.instance.isLogInMode && !_trackid.Equals(""))
            {
                if(_time > -1)
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
                    UIAniManager.instance.FadeIn(OlaColorButton, AnimationDuration);
                    OlaColorButton.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .3f).OnComplete(() => { OlaColorButton.transform.DOScale(new Vector3(1f, 1f, 1f), .3f); });
                    IsItOlaColorButtonActive = true;
                }           
            }        
        }else{
            if(_time > -1)
            {
                if (AppManager.instance.isLogInMode && !_trackid.Equals(""))
                {
                    MwsiveConnectionManager.instance.PostTrackAction(_trackid, "UNPIK", _time, null, Callback_TrackActionUNPIK);
                }

            }
        }  
    }


    private void Callback_TrackActionPIK(object[] _value)
    {
        RootTrackAction rootTrackAction = (RootTrackAction)_value[1];
        Debug.Log(rootTrackAction);
        InvokeEvent<ChangeDiskAppEvent>(new ChangeDiskAppEvent(rootTrackAction.disks, "SUBSTRACT"));

        UIAniManager.instance.FadeIn(OlaColorButton, AnimationDuration);
        OlaColorButton.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .3f).OnComplete(() => { OlaColorButton.transform.DOScale(new Vector3(1f, 1f, 1f), .3f); });
        IsItOlaColorButtonActive = true;

        gameObject.GetComponentInParent<ButtonSurfPlaylist>().PlusOrLessOne(true, "PIK");

    }

    private void Callback_TrackActionUNPIK(object[] _value)
    {
        UIAniManager.instance.FadeOut(OlaColorButton, AnimationDuration);
        IsItOlaColorButtonActive = false;
        OlaColorButton.transform.DOScale(new Vector3(0f, 0f, 0f), .3f);
        gameObject.GetComponentInParent<ButtonSurfPlaylist>().PlusOrLessOne(false, "PIK");
    }



    public bool GetIsItOlaActive(){
        return IsItOlaColorButtonActive;
    }

    public bool GetIsItCompartirActive(){
        return IsItCompartirColorButtonActive;
    }

    public void ChangeAddToPlaylistButtonColor(float _AnimationDuration){
        if(!IsiTAddColorButtonActive){
            UIAniManager.instance.FadeIn(AddColorButton, _AnimationDuration);
            AddColorButton.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .3f).OnComplete(() => {AddColorButton.transform.DOScale(new Vector3(1f, 1f, 1f), .3f);});
            IsiTAddColorButtonActive = true;
        } 
    }

    public void AddToPlaylistButtonColorButtonColorAgain(float _AnimationDuration) 
    {    
      UIAniManager.instance.FadeOut(AddColorButton, _AnimationDuration);
      AddColorButton.transform.DOScale(new Vector3(0f, 0f, 0f), .3f);
      IsiTAddColorButtonActive = false;        
    }

    public void OnClickCompartirButton(float _AnimationDuration){
        if(!IsItCompartirColorButtonActive){
            
            UIAniManager.instance.FadeIn(CompartirColorButton, _AnimationDuration);
            CompartirColorButton.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .3f).OnComplete(() => {CompartirColorButton.transform.DOScale(new Vector3(1f, 1f, 1f), .3f);});
            IsItCompartirColorButtonActive = true;
        }else{
            UIAniManager.instance.FadeOut(CompartirColorButton, _AnimationDuration);
            CompartirColorButton.transform.DOScale(new Vector3(0f, 0f, 0f), .3f);
            IsItCompartirColorButtonActive = false;
        }  
    }
}
