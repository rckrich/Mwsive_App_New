using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsViewModel : ViewModel
{
  
    public void OnClick_ZonaPeligrosa()
    {

    }
    public void OnClickBack()
    {
        NewScreenManager.instance.ChangeToMainView(ViewID.ProfileViewModel, false);
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }
}
