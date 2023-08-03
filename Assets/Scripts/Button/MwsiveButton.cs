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

    public void OnClickOlaButton(float AnimationDuration){
        if(!IsItOlaColorButtonActive){
            UIAniManager.instance.FadeIn(OlaColorButton, AnimationDuration);
            OlaColorButton.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .3f).OnComplete(() => {OlaColorButton.transform.DOScale(new Vector3(1f, 1f, 1f), .3f);});
            IsItOlaColorButtonActive = true;
        }else{
            UIAniManager.instance.FadeOut(OlaColorButton, AnimationDuration);
            IsItOlaColorButtonActive = false;
            OlaColorButton.transform.DOScale(new Vector3(0f, 0f, 0f), .3f);
        }  
    }

    public bool GetIsItOlaActive(){
        return IsItOlaColorButtonActive;
    }

    public void OnClickAñadirButton(float AnimationDuration){
        if(!IsItAñadirColorButtonActive){
            UIAniManager.instance.FadeIn(AñadirColorButton, AnimationDuration);
            AñadirColorButton.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .3f).OnComplete(() => {AñadirColorButton.transform.DOScale(new Vector3(1f, 1f, 1f), .3f);});
            IsItAñadirColorButtonActive = true;
        }else{
            UIAniManager.instance.FadeOut(AñadirColorButton, AnimationDuration);
            AñadirColorButton.transform.DOScale(new Vector3(0f, 0f, 0f), .3f);
            IsItAñadirColorButtonActive = false;
        }  
    }


    public void OnClickCompartirButton(float AnimationDuration){
        if(!IsItCompartirColorButtonActive){
            UIAniManager.instance.FadeIn(CompartirColorButton, AnimationDuration);
            CompartirColorButton.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .3f).OnComplete(() => {CompartirColorButton.transform.DOScale(new Vector3(1f, 1f, 1f), .3f);});
            IsItCompartirColorButtonActive = true;
        }else{
            UIAniManager.instance.FadeOut(CompartirColorButton, AnimationDuration);
            CompartirColorButton.transform.DOScale(new Vector3(0f, 0f, 0f), .3f);
            IsItCompartirColorButtonActive = false;
        }  
    }
}
