using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptions : ViewModel
{
    public GameObject descubrir;
    public GameObject explorar;
    public GameObject ranking;

    public DescubrirButton ResetDynamicSearch;

    private bool exploreChange = false;
    void Start()
    {
        explorar.GetComponent<Image>().GetComponent<Graphic>().color = Color.gray;
        descubrir.GetComponent<Image>().GetComponent<Graphic>().color = Color.white;
        ranking.GetComponent<Image>().GetComponent<Graphic>().color = Color.gray;
    }

    public void OnClick(int numero)
    {                   
            if(numero == 0)
            {
                if(NewScreenManager.instance.GetCurrentView().viewID == ViewID.ExploreViewModel){
                    ResetDynamicSearch.OnClick_CancelarButton();
                }
                SurfManager.instance.SetActive(false);
                SpotifyPreviewAudioManager.instance.StopTrack();
                
                explorar.GetComponent<Image>().GetComponent<Graphic>().color = Color.white;
                descubrir.GetComponent<Image>().GetComponent<Graphic>().color = Color.gray;
                ranking.GetComponent<Image>().GetComponent<Graphic>().color = Color.gray;
                StopAllCoroutines();
                OpenView(ViewID.ExploreViewModel);         
            }

            if(numero == 1)
            {
#if PLATFORM_ANDROID
            AppManager.instance.ResetAndroidBackAction();
#endif      
            explorar.GetComponent<Image>().GetComponent<Graphic>().color = Color.gray;
                descubrir.GetComponent<Image>().GetComponent<Graphic>().color = Color.white;
                ranking.GetComponent<Image>().GetComponent<Graphic>().color = Color.gray;
                OpenView(ViewID.SurfViewModel);

                SurfManager.instance.SetActive(true);
        }

            if(numero == 2)
            {
                if(NewScreenManager.instance.GetCurrentView().viewID == ViewID.ExploreViewModel){
                    ResetDynamicSearch.OnClick_CancelarButton();
                }
                SurfManager.instance.SetActive(false);
                SpotifyPreviewAudioManager.instance.StopTrack();
                explorar.GetComponent<Image>().GetComponent<Graphic>().color = Color.gray;
                descubrir.GetComponent<Image>().GetComponent<Graphic>().color = Color.gray;
                ranking.GetComponent<Image>().GetComponent<Graphic>().color = Color.white;
                StopAllCoroutines();
                OpenView(ViewID.RankingViewModel);
            }               
    }

    private void OpenView(ViewID _value)
    {
        NewScreenManager.instance.ChangeToMainView(_value, false);   
        
        if(_value == ViewID.ExploreViewModel && !exploreChange)
        {
            NewScreenManager.instance.GetCurrentView().GetComponent<Descubrir_ViewModel>().Initialize();
            exploreChange = true;
        }
    }
}
