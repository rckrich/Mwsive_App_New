using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdnViewModel : ViewModel
{
    public override void Initialize(params object[] list)
    {
#if PLATFORM_ANDROID
        SetAndroidBackAction();
#endif
    }
    public void OnClick_BackButton()
    {
        Debug.Log("Bot?n");
        NewScreenManager.instance.BackToPreviousView();
        NewScreenManager.instance.GetCurrentView().SetAndroidBackAction();
    }

    public override void SetAndroidBackAction()
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
}
