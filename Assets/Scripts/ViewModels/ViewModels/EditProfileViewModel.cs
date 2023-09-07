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

   public override void Initialize(params object[] list)
    {
        placeHolderEditProfile.text = AppManager.instance.currentMwsiveUser.display_name;

        if(AppManager.instance.currentMwsiveUser.image != null)
        {
            ImageManager.instance.GetImage(AppManager.instance.currentMwsiveUser.image, imageProfile, (RectTransform)this.transform);
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
       
        if (inputProfileName.Equals("") || inputProfileName.text.Length <= 6)
        {
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
