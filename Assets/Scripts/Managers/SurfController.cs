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
    public List<int> SurfManagerIndex = new List<int>();
    public GameObject Main, CurrentView;
    private int position;


    public void AddToList(GameObject _SurfManager, bool IsThisMain = false)
    {
        if (!SurfManagers.Contains(_SurfManager))
        {
            SurfManagers.Add(_SurfManager);
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
    private void ControlHierarchy()
    {
        int HigherNumber = -10;
        position = 0;
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
            }
            else
            {
                SurfManagers[i].SetActive(false);
            }

        }

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
               
                CurrentView = Main;
            }
        }



    }

    public void DeleteFromList(GameObject _SurfManager)
    {
        for (int i = 0; i < SurfManagers.Count; i++)
        {
            if (SurfManagers[i] == _SurfManager)
            {
                SurfManagers.Remove(_SurfManager);
                SurfManagerIndex.RemoveAt(i);
            }
        }



    }

}
