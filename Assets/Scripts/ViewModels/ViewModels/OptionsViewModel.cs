using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsViewModel : ViewModel
{

    public void OnClick_ZonaPeligrosa()
    {
        OpenView(ViewID.DangerZoneViewModel);
    }
    public void OnClickBack()
    {
        OpenView(ViewID.ProfileViewModel);
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }
    public void OnClick_LogOut()
    {
        NewScreenManager.instance.ChangeToMainView(ViewID.PopUpViewModel, true);
        PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(PopUpViewModelTypes.OptionChoice, "","¿Estas seguro que deseas cerrar sesión?","Cerrar sesión");
        popUpViewModel.SetPopUpAction(() => { NewScreenManager.instance.BackToPreviousView(); });
        CallWaitAFrame();
    }

    private void OpenView(ViewID _value)
    {
        NewScreenManager.instance.ChangeToMainView(_value, false);
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);

    }
}
