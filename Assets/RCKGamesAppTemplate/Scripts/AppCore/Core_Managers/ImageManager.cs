using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using TMPro;

public delegate void ImageDownloaderCallback(object[] _list);


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
    public Sprite errorSpriteProfile;

    public void GetImage(string imageURL, IImageDownloaderObject ImageAppObject)
    {
        if(imageURL == null)
        {
            ImageAppObject.SetImage(errorSprite);
        }
        StartCoroutine(CR_GetImage(imageURL, ImageAppObject));
    }

    public void GetImage(string imageURL, Image imageObject, RectTransform imageParentRectTransform, string _type = null, ImageDownloaderCallback _callback = null)
    {
        if (imageURL == null)
        {
            if (_type == null)
            {
                imageObject.sprite = errorSprite;
                return;
            }
            else
            {
                imageObject.sprite = errorSpriteProfile;
                return;
            }
           
        }
        StartCoroutine(CR_GetImage(imageURL, imageObject, imageParentRectTransform, _callback, _type));
    }

    private IEnumerator CR_GetImage(string imageURL, IImageDownloaderObject ImageAppObject, string _type = null)
    {
        GameObject imageContainer = ImageAppObject.GetImageContainer();

        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageURL))
        {
            yield return webRequest.SendWebRequest();
            if(webRequest.result == UnityWebRequest.Result.ProtocolError && _type.Equals("PROFILEIMAGE") || webRequest.result == UnityWebRequest.Result.ConnectionError && _type.Equals("PROFILEIMAGE"))
            {
                if (imageContainer != null)
                    ImageAppObject.SetImage(errorSpriteProfile);
            }
            else if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
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

    private IEnumerator CR_GetImage(string imageURL, Image imageObject, RectTransform imageParentRectTransform, ImageDownloaderCallback _callback = null, string _type = null)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageURL))
        {
            yield return webRequest.SendWebRequest();
            
            if (webRequest.result == UnityWebRequest.Result.ProtocolError && _type == null || webRequest.result == UnityWebRequest.Result.ConnectionError && _type == null)
            {
                if (imageObject != null)
                {
                    imageObject.sprite = errorSprite;
                    LayoutRebuilder.ForceRebuildLayoutImmediate(imageParentRectTransform);
                }
               
            }
            else if (webRequest.result == UnityWebRequest.Result.ProtocolError && _type.Equals("PROFILEIMAGE") || webRequest.result == UnityWebRequest.Result.ConnectionError && _type.Equals("PROFILEIMAGE"))
            {
                if (imageObject != null)
                {
                    imageObject.sprite = errorSpriteProfile;
                    LayoutRebuilder.ForceRebuildLayoutImmediate(imageParentRectTransform);
                }
            }
            else if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                if (imageObject != null)
                {
                    imageObject.sprite = errorSprite;
                    LayoutRebuilder.ForceRebuildLayoutImmediate(imageParentRectTransform);
                }
            }
            else
            {
                Texture2D retrievedTexture2D = DownloadHandlerTexture.GetContent(webRequest);
                try
                {
                    Sprite createdSprite = Sprite.Create(retrievedTexture2D, new Rect(0.0f, 0.0f, retrievedTexture2D.width, retrievedTexture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
                    if (imageObject != null)
                    {
                        imageObject.sprite = createdSprite;
                        LayoutRebuilder.ForceRebuildLayoutImmediate(imageParentRectTransform);
                    }
                    if (_callback != null)
                    {
                        _callback(new object[] { createdSprite });
                    }
                }
                catch (System.NullReferenceException)
                {
                    imageObject.sprite = errorSprite;
                }
            }
        }
    }
}