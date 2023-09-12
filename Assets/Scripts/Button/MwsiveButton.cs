using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MwsiveButton : MonoBehaviour
{
    public GameObject OlaColorButton;
    private bool IsItOlaColorButtonActive = false;
    public GameObject AñadirColorButton;
    private bool IsItAñadirColorButtonActive = false;
    public GameObject CompartirColorButton;
    private bool IsItCompartirColorButtonActive = false;

    private const int PIK_PRICE = 10;

    private float AnimationDuration = .5f;

    public void OnClickOlaButton(float _AnimationDuration, string _trackid, float _time){
        AnimationDuration = _AnimationDuration;
        if(!IsItOlaColorButtonActive){
            if (AppManager.instance.isLogInMode && !_trackid.Equals(""))
            {
                if(AppManager.instance.currentMwsiveUser.total_disks >= PIK_PRICE)
                {
                    MwsiveConnectionManager.instance.PostTrackAction(_trackid, "PIK", _time, Callback_TrackActionPIK);
                }
                else
                {
                    UIMessage.instance.UIMessageInstanciate("Completa retos para conseguir mas disks");
                }
                
            }
                
            
        }else{
            if (AppManager.instance.isLogInMode && !_trackid.Equals(""))
            {
                MwsiveConnectionManager.instance.PostTrackAction(_trackid, "UNPIK", _time, Callback_TrackActionUNPIK);
            }
                
            
        }  
    }

    private void Callback_TrackActionPIK(object[] _value)
    {
        
        UIAniManager.instance.FadeIn(OlaColorButton, AnimationDuration);
        OlaColorButton.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .3f).OnComplete(() => { OlaColorButton.transform.DOScale(new Vector3(1f, 1f, 1f), .3f); });
        IsItOlaColorButtonActive = true;
        AppManager.instance.RefreshUser();
    }

    private void Callback_TrackActionUNPIK(object[] _value)
    {
        UIAniManager.instance.FadeOut(OlaColorButton, AnimationDuration);
        IsItOlaColorButtonActive = false;
        OlaColorButton.transform.DOScale(new Vector3(0f, 0f, 0f), .3f);
        
    }



    public bool GetIsItOlaActive(){
        return IsItOlaColorButtonActive;
    }

    public bool GetIsItCompartirActive(){
        return IsItCompartirColorButtonActive;
    }

    public void ChangeAddToPlaylistButtonColor(float _AnimationDuration){
        if(!IsItAñadirColorButtonActive){
            UIAniManager.instance.FadeIn(AñadirColorButton, _AnimationDuration);
            AñadirColorButton.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .3f).OnComplete(() => {AñadirColorButton.transform.DOScale(new Vector3(1f, 1f, 1f), .3f);});
            IsItAñadirColorButtonActive = true;
        } 
    }

    public void AddToPlaylistButtonColorButtonColorAgain(float _AnimationDuration) 
    {    
      UIAniManager.instance.FadeOut(AñadirColorButton, _AnimationDuration);
      AñadirColorButton.transform.DOScale(new Vector3(0f, 0f, 0f), .3f);
      IsItAñadirColorButtonActive = false;        
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
