using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSubMainViewModel : ViewModel
{
    public void OnClick_SpawnViewButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("normal");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }

    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
    }
}
