using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;

public class ImageManager : Manager
{
    private static ImageManager self;

    public static ImageManager instance
    {
        get
        {
            if (self == null)
            {
                self = (ImageManager)GameObject.FindObjectOfType(typeof(ImageManager));
            }
            return self;
        }
    }

    private const string GET_IMAGE_URI = "";

    [Header("Error at loading image texture")]
    public Sprite errorSprite;

    public void GetImage(string imageURL, IImageDownloaderObject ImageAppObject)
    {
        StartCoroutine(CR_GetImage(imageURL, ImageAppObject));
    }

    public void GetImage(string imageURL, Image imageObject, RectTransform imageParentRectTransform)
    {
        StartCoroutine(CR_GetImage(imageURL, imageObject, imageParentRectTransform));
    }

    private IEnumerator CR_GetImage(string imageURL, IImageDownloaderObject ImageAppObject)
    {
        GameObject imageContainer = ImageAppObject.GetImageContainer();

        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageURL))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(webRequest.error);
                if (imageContainer != null)
                    ImageAppObject.SetImage(errorSprite);
            }
            else
            {
                Texture2D retrievedTexture2D = DownloadHandlerTexture.GetContent(webRequest);
                if (imageContainer != null)
                    ImageAppObject.SetImage(retrievedTexture2D);
            }
        }
    }

    private IEnumerator CR_GetImage(string imageURL, Image imageObject, RectTransform imageParentRectTransform)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageURL))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(webRequest.error);
                if (imageObject != null)
                {
                    imageObject.sprite = errorSprite;
                    LayoutRebuilder.ForceRebuildLayoutImmediate(imageParentRectTransform);
                }
            }
            else
            {
                Texture2D retrievedTexture2D = DownloadHandlerTexture.GetContent(webRequest);
                if (imageObject != null)
                {
                    imageObject.sprite = Sprite.Create(retrievedTexture2D, new Rect(0.0f, 0.0f, retrievedTexture2D.width, retrievedTexture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(imageParentRectTransform);
                }
            }
        }
    }
}