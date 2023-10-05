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

    public GameObject ReturnCurrentView(){
        if(CurrentView != null)
        {
            return CurrentView;
        }
        else
        {
            ControlHierarchy();
            return CurrentView;
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

    ///Something Something, Se necesita Actualizar los discos en todos los surfManagers que haya, tanto el actual como en los otros. Puede ser simplemente en un enable y actualizar el actual. 
    //Hay que encontrar la manera de pedir a MwsiveDB los discos del usuario spara poder actualizar el icono. 
    // De otra forma se necesita hay que ver que se tiene que hacer cuando acabas un challenge porque no se actualiza la lista de Challenges en prefab. 

    public void DeleteFromList(GameObject _SurfManager){
        SurfManagers.Remove(_SurfManager);
        SurfManagerIndex.Remove(_SurfManager.transform.GetSiblingIndex());

    }

}
