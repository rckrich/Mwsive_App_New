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
    public TextMeshProUGUI displayAutor;

    private string artists;
    private Track track;
    private string trackID;


    public override void Initialize(params object[] list)
    {
        track = (Track)list[0];

        if (track.name.Length > 20)
        {
            string _text2 = "";
            for (int k = 0; k < 20; k++)
            {
                _text2 = _text2 + track.name[k];
            }
            _text2 = _text2 + "...";
            displayName.text = _text2;
        }
        else
        {
            displayName.text = track.name;
        }

        artists = "";
        foreach (Artist artist in track.artists) { artists += artist.name + ", "; }

        if (artists.Length > 16)
        {
            string _text2 = "";
            for (int k = 0; k < 16; k++)
            {

                _text2 = _text2 + artists[k];

            }
            _text2 = _text2 + "...";
            displayAutor.text = _text2;
        }
        else
        {
            displayAutor.text = artists;
        }

        if (track.album == null) return;

        if (track.album.images != null)
        {
            if (track.album.images.Count > IMAGES_NOT_NULL_INDEX)
                ImageManager.instance.GetImage(track.album.images[0].url, trackPicture, (RectTransform)this.transform);
        }

        trackID = track.id;
    }

    public void OnClick_TrackAppObject()
    {
        
        NewScreenManager.instance.ChangeToSpawnedView("cancion");
        NewScreenManager.instance.GetCurrentView().GetComponent<TrackViewModel>().trackID = trackID;
    }
}
