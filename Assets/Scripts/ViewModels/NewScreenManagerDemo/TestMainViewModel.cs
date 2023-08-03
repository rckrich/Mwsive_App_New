using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMainViewModel : ViewModel
{
    public void OnClick_SpawnViewButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("normal");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }

    public void OnClick_SpawnSubMainViewButton()
    {
        NewScreenManager.instance.ChangeToMainView(ViewID.LogInDemoViewModel, true);
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }
}