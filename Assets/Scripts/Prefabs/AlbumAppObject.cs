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
    public TextMeshProUGUI displayAutor;
    
    private string artists;
    private Album album;
    private string spotifyID;


    public override void Initialize(params object[] list)
    {
        album = (Album)list[0];

        if (album.name.Length > 27)
        {
            string _text2 = "";
            for (int k = 0; k < 27; k++)
            {

                _text2 = _text2 + album.name[k];

            }
            _text2 = _text2 + "...";
            displayName.text = _text2;
        }
        else
        {
            displayName.text = album.name;
        }

        artists = "";
        foreach (Artist artist in album.artists) { artists += artist.name + ", "; }

        if (artists.Length > 27)
        {
            string _text2 = "";
            for (int k = 0; k < 27; k++)
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

        if (album.images != null)
        {
            if (album.images.Count > IMAGES_NOT_NULL_INDEX)
                ImageManager.instance.GetImage(album.images[0].url, albumPicture, (RectTransform)this.transform);
        }

        spotifyID = album.id;
    }

    public void OnClick_AlbumAppObject()
    {
        NewScreenManager.instance.ChangeToSpawnedView("album");
        NewScreenManager.instance.GetCurrentView().GetComponent<AlbumViewModel>().id = spotifyID;
    }

    
}
