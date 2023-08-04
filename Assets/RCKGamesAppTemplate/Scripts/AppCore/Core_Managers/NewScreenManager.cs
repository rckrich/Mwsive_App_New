using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewScreenManager : Manager
{
    private const int MIN_BACK_STACK_COUNT = 1;

    private Stack<ViewModel> backViewStack;
    private List<ViewModel> spawnedViewsList;

    [Header("Views Array")]
    [SerializeField]
    private ViewModel[] mainViews = null;

    [Header("Current View")]
    [SerializeField]
    private ViewModel currentView = null;

    [Header("Header View")]
    [SerializeField]
    private ViewModel headerView = null;

    [Header("General Loading Canvas")]
    [SerializeField]
    private GameObject loadingCanvas = null;

    [Header("View Types Data")]
    [SerializeField]
    private SpawnableViewModelTypesScriptableObject viewModelTypesData;

    private bool isCurrentViewSpawned = false;

    public RectTransform spawnedViewsParent;

    private static NewScreenManager _instance;

    public static NewScreenManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<NewScreenManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        backViewStack = new Stack<ViewModel>();
        spawnedViewsList = new List<ViewModel>();
    }

    public void BackToPreviousView()
    {
        if (backViewStack.Count >= MIN_BACK_STACK_COUNT)
        {
            isCurrentViewSpawned = CeckIfCurrentViewSpawned(currentView);

            if (isCurrentViewSpawned)
            {
                RemoveSpawnedViewFromList(currentView);
                Destroy(currentView.gameObject);

            }
            else {
                currentView.SetActive(false);
            }
            
            currentView = backViewStack.Peek();
            backViewStack.Pop();
        }
    }

    public void ChangeToMainView(ViewID _viewID, bool _isSubMainView = false)
    {
        SetChangeOfMainViews(_viewID, _isSubMainView);
    }

    public void ChangeToSpawnedView(string _SpawnedViewType)
    {
        SetChangeOfSpawnedViews(_SpawnedViewType);
    }

    public ViewModel GetMainView(ViewID viewID)
    {
        foreach (ViewModel viewInstance in mainViews)
        {
            if (((ViewModel)(viewInstance.GetComponent(typeof(ViewModel)))).GetViewID() == viewID)
            {
                return viewInstance;
            }
        }

        return null;
    }

    public ViewModel GetCurrentView()
    {
        return currentView;
    }

    public ViewModel GetHeaderView()
    {
        return headerView;
    }

    public void SetHeaderViewActive(bool _value)
    {
        headerView.SetActive(_value);
    }

    public bool HasSubViews()
    {
        if (backViewStack.Count <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void SetCurrentViewSiblingIndex(int _value)
    {
        currentView.transform.SetSiblingIndex(_value);
    }

    public void SetCurrentViewAsFirstSibling(int _value)
    {
        currentView.transform.SetAsFirstSibling();
    }

    public void SetCurrentViewAsLastSibling(int _value)
    {
        currentView.transform.SetAsLastSibling();
    }

    public void SetMainViewSiblingIndex(ViewID _id, int _value)
    {
        GetMainView(_id).transform.SetSiblingIndex(_value);
    }

    public void SetMainViewAsFirstSibling(ViewID _id)
    {
        GetMainView(_id).transform.SetAsFirstSibling();
    }

    public void SetMainViewAsLastSiblig(ViewID _id)
    {
        GetMainView(_id).transform.SetAsLastSibling();
    }

    private void SetChangeOfMainViews(ViewID _viewID, bool _isSubView = false)
    {

        if (_isSubView)
        {
            if (_viewID != ((ViewModel)currentView.GetComponent(typeof(ViewModel))).GetViewID())
                backViewStack.Push(currentView);
        }
        else
        {
            ClearMainViews();
            DestroyAllSpawnedViews();
            backViewStack.Clear();
        }

        ChangeToMainView(_viewID);
    }

    private void SetChangeOfSpawnedViews(string _SpawnedViewType)
    {
        backViewStack.Push(currentView);

        SpawnViewOfType(_SpawnedViewType);
    }

    private void ChangeToMainView(ViewID _viewID)
    {
        foreach (ViewModel viewInstance in mainViews)
        {
            if (((ViewModel)viewInstance.GetComponent(typeof(ViewModel))).GetViewID() == _viewID)
            {
                this.currentView = viewInstance;

                ((ViewModel)(viewInstance.GetComponent(typeof(ViewModel)))).SetActive(true);
            }
        }
    }

    private void ClearMainViews()
    {
        foreach (ViewModel viewInstance in mainViews)
        {
            ((ViewModel)viewInstance.GetComponent(typeof(ViewModel))).SetActive(false);
        }
    }

    private void SpawnViewOfType(string _type)
    {
        GameObject searchedView = viewModelTypesData.viewTypes.Find(x => x.viewModelType.Equals(_type)).viewModelPrefab;

        ViewModel spawnedView = Instantiate(searchedView, spawnedViewsParent).GetComponent<ViewModel>();

        currentView = spawnedView;

        spawnedViewsList.Add(spawnedView);

        spawnedView.transform.SetAsLastSibling();

        LayoutRebuilder.ForceRebuildLayoutImmediate(spawnedViewsParent);
    }

    private void DestroyAllSpawnedViews()
    {
        foreach(ViewModel view in spawnedViewsList)
        {
            Destroy(view.gameObject);
        }

        spawnedViewsList.Clear();
    }

    private void RemoveSpawnedViewFromList(ViewModel _currentView)
    {
        if (CeckIfCurrentViewSpawned(_currentView))
        {
            spawnedViewsList.Remove(_currentView);
        }
    }

    private bool CeckIfCurrentViewSpawned(ViewModel _currentView)
    {
        return spawnedViewsList.Contains(_currentView);
    }
}
