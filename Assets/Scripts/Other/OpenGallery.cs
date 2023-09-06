using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.EssentialKit;

public class OpenGallery : Manager
{
    public static OpenGallery instance;

    GalleryAccessStatus readaccessStatus;
    GalleryAccessStatus readAndWriteAccessStatus;
    GalleryAccessStatus cameraaccesstatus;

    public Texture2D currentImage;
 
    void Start()
    {
        instance = this;
    }

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
                    currentImage = textureData.GetTexture();
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
                    Debug.Log("Select image from gallery finished successfully.");
                    currentImage = textureData.GetTexture();
                    //textureData.GetTexture() // This returns the texture
                }
                else
                {
                    Debug.Log("Select image from gallery failed with error. Error: " + error);
                }
            });
        }
    }
}
