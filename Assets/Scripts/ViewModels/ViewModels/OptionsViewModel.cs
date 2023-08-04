using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        CallPopUP(PopUpViewModelTypes.OptionChoice, "", "¿Estas seguro que deseas cerrar sesión?", "Cerrar sesión");
        PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
        popUpViewModel.SetPopUpAction(() => MwsiveConnectionManager.instance.PostLogout(Callback_PostLogout));
    }
        private void OpenView(ViewID _value)
    {
        NewScreenManager.instance.ChangeToMainView(_value, false);
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);

    }

    private void Callback_PostLogout(object[] _value)
    {
        Debug.Log("LogOut Exitoso");
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
