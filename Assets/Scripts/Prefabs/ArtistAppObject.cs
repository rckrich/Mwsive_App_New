using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArtistAppObject : AppObject
{
    private const int IMAGES_NOT_NULL_INDEX = 0;

    public TextMeshProUGUI displayName;
    public Image artistPicture;

    private Artist artist;

    public override void Initialize(params object[] list)
    {
        Debug.Log("33333333");
        artist = (Artist)list[0];

        displayName.text = artist.name;

        if (artist.images != null)
        {
            if(artist.images.Count > IMAGES_NOT_NULL_INDEX)
                ImageManager.instance.GetImage(artist.images[0].url, artistPicture, (RectTransform)this.transform);
        }
    }

    public void OnClick_ArtistAppObject()
    {
        //NewScreenManager.instance.ChangeToSpawnedView("artist");
    }
}
