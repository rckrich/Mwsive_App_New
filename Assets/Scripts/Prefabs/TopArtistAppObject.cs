using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopArtistAppObject : AppObject
{
    private const int IMAGES_NOT_NULL_INDEX = 0;

    public TextMeshProUGUI displayName;
    public Image artistPicture;
    public TextMeshProUGUI rank;

    private Artist artist;

    public override void Initialize(params object[] list)
    {
        artist = (Artist)list[0];
        if (artist.name.Length > 27)
        {
            string _text2 = "";
            for (int k = 0; k < 27; k++)
            {

                _text2 = _text2 + artist.name[k];

            }
            _text2 = _text2 + "...";
            displayName.text = _text2;
        }
        else
        {
            displayName.text = artist.name;
        }

        if (artist.images != null)
        {
            if(artist.images.Count > IMAGES_NOT_NULL_INDEX)
                ImageManager.instance.GetImage(artist.images[0].url, artistPicture, (RectTransform)this.transform);
        }

        rank.text = AppManager.instance.countTopArtist.ToString();
        AppManager.instance.countTopArtist++;
    }

    public void OnClick_ArtistAppObject()
    {
        //NewScreenManager.instance.ChangeToSpawnedView("artist");
    }
}
