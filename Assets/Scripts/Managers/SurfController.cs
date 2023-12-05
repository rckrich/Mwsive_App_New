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

    public List<GameObject> SurfManagers = new List<GameObject>();
    public List<GameObject> SurfViewmodels = new List<GameObject>();
    public List<int> SurfManagerIndex = new List<int>();
    public GameObject Main, CurrentView, CurrentSurfViewModel;
    
    private int position;


    public void AddToList(GameObject _SurfViewmodel, GameObject _SurfManager, bool IsThisMain = false)
    {
        if (!SurfManagers.Contains(_SurfManager))
        {
            SurfManagers.Add(_SurfManager);
            SurfViewmodels.Add(_SurfViewmodel);
            if (!IsThisMain)
            {
                SurfManagerIndex.Add(_SurfManager.GetComponentInParent<PF_SurfViewModel>().gameObject.transform.GetSiblingIndex());
            }
            else
            {
                SurfManagerIndex.Add(-1);
            }



        }
        if (IsThisMain)
        {
            Main = _SurfManager;
        }
        ControlHierarchy();

    }

    public bool AmICurrentView(GameObject _SurfManager)
    {
        if (_SurfManager == CurrentView)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public GameObject ReturnMain()
    {
        if (Main != null)
        {
            return Main;
        }
        else
        {
            ControlHierarchy();
            return Main;
        }

    }

    public GameObject ReturnCurrentSurfViewModel()
    {
        if (CurrentSurfViewModel != null)
        {
            return CurrentSurfViewModel;
        }
        else
        {
            ControlHierarchy();
            return CurrentSurfViewModel;
        }

    }

    public GameObject ReturnCurrentView()
    {
        
        if (CurrentView != null)
        {
            return CurrentView;
        }
        else
        {
            ControlHierarchy();
            return CurrentView;
        }
        

    }
    public void ControlHierarchy()
    {
        int HigherNumber = -10;
        position = 0;
        if (SurfManagers.Count == 1)
        {
            if (SurfManagers[0] == Main)
            {
                if (NewScreenManager.instance.GetCurrentView().viewID == ViewID.SurfViewModel)
                {
                    Main.SetActive(true);
                }
                else
                {
                    Main.SetActive(false);
                }
                CurrentSurfViewModel = SurfViewmodels[0];
                CurrentView = Main;
            }
            return;
        }
        for (int i = 0; i < SurfManagers.Count; i++)
        {
            if (SurfManagerIndex[i] > HigherNumber && SurfManagers[i] != Main)
            {
                HigherNumber = SurfManagerIndex[i];
                position = i;
            }
        }
        for (int i = 0; i < SurfManagers.Count; i++)
        {
            if (i == position)
            {

                SurfManagers[i].SetActive(true);

                CurrentView = SurfManagers[i];
                CurrentSurfViewModel = SurfViewmodels[i];
            }
            else
            {
                SurfManagers[i].SetActive(false);
            }

        }

        



    }

    public void DeleteFromList(GameObject _SurfManager, GameObject _Surfviewmodel)
    {
        for (int i = 0; i < SurfManagers.Count; i++)
        {
            if (SurfManagers[i] == _SurfManager)
            {
                SurfManagers.Remove(_SurfManager);
                SurfViewmodels.Remove(_Surfviewmodel);
                SurfManagerIndex.RemoveAt(i);

                ControlHierarchy();
            }
        }



    }

}
