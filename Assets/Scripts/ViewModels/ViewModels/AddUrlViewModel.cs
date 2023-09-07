using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddUrlViewModel : ViewModel
{
    public TextMeshProUGUI inputUrl;
    string type;

    public override void Initialize(params object[] list)
    {
        type = list[0].ToString();
    }

    public void OnEndEdit_URL()
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
    public void OnEndEdit_TiktokUrl()
    {
        if (inputUrl.text.Contains("https://www.tiktok.com"))
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
        if (inputUrl.text.Contains("https://instagram.com"))
        {
            MwsiveConnectionManager.instance.PostUserLink("INSTAGRAM", inputUrl.text, Callback_PostUserLink);
        }
        else
        {
            CallPopUP(PopUpViewModelTypes.MessageOnly, "URL inv?lido", "URL inv?lido, intente otra vez", "Aceptar");
        }
    }

    public void OnEndEdit_YoutubeUrl()
    {
        if (inputUrl.text.Contains("youtube.com/channel/"))
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
        if (inputUrl.text.Contains("https://")|| inputUrl.text.Contains("."))
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
    }
}
