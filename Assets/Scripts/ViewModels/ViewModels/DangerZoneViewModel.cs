using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        NewScreenManager.instance.GetCurrentView().GetComponent<OptionsViewModel>().SetAndroidBackAction();
    }
}
