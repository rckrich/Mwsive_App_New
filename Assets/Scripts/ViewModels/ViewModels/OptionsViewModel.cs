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
    public List<string> url = new List<string>();


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
            url.Add( mwsiveSettingsRoot.settings[i].value);
        }
        
    }
    public void OnClick_ZonaPeligrosa()
    {
        NewScreenManager.instance.ChangeToSpawnedView("zonaPeligrosa");
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

    public void OnClick_OpenLinks(string _value)
    {
        switch (_value)
        {
            case "instagram":
                Application.OpenURL(url[0]);
                break;
            case "threads":
                Application.OpenURL(url[1]);
                break;
            case "tiktok":
                Application.OpenURL(url[2]);
                break;
            case "snapchat":
                Application.OpenURL(url[3]);
                break;
            case "youtube":
                Application.OpenURL(url[4]);
                break;
            case "twitter":
                Application.OpenURL(url[5]);
                break;
            case "linkedin":
                Application.OpenURL(url[6]);
                break;
            case "website":
                Application.OpenURL(url[7]);
                break;
            case "help_us":
                Application.OpenURL(url[8]);
                break;          
            case "faq":
                Application.OpenURL(url[9]);
                break;
            case "terms_and_conditions":
                Application.OpenURL(url[10]);
                break;
            case "privacy_policies":
                Application.OpenURL(url[11]);
                break;
            case "contact_email":
                CopyToClipboard(url[12]);
                break;
            
        }
    }

    public static void CopyToClipboard(string _str)
    {
        GUIUtility.systemCopyBuffer = _str;
    }

    public void SetAndroidBackAction()
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
