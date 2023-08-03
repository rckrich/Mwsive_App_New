using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashViewModel : ViewModel
{
    private void Start()
    {
        AppStartProsses();
    }
  
    private void AppStartProsses()
    {
        if (ProgressManager.instance.progress.userDataPersistance.spotify_userTokenSetted)
        {
            if (HasSpotifyTokenExpired())
            {
                LogInManager.instance.StartLogInProcess();
            }
            else
            {
                LogInManager.instance.MwsiveTokenLogInProcess();
            }
        }
        else
        {
            OpenView(ViewID.LogInViewModel);
        }
    }

    private bool HasSpotifyTokenExpired()
    {
        return ProgressManager.instance.progress.userDataPersistance.spotify_expires_at.CompareTo(DateTime.Now) > 0;
    }

    private void OpenView(ViewID _value)
    {
        NewScreenManager.instance.ChangeToMainView(_value, false);
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }
}