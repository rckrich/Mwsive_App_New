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
        MwsiveConnectionManager.instance.PostDeleteUser(Callback_PostDeleteUser);
    }

    private void Callback_PostDeleteUser(object[] _value)
    {
        ProgressManager.instance.progress.userDataPersistance.userTokenSetted = false;
        ProgressManager.instance.progress.userDataPersistance.access_token = "";
        ProgressManager.instance.progress.userDataPersistance.spotify_userTokenSetted = false;
        ProgressManager.instance.progress.userDataPersistance.spotify_expires_at = DateTime.Now;
        ProgressManager.instance.progress.userDataPersistance.expires_at = DateTime.Now;
        ProgressManager.instance.progress.userDataPersistance.current_playlist = "";
        ProgressManager.instance.progress.userDataPersistance.token_type = "";
        ProgressManager.instance.progress.userDataPersistance.raw_value = "";
        ProgressManager.instance.save();

        SceneManager.LoadScene("LogInScene");
    }
}
