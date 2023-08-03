using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsDemoViewModel : ViewModel
{
    private void Start()
    {
        Initialize<SettingsDemoViewModel, SettingsDemoPresenter, SettingsDemoInteractor>(this, null);
    }

    public void OnClick_LogOut()
    {
        CallPresenter();
    }

    public void OnClick_Close()
    {
        ScreenManager.instance.BackToPreviousView();
    }
}
