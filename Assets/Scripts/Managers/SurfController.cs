using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SurfController : MonoBehaviour
{
    private static SurfController _instance;
    
    public static SurfController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SurfController>();
            }
            return _instance;
        }
    }

    private List<GameObject> SurfManagers = new List<GameObject>();
    private List<int> SurfManagerIndex = new List<int>();
    private GameObject Main, CurrentView;
    private int position;


    public void AddToList(GameObject _SurfManager, bool IsThisMain = false){
        if(!SurfManagers.Contains(_SurfManager)){
            SurfManagers.Add(_SurfManager);
            SurfManagerIndex.Add(_SurfManager.transform.GetSiblingIndex());
            
        }
        if(IsThisMain){
            _SurfManager = Main;
        }
        ControlHierarchy();

    }

    public bool AmICurrentView(GameObject _SurfManager){
        if(_SurfManager == CurrentView){
            return true;
        }else{
            return false;
        }
    }


    private void ControlHierarchy()
    {   int HigherNumber = 0;
        position = 0;
        for (int i = 0; i < SurfManagers.Count; i++)
        {
            if(SurfManagerIndex[i] > HigherNumber && SurfManagers[i] != Main){
                HigherNumber = SurfManagerIndex[i];
                position = i;
            }
        }
        for (int i = 0; i < SurfManagers.Count; i++)
        {
            if(i == position){
                SurfManagers[i].SetActive(true);
                CurrentView = SurfManagers[i];
            }else{
                SurfManagers[i].SetActive(false);
            }
            
        }

        if(SurfManagers.Count == 1){
            if(SurfManagers[0] == Main){
                Main.SetActive(true);
                CurrentView = Main;
            }
        }



    }

    public void DeleteFromList(GameObject _SurfManager){
        SurfManagers.Remove(_SurfManager);
        SurfManagerIndex.Remove(_SurfManager.transform.GetSiblingIndex());

    }

}
