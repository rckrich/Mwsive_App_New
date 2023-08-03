using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public abstract class ViewModel : AppObject
{
    private const int NETWORKLOADINGPANEL_ANIMATION_LAYER = 0;

    [Header("View ID")]
    public ViewID viewID;
    [Header("General View Model Game Object Reference")]
    public GameObject networkLoadingCanvas;
    public Transform scrollViewContent;

    [SerializeField]
    public System.Action backAction;

    protected Presenter presenter;

    public virtual void Initialize<TV, TP, TI>(TV _viewModel, object[] _list) where TV : ViewModel where TP : Presenter where TI : Interactor{
        if (presenter == null)
        {
            presenter = gameObject.AddComponent(typeof(TP)) as TP;
            presenter.Initialize<TV,TP,TI>(_viewModel, (TP)presenter);
        }
    }

    public virtual void CallPresenter(params object[] list)
    {
        if (!CheckDeviceNetworkReachability())
        {
            CallErrorPopUp();
            return;
        }

        StartSearch();
        presenter.CallInteractor(list);
    }

    public virtual void DisplayOnResult(params object[] list) { CallWaitAFrame(); EndSearch(); }

    public virtual void DisplayOnFailedResult(params object[] list) { CallWaitAFrame(); EndSearch(); }

    public virtual void DisplayOnNetworkError() { CallWaitAFrame(); }

    public virtual void DisplayOnServerError() { CallWaitAFrame(); }

    public virtual ViewID GetViewID() { return viewID; }

    public virtual void StartSearch() { StartCoroutine(CR_StartSearch()); }

    public virtual void EndSearch() { StartCoroutine(CR_EndSearch()); }

    public virtual void EndSearch(string popUpTitle, string popPupMessage) { StartCoroutine(CR_EndSearch(popUpTitle, popPupMessage)); }

    protected virtual void CallErrorPopUp() { ScreenManager.instance.ChangeView(ViewID.ErrorViewModel, true); }

    protected virtual void CallPopUP(PopUpViewModelTypes _type, string _titleText, string _descriptionText, string _actionButtonText = "")
    {
        ScreenManager.instance.ChangeView(ViewID.PopUpViewModel, true);
        PopUpViewModel popUpViewModel = (PopUpViewModel)ScreenManager.instance.GetView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(_type, _titleText, _descriptionText, _actionButtonText);
        popUpViewModel.SetPopUpAction(() => { ScreenManager.instance.BackToPreviousView(); });
        CallWaitAFrame();
    }

    protected virtual void CallWaitAFrame()
    {
        StartCoroutine(CR_WaitAFrame());
    }

    protected virtual IEnumerator CR_WaitAFrame()
    {
        yield return new WaitForSeconds(0.5f);
        if (scrollViewContent != null) LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)scrollViewContent);
        yield return new WaitForSeconds(0.2f);
    }

    protected virtual IEnumerator CR_StartSearch()
    {
        if(networkLoadingCanvas == null)
            networkLoadingCanvas = GameObject.FindWithTag("NetworkLoadingCanvas");

        if (networkLoadingCanvas != null)
        {
            networkLoadingCanvas.transform.GetChild(0).gameObject.SetActive(true);
        }
        yield return null;
    }

    protected virtual IEnumerator CR_EndSearch()
    {
        if (networkLoadingCanvas == null)
            networkLoadingCanvas = GameObject.FindWithTag("NetworkLoadingCanvas");

        if (networkLoadingCanvas != null)
        {
            GameObject networkLoadingPanel = networkLoadingCanvas.transform.GetChild(0).gameObject;
            networkLoadingPanel.SetActive(false);
            float animationLenght = networkLoadingPanel.GetComponent<Animator>().GetCurrentAnimatorClipInfo(NETWORKLOADINGPANEL_ANIMATION_LAYER)[0].clip.length;
            yield return new WaitForSeconds(animationLenght);
        }

        yield return null;
    }

    protected virtual IEnumerator CR_EndSearch(string popUpTitle, string popUpMessage)
    {
        if (networkLoadingCanvas == null)
            networkLoadingCanvas = GameObject.FindWithTag("NetworkLoadingCanvas");

        if (networkLoadingCanvas != null)
        {
            networkLoadingCanvas.transform.GetChild(0).gameObject.SetActive(false);
            CallPopUP(PopUpViewModelTypes.MessageOnly, popUpTitle, popUpMessage);
        }

        yield return null;
    }
}