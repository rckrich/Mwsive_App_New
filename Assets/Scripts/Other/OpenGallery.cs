using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.EssentialKit;

public class OpenGallery : Manager
{
    private static OpenGallery _instance;
    public static OpenGallery instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<OpenGallery>();

            return _instance;
        }
    }

    GalleryAccessStatus readaccessStatus;
    GalleryAccessStatus readAndWriteAccessStatus;
    GalleryAccessStatus cameraaccesstatus;

    public Texture2D currentTexture2D;

    public void GetImageFromGallery()
    {
        readaccessStatus = MediaServices.GetGalleryAccessStatus(GalleryAccessMode.Read);
        if(readaccessStatus == GalleryAccessStatus.Denied)
        {
            CallPopUP(PopUpViewModelTypes.OptionChoice, "Necesitas permisos", "La aplicación requiere permisos de galería para poder cambiar tu foto.", "Dar permisos");
            PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
            popUpViewModel.SetPopUpAction(() => {
                Utilities.OpenApplicationSettings();
            });
            return;
        }
        if(readaccessStatus == GalleryAccessStatus.NotDetermined)
        {
            MediaServices.RequestGalleryAccess(GalleryAccessMode.Read, callback: (result, error) =>
            {
                readaccessStatus = result.AccessStatus;
                if (readaccessStatus == GalleryAccessStatus.Authorized)
                {
                    GalleryAuthorized();
                }
                    
            });
        }

        GalleryAuthorized();

    }

    public void GalleryAuthorized()
    {
        readaccessStatus = MediaServices.GetGalleryAccessStatus(GalleryAccessMode.Read);
        if (readaccessStatus == GalleryAccessStatus.Authorized)
        {

            MediaServices.SelectImageFromGallery(canEdit: true, (textureData, error) =>
            {
                if (error == null)
                {
                    currentTexture2D = textureData.GetTexture();
                    ((EditProfileViewModel)NewScreenManager.instance.GetCurrentView()).ChangePicture(currentTexture2D);
                }
                else
                {
                    Debug.Log("Select image from gallery failed with error. Error: " + error);
                }
            });
        }
    }

    public void GalleryReadAndWrite()
    {
        readAndWriteAccessStatus = MediaServices.GetGalleryAccessStatus(GalleryAccessMode.ReadWrite);

        if(readAndWriteAccessStatus == GalleryAccessStatus.NotDetermined)
        {
            MediaServices.RequestGalleryAccess(GalleryAccessMode.ReadWrite, callback: (result, error) =>
            {

            });
        }

        if(readAndWriteAccessStatus == GalleryAccessStatus.Authorized)
        {
            MediaServices.SelectImageFromGallery(canEdit: true, (textureData, error) =>
            {
                if (error == null)
                {
                    currentTexture2D = textureData.GetTexture();
                    NewScreenManager.instance.GetCurrentView().GetComponent<EditProfileViewModel>().ChangePicture(currentTexture2D);
                }
                else
                {
                    Debug.Log("Select image from gallery failed with error. Error: " + error);
                }
            });
        }
    }

    private void CallPopUP(PopUpViewModelTypes _type, string _titleText, string _descriptionText, string _actionButtonText = "")
    {

        NewScreenManager.instance.ChangeToMainView(ViewID.PopUpViewModel, true);
        PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(_type, _titleText, _descriptionText, _actionButtonText);
        popUpViewModel.SetPopUpAction(() => { NewScreenManager.instance.BackToPreviousView(); });


    }
}
