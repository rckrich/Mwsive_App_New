using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrackAppObject : AppObject
{
    private const int IMAGES_NOT_NULL_INDEX = 0;

    public TextMeshProUGUI displayName;
    public Image trackPicture;

    private Track track;


    public override void Initialize(params object[] list)
    {
        track = (Track)list[0];

        displayName.text = track.name;

        if (track.album == null) return;

        if (track.album.images != null)
        {
            if (track.album.images.Count > IMAGES_NOT_NULL_INDEX)
                ImageManager.instance.GetImage(track.album.images[0].url, trackPicture, (RectTransform)this.transform);
        }
    }

    public void OnClick_TrackAppObject()
    {
        //NewScreenManager.instance.ChangeToSpawnedView("track");
    }
}
