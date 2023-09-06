using System.Collections;
using System.Collections.Generic;
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
        MwsiveConnectionManager.instance.GetCurrentMwsiveUser(Callback_GetCurrentMwsiveUser);
    }

    public void Callback_GetCurrentMwsiveUser(object[] _value)
    {
        MwsiveUserRoot mwsiveUserRoot = (MwsiveUserRoot)_value[1];

        AppManager.instance.currentMwsiveUser = mwsiveUserRoot.user;
    }
}
