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
            LogInManager.instance.StartLogInProcess();
        }
        else
        {
            OpenView(ViewID.LogInViewModel);
        }
    }

    private bool HasSpotifyTokenExpired()
    {
        return ProgressManager.instance.progress.userDataPersistance.spotify_expires_at.CompareTo(DateTime.Now) < 0;
    }

    private void OpenView(ViewID _value)
    {
        NewScreenManager.instance.ChangeToMainView(_value, false);
    }

    public void OpenLogInViewFromSplashView()
    {
        OpenView(ViewID.LogInViewModel);
    }
}