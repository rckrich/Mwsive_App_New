using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DangerZoneViewModel : ViewModel
{
    public override void Initialize(params object[] list)
    {
#if PLATFORM_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            AppManager.instance.SetAndroidBackAction(() => {
                if (finishedLoading)
                {
                    OnClick_BackButton();
                }
            });
        }
#endif
    }

    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
        NewScreenManager.instance.GetCurrentView().SetAndroidBackAction();
    }

    public void OnClick_Deleteuser()
    {
        NewScreenManager.instance.GetCurrentView().EndSearch();
        NewScreenManager.instance.ChangeToMainView(ViewID.PopUpViewModel, true);
        PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(PopUpViewModelTypes.OptionChoice, "Advertencia", "De aceptar, el usuario será eliminado de la base de datos.", "Aceptar");
        popUpViewModel.SetPopUpAction(() => {
            NewScreenManager.instance.BackToPreviousView();
            MwsiveConnectionManager.instance.PostDeleteUser(Callback_PostDeleteUser);
        });
        popUpViewModel.SetPopUpCancelAction(() => {
            NewScreenManager.instance.BackToPreviousView();
        });
    }

    private void Callback_PostDeleteUser(object[] _value)
    {
        ProgressManager.instance.DeleteCache();
        SceneManager.LoadScene("LogInScene_Ricardo");
    }
}
