using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptions : ViewModel
{
    public GameObject descubrir;
    public GameObject explorar;
    public GameObject ranking;
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
        
        if(_value == ViewID.ExploreViewModel)
        {
            NewScreenManager.instance.GetCurrentView().GetComponent<Descubrir_ViewModel>().Initialize();
        }
    }
}
