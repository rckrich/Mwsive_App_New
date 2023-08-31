using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdvertisingAppObject : AppObject
{
    public Image advertisingImage;

    private Advertising advertising;

    public override void Initialize(params object[] list)
    { 
        advertising = (Advertising)list[0];

        if(advertising.image_url != null)
            ImageManager.instance.GetImage(advertising.image_url, advertisingImage, (RectTransform)this.transform);
    }

    public void OnClick_Advertising()
    {
        Application.OpenURL(advertising.link);

        if (AppManager.instance.isLogInMode)
        {
            MwsiveConnectionManager.instance.PostSaveAdvertisementClick(AppManager.instance.profileID, advertising.id.ToString());
        }
        else
        {
            MwsiveConnectionManager.instance.PostSaveAdvertisementClick("", advertising.id.ToString());
        }
    }

}
