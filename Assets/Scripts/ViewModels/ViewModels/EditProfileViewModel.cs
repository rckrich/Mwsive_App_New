using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditProfileViewModel : ViewModel
{

    public TMP_InputField inputProfileName;
    public TextMeshProUGUI placeHolderEditProfile;
    public Image imageProfile;
    public Sprite haveUrl;
    public Sprite noHaveUrl;
    public GameObject X_image;
    public GameObject GuardarNombre;

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
        inputProfileName.text = AppManager.instance.currentMwsiveUser.display_name;

        if(AppManager.instance.currentMwsiveUser.image_url != null)
        {
            ImageManager.instance.GetImage(AppManager.instance.currentMwsiveUser.image_url, imageProfile, (RectTransform)this.transform);
        }

        if(AppManager.instance.currentMwsiveUser.user_links.Count != 0)
        {
            foreach(UserLink url in AppManager.instance.currentMwsiveUser.user_links)
            {
                if (url.link == null)
                {
                    switch (url.type)
                    {
                        case "TIK_TOK":
                            urlTiktok.text = "Agregar enlace externo";
                            imageTiktok.sprite = noHaveUrl;
                            break;
                        case "INSTAGRAM":
                            urlIntegram.text = "Agregar enlace externo";
                            imageInstagram.sprite = noHaveUrl;
                            break;
                        case "YOU_TUBE":
                            urlYoutube.text = "Agregar enlace externo";
                            imageYoutube.sprite = noHaveUrl;
                            break;
                        case "EXTERNAL":
                            urlExternal.text = "Agregar enlace externo";
                            imageExternal.sprite = noHaveUrl;
                            break;
                    }
                }
                else
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
    }

    public void OnClick_BackButton()
    {
        SetFalseXImage();
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

        X_image.SetActive(false);
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

    public void ChangePicture(Texture2D _texture)
    {
        imageProfile.sprite = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        StartSearch();
        MwsiveConnectionManager.instance.PostProfilePicture(_texture, Callback_ChangeProfilePicture);
    }

    public void Callback_ChangeProfilePicture(object[] _value)
    {
        AppManager.instance.RefreshUser(Callback_RefreshUser);
        EndSearch();
    }

    public void Callback_RefreshUser(object[] _value)
    {
        Initialize();
    }

    public void SetActiveXImage()
    {
        X_image.SetActive(true);
        GuardarNombre.SetActive(true);
    }

    public void SetFalseXImage()
    {
        X_image.SetActive(false);
        GuardarNombre.SetActive(false);
    }

    public void OnClick_XImage()
    {
        inputProfileName.text = "";
    }
}
