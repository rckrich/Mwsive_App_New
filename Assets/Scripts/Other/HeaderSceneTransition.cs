using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HeaderSceneTransition : MonoBehaviour
{
    private bool IsDescubrirEnable = false;
    private bool IsRakingEnable = false;

    public GameObject Descubrir;
    public GameObject Ranking;
    public Image DescubrirButton;
    public Image RankingButton;
    public Image SurfButton;

    public void OnClickChangeSceneSurf(){
        SurfButton.color = new Color32(255,255,255,255);
        if(IsDescubrirEnable){
            IsDescubrirEnable = false;
            UIAniManager.instance.SideLeftTransitionExitCenter(Descubrir);
            DescubrirButton.color = new Color32(147,147,147,255);
        }else if(IsRakingEnable){
            IsRakingEnable = false;
            UIAniManager.instance.SideLeftTransitionExitCenter(Ranking);
            RankingButton.color = new Color32(147,147,147,255);
        }
    }
    public void OnClickChangeSceneDescubrir(){
        if(Ranking){
            IsRakingEnable = false;
          UIAniManager.instance.SideLeftTransitionExitCenter(Ranking);
          RankingButton.color = new Color32(147,147,147,255);  
        }
        UIAniManager.instance.SideLeftTransitionEnterCenter(Descubrir);
        IsDescubrirEnable = true;
        DescubrirButton.color = new Color32(255,255,255,255);
        SurfButton.color = new Color32(147,147,147,255);
    }
    public void OnClickChangeSceneRanking(){
        if(IsDescubrirEnable){
            IsDescubrirEnable = false;
            UIAniManager.instance.SideLeftTransitionExitCenter(Descubrir);
            DescubrirButton.color = new Color32(147,147,147,255);
        }
        UIAniManager.instance.SideLeftTransitionEnterCenter(Ranking);
        IsRakingEnable = true;
        RankingButton.color = new Color32(255,255,255,255);
        SurfButton.color = new Color32(147,147,147,255);
        
    }
}
