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

    // Update is called once per frame
    public void OnClick(int numero)
    {                   
            if(numero == 0)
            {
                explorar.GetComponent<Image>().GetComponent<Graphic>().color = Color.white;
                descubrir.GetComponent<Image>().GetComponent<Graphic>().color = Color.gray;
                ranking.GetComponent<Image>().GetComponent<Graphic>().color = Color.gray;
                //OpenView(ViewID.DescubrirViewModel);
           
            }
            if(numero == 1)
            {
                explorar.GetComponent<Image>().GetComponent<Graphic>().color = Color.gray;
                descubrir.GetComponent<Image>().GetComponent<Graphic>().color = Color.white;
                ranking.GetComponent<Image>().GetComponent<Graphic>().color = Color.gray;
                OpenView(ViewID.SurfViewModel);
            }
            if(numero == 2)
            {
                explorar.GetComponent<Image>().GetComponent<Graphic>().color = Color.gray;
                descubrir.GetComponent<Image>().GetComponent<Graphic>().color = Color.gray;
                ranking.GetComponent<Image>().GetComponent<Graphic>().color = Color.white;
                OpenView(ViewID.RankingViewModel);
            }               
    }

    private void OpenView(ViewID _value)
    {
        NewScreenManager.instance.ChangeToMainView(_value, false);
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
        
    }
}
