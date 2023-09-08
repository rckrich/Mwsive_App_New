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
        Debug.Log("Empezo el cambio");
        readaccessStatus = MediaServices.GetGalleryAccessStatus(GalleryAccessMode.Read);
        Debug.Log(readaccessStatus);
        if(readaccessStatus == GalleryAccessStatus.NotDetermined)
        {
            MediaServices.RequestGalleryAccess(GalleryAccessMode.Read, callback: (result, error) =>
            {
                Debug.Log("Request for gallery access finished.");
                Debug.Log("Gallery access status: " + result.AccessStatus);
                readaccessStatus = result.AccessStatus;
                if (readaccessStatus == GalleryAccessStatus.Authorized)
                {
                    Debug.Log("Call GalleryAuthorized");
                    GalleryAuthorized();
                }
                    
            });
        }

        GalleryAuthorized();

    }

    public void GalleryAuthorized()
    {
        Debug.Log("Enter GalleryAuthorized");
        readaccessStatus = MediaServices.GetGalleryAccessStatus(GalleryAccessMode.Read);
        if (readaccessStatus == GalleryAccessStatus.Authorized)
        {

            Debug.Log("Enter GalleryAuthorized's readaccessStatus");

            MediaServices.SelectImageFromGallery(canEdit: true, (textureData, error) =>
            {
                if (error == null)
                {
                    Debug.Log("Select image from gallery finished successfully.");
                    currentTexture2D = textureData.GetTexture();
                    ((EditProfileViewModel)NewScreenManager.instance.GetCurrentView()).ChangePicture(currentTexture2D);
                    //textureData.GetTexture() // This returns the texture
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
                Debug.Log("Request for gallery access finished.");
                Debug.Log("Gallery access status: " + result.AccessStatus);
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

                    Debug.Log("Select image from gallery finished successfully.");
                    Debug.Log("Texture is " + currentTexture2D.ToString());
                    Debug.Log("Post");
                }
                else
                {
                    Debug.Log("Select image from gallery failed with error. Error: " + error);
                }
            });
        }
    }
}
