using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditProfileViewModel : ViewModel
{

    public TextMeshProUGUI inputProfileName;
    public TextMeshProUGUI placeHolderEditProfile;
    public Image imageProfile;
    public Sprite haveUrl;
    public Sprite noHaveUrl;

    [Header("Url")]
    public TextMeshProUGUI urlExternal;
    public Image imageExternal;
    public TextMeshProUGUI urlTiktok;
    public Image imageTiktok;
    public TextMeshProUGUI urlIntegram;
    public Image imageInstagram;
    public TextMeshProUGUI urlYoutube;
    public Image imageYoutube;




    public override void Initialize(params object[] list)
    {
        imageTiktok.sprite = noHaveUrl;
        imageInstagram.sprite = noHaveUrl;
        imageYoutube.sprite = noHaveUrl;
        imageExternal.sprite = noHaveUrl;
        placeHolderEditProfile.text = AppManager.instance.currentMwsiveUser.display_name;

        if(AppManager.instance.currentMwsiveUser.image != null)
        {
            ImageManager.instance.GetImage(AppManager.instance.currentMwsiveUser.image, imageProfile, (RectTransform)this.transform);
        }

        if(AppManager.instance.currentMwsiveUser.user_links.Count != 0)
        {
            foreach(UserLink url in AppManager.instance.currentMwsiveUser.user_links)
            {
                switch (url.type)
                {
                    case "TIK_TOK":
                        urlTiktok.text = url.link;
                        imageTiktok.sprite = haveUrl;
                        break;
                    case "INSTAGRAM":
                        urlIntegram.text = url.link;
                        imageInstagram.sprite = haveUrl;
                        break;
                    case "YOU_TUBE":
                        urlYoutube.text = url.link;
                        imageYoutube.sprite = haveUrl;
                        break;
                    case "EXTERNAL":
                        urlExternal.text = url.link;
                        imageExternal.sprite = haveUrl;
                        break;
                }
            }
        }
    }

    public void OnClick_BackButton()
    {
        NewScreenManager.instance.ChangeToMainView(ViewID.ProfileViewModel);
        NewScreenManager.instance.GetCurrentView().GetComponent<ProfileViewModel>().Initialize();
    }

    public void OnClick_EditPhoto()
    {
        OpenGallery.instance.GetImageFromGallery();
    }

    public void OnValueChange_ProfileName()
    {
       
        if (inputProfileName.text.Equals("") || inputProfileName.text.Length <= 6)
        {
            if(AppManager.instance.currentMwsiveUser.display_name != inputProfileName.text)
                CallPopUP(PopUpViewModelTypes.MessageOnly, "Necesitas escribir un nombre", "por favor escribe un nombre con mas de 6 caracteres", "Aceptar");
        }
        else
        {
            MwsiveConnectionManager.instance.PostDisplayName(inputProfileName.text, Callback_PostDisplayName);
        }
    }

    public void Callback_PostDisplayName(object[] _value)
    {
        AppManager.instance.RefreshUser();
    }
    
    public void OnClick_TiktokUrlAdd()
    {
        NewScreenManager.instance.ChangeToSpawnedView("addURL");
        NewScreenManager.instance.GetCurrentView().GetComponent<AddUrlViewModel>().Initialize("TIK_TOK");
    }

    public void OnClick_InstagramUrlAdd()
    {
        NewScreenManager.instance.ChangeToSpawnedView("addURL");
        NewScreenManager.instance.GetCurrentView().GetComponent<AddUrlViewModel>().Initialize("INSTAGRAM");
    }

    public void OnClick_YouTubeUrlAdd()
    {
        NewScreenManager.instance.ChangeToSpawnedView("addURL");
        NewScreenManager.instance.GetCurrentView().GetComponent<AddUrlViewModel>().Initialize("YOU_TUBE");
    }

    public void OnClick_ExternalUrlAdd()
    {
        NewScreenManager.instance.ChangeToSpawnedView("addURL");
        NewScreenManager.instance.GetCurrentView().GetComponent<AddUrlViewModel>().Initialize("EXTERNAL");
    }
}
