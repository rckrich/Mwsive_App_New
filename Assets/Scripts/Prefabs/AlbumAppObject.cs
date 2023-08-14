using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlbumAppObject : AppObject
{
    private const int IMAGES_NOT_NULL_INDEX = 0;

    public TextMeshProUGUI displayName;
    public Image albumPicture;

    private Album album;


    public override void Initialize(params object[] list)
    {
        album = (Album)list[0];

        displayName.text = album.name;

        if (album.images != null)
        {
            if (album.images.Count > IMAGES_NOT_NULL_INDEX)
                ImageManager.instance.GetImage(album.images[0].url, albumPicture, (RectTransform)this.transform);
        }
    }

    public void OnClick_AlbumAppObject()
    {
        //NewScreenManager.instance.ChangeToSpawnedView("album");
    }
}
