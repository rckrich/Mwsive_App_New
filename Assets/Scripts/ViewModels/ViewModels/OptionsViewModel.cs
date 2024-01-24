using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsViewModel : ViewModel
{
    public List<KeyValuePair<string, string>> url = new List<KeyValuePair<string, string>>();
    public TextMeshProUGUI contactText;

    public override void Initialize(params object[] list)
    {
#if PLATFORM_ANDROID
        SetAndroidBackAction();
#endif
    }

    public void GetSettings()
    {
        MwsiveConnectionManager.instance.GetSettings(Callback_GetSettings);
    }

    private void Callback_GetSettings(object[] _value)
    {
        MwsiveSettingsRoot mwsiveSettingsRoot = (MwsiveSettingsRoot)_value[1];
        
        for(int i = 0; i < mwsiveSettingsRoot.settings.Count; i++)
        {
            url.Add(new KeyValuePair<string, string>(mwsiveSettingsRoot.settings[i].name, mwsiveSettingsRoot.settings[i].value));
        }

        foreach (Settings setting in mwsiveSettingsRoot.settings)
        {
            if(setting.name == "contact_email")
            {
                contactText.text = setting.value;
            }
        }

    }

    public void OnClick_ZonaPeligrosa()
    {
        NewScreenManager.instance.ChangeToSpawnedView("zonaPeligrosa");
        NewScreenManager.instance.GetCurrentView().GetComponent<DangerZoneViewModel>().Initialize();
    }

    public void OnClickBack()
    {
        OpenView(ViewID.ProfileViewModel);
        NewScreenManager.instance.GetCurrentView().SetAndroidBackAction();
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
    }

    private void Callback_PostLogout(object[] _value)
    {
        ProgressManager.instance.DeleteCache();
        SceneManager.LoadScene("LogInScene");     
    }

    public void OnClick_OpenLinks(string _value)
    {
        for(int i = 0; i < url.Count; i++)
        {
            if (_value.Equals("contact_email"))
            {
                CopyToClipboard(url[i].Value);
                return;
            }

            if (_value.Equals(url[i].Key))
            {
                Application.OpenURL(url[i].Value);
                return;
            }
        }
    }

    public static void CopyToClipboard(string _str)
    {
        GUIUtility.systemCopyBuffer = _str;
    }

    public override void SetAndroidBackAction()
    {
#if PLATFORM_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            AppManager.instance.SetAndroidBackAction(() => {
                if (finishedLoading)
                {
                    OnClickBack();       
                }
            });
        }
#endif
    }
}
