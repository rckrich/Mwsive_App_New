using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableTestViewModel : ScrollViewModel
{
    public void OnClick_SpawnViewButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("normal");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }

    public void OnClick_SpawnScrollViewButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("scroll");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }

    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
    }

    public void OnClick_SpawnSubMainViewButton()
    {
        NewScreenManager.instance.ChangeToMainView(ViewID.LogInDemoViewModel, true);
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }
}
