using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddUrlViewModel : ViewModel
{
    public TMP_InputField inputUrl;
    public GameObject cross;

    private string type;

    public override void Initialize(params object[] list)
    {
        type = list[0].ToString();
        

        if(AppManager.instance.currentMwsiveUser.user_links.Count != 0)
        {
            SetPlaceHolderInputText();
        }

#if PLATFORM_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            AppManager.instance.SetAndroidBackAction(() => {
                if (finishedLoading)
                {
                    OnClick_BackButton();
                }
            });
        }
#endif
    }

    public void OnEndEdit_URL()
    {
        cross.SetActive(false);
        if (inputUrl.text.Equals(""))
        {
            switch (type)
            {
                case "TIK_TOK":
                    ResetLinks("TIK_TOK");
                    break;
                case "INSTAGRAM":
                    ResetLinks("INSTAGRAM");
                    break;
                case "YOU_TUBE":
                    ResetLinks("YOU_TUBE");
                    break;
                case "EXTERNAL":
                    ResetLinks("EXTERNAL");
                    break;

            }
        }
        else
        {
            switch (type)
            {
                case "TIK_TOK":
                    OnEndEdit_TiktokUrl();
                    break;
                case "INSTAGRAM":
                    OnEndEdit_InstagramUrl();
                    break;
                case "YOU_TUBE":
                    OnEndEdit_YoutubeUrl();
                    break;
                case "EXTERNAL":
                    OnEndEdit_ExternalUrl();
                    break;

            }
        }
    }
    public void OnEndEdit_TiktokUrl()
    {
        if (inputUrl.text.Contains("tiktok.com"))
        {
            MwsiveConnectionManager.instance.PostUserLink("TIK_TOK", CheckURL(inputUrl.text), Callback_PostUserLink);
        }
        else
        {
            CallPopUP(PopUpViewModelTypes.MessageOnly, "URL inválido", "URL inválido o distinto a tiktok", "Aceptar");
#if PLATFORM_ANDROID
            PopUpViewModel currentPopUp = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
            AppManager.instance.SetAndroidBackAction(() => {
                currentPopUp.ExitButtonOnClick();
                this.SetAndroidBackAction();
            });
#endif
        }
    }

    public void OnEndEdit_InstagramUrl()
    {
        if (inputUrl.text.Contains("instagram.com"))
        {
            MwsiveConnectionManager.instance.PostUserLink("INSTAGRAM", CheckURL(inputUrl.text), Callback_PostUserLink);
        }
        else
        {
            CallPopUP(PopUpViewModelTypes.MessageOnly, "URL iválido", "URL inválido o distinto a instagram", "Aceptar");
#if PLATFORM_ANDROID
            PopUpViewModel currentPopUp = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
            AppManager.instance.SetAndroidBackAction(() => {
                currentPopUp.ExitButtonOnClick();
                this.SetAndroidBackAction();
            });
#endif
        }
    }

    public void OnEndEdit_YoutubeUrl()
    {
        if (inputUrl.text.Contains("youtube.com/channel/") || inputUrl.text.Contains("youtube.com/c/") || inputUrl.text.Contains("youtube.com/@") || inputUrl.text.Contains("youtube.com")) 
        {
            MwsiveConnectionManager.instance.PostUserLink("YOU_TUBE", CheckURL(inputUrl.text), Callback_PostUserLink);
        }
        else
        {
            CallPopUP(PopUpViewModelTypes.MessageOnly, "URL inválido", "URL inválido o distinto a youtube", "Aceptar");
#if PLATFORM_ANDROID
            PopUpViewModel currentPopUp = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
            AppManager.instance.SetAndroidBackAction(() => {
                currentPopUp.ExitButtonOnClick();
                this.SetAndroidBackAction();
            });
#endif
        }
    }

    public void OnEndEdit_ExternalUrl()
    {
        if (inputUrl.text.Contains("https://") || inputUrl.text.Contains("."))
        {
            MwsiveConnectionManager.instance.PostUserLink("EXTERNAL", CheckURL(inputUrl.text),  Callback_PostUserLink);
        }
        
        else
        {
            CallPopUP(PopUpViewModelTypes.MessageOnly, "URL inválido", "URL inválido, intente otra vez", "Aceptar");
#if PLATFORM_ANDROID
            PopUpViewModel currentPopUp = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
            AppManager.instance.SetAndroidBackAction(() => {
                currentPopUp.ExitButtonOnClick();
                this.SetAndroidBackAction();
            });
#endif
        }

    }

    public void Callback_PostUserLink(object[] _value)
    {
        StartSearch();
        AppManager.instance.RefreshUser(Callback_RefreshUser);
        
    }

    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
        NewScreenManager.instance.GetCurrentView().GetComponent<EditProfileViewModel>().Initialize(null);
        NewScreenManager.instance.GetCurrentView().SetAndroidBackAction();
        EndSearch();
    }

    public void SetPlaceHolderInputText()
    {
        foreach (UserLink url in AppManager.instance.currentMwsiveUser.user_links)
        {
            if (url.type.Equals( type))
            {
                inputUrl.text = url.link;
            }
        }
    }

    public void ResetLinks(string _type)
    {
        MwsiveConnectionManager.instance.PostUserLink(_type,null, Callback_PostUserLink);
    }

    public void OnClick_CrossImage()
    {
        inputUrl.text = "";
    }

    private void Callback_RefreshUser(object[] value)
    {
        OnClick_BackButton();
    }

    private string CheckURL(string submittedURL)
    {
        bool result = submittedURL.Contains("http://") || submittedURL.Contains("https://");

        if (result)
        {
            return submittedURL;
        }
        else
        {
            return "http://" + submittedURL;
        }
    }

    public override void SetAndroidBackAction()
    {
#if PLATFORM_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            AppManager.instance.SetAndroidBackAction(() => {
                if (finishedLoading)
                {
                    OnClick_BackButton();
                }
            });
        }
# endif
    }
}
