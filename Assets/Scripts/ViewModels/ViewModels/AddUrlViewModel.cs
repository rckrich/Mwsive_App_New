using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddUrlViewModel : ViewModel
{
    public TMP_InputField inputUrl;

    private string type;

    public override void Initialize(params object[] list)
    {
        type = list[0].ToString();
        

        if(AppManager.instance.currentMwsiveUser.user_links.Count != 0)
        {
            SetPlaceHolderInputText();
        }
        
    }

    public void OnEndEdit_URL()
    {
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
            MwsiveConnectionManager.instance.PostUserLink("TIK_TOK", inputUrl.text, Callback_PostUserLink);
        }
        else
        {
            CallPopUP(PopUpViewModelTypes.MessageOnly, "URL inv?lido", "URL inv?lido, intente otra vez", "Aceptar");
        }
    }

    public void OnEndEdit_InstagramUrl()
    {
        if (inputUrl.text.Contains("instagram.com"))
        {
            MwsiveConnectionManager.instance.PostUserLink("INSTAGRAM", inputUrl.text, Callback_PostUserLink);
        }/*else if (inputUrl.text.Equals(""))
        {
            MwsiveConnectionManager.instance.PostUserLink("INSTAGRAM", inputUrl.text, Callback_PostUserLink);
        }*/
        else
        {
            CallPopUP(PopUpViewModelTypes.MessageOnly, "URL inv?lido", "URL inv?lido, intente otra vez", "Aceptar");
        }
    }

    public void OnEndEdit_YoutubeUrl()
    {
        if (inputUrl.text.Contains("youtube.com/channel/") || inputUrl.text.Contains("youtube.com/c/"))
        {
            MwsiveConnectionManager.instance.PostUserLink("YOU_TUBE", inputUrl.text, Callback_PostUserLink);
        }
        else
        {
            CallPopUP(PopUpViewModelTypes.MessageOnly, "URL inv?lido", "URL inv?lido, intente otra vez", "Aceptar");
        }
    }

    public void OnEndEdit_ExternalUrl()
    {
        if (inputUrl.text.Contains("https://") || inputUrl.text.Contains("."))
        {
            MwsiveConnectionManager.instance.PostUserLink("EXTERNAL", inputUrl.text, Callback_PostUserLink);
        }
        else
        {
            CallPopUP(PopUpViewModelTypes.MessageOnly, "URL inv?lido", "URL inv?lido, intente otra vez", "Aceptar");
        }
    }

    public void Callback_PostUserLink(object[] _value)
    {
        AppManager.instance.RefreshUser();
    }

    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
        NewScreenManager.instance.GetCurrentView().GetComponent<EditProfileViewModel>().Initialize();
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
        MwsiveConnectionManager.instance.PostUserLink(_type,"", Callback_PostUserLink);
    }

    public void OnClick_CrossImage()
    {
        inputUrl.text = "";
    }
}
