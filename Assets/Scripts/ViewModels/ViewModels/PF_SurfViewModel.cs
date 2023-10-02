using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_SurfViewModel : ViewModel
{
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
        NewScreenManager.instance.GetCurrentView().SetAndroidBackAction();
    }

    private void OnDestroy() {
        SpotifyPreviewAudioManager.instance.StopTrack();
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
