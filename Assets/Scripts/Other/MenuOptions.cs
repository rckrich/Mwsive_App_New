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
                OpenView(ViewID.ExploreViewModel);         
            }

            if(numero == 1)
            {
                
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
                OpenView(ViewID.RankingViewModel);
            }               
    }

    private void OpenView(ViewID _value)
    {
        NewScreenManager.instance.ChangeToMainView(_value, false);        
    }
}
