using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_SurfViewModel : ViewModel
{
    [HideInInspector]
    public bool canGoBack = true;

    public Sprite image;
    public override void Initialize(params object[] list)
    {
#if PLATFORM_ANDROID
        SetAndroidBackAction();
#endif
    }

    public void OnClick_BackButton()
    {
        StopAllCoroutines();
        SpotifyPreviewAudioManager.instance.StopTrack();
        NewScreenManager.instance.BackToPreviousView();
        if (NewScreenManager.instance.GetCurrentView().GetComponent<Descubrir_ViewModel>())
        {
            NewScreenManager.instance.GetCurrentView().GetComponent<Descubrir_ViewModel>().InteractableButton();
        }
        NewScreenManager.instance.GetCurrentView().SetAndroidBackAction();
    }

    private void OnDestroy() {
        try
        {
            SpotifyPreviewAudioManager.instance.StopTrack();
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("Can not stop strack, object is killed");
        }
        
    }

    public void OnClick_Discos()
    {
        CallPopUP(PopUpViewModelTypes.MessageOnly, "¿Qué son los discos?", "Cada vez que escuches una canción que te haga vibrar, puedes lanzar un disco para votar por tus favoritas y destacar en el ranking. <color=#7C7DF5>(1 Disco = 1 Pik)</color> <br>", "Aceptar", image);
#if PLATFORM_ANDROID
        PopUpViewModel currentPopUp = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
        AppManager.instance.SetAndroidBackAction(() => {
            currentPopUp.ExitButtonOnClick();
            this.SetAndroidBackAction();
        });
#endif   
    }

    public override void SetAndroidBackAction()
    {
#if PLATFORM_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            AppManager.instance.SetAndroidBackAction(() => {
                if (finishedLoading)
                {
                    if(canGoBack)
                        OnClick_BackButton();
                }
            });
        }
#endif
    }
}
