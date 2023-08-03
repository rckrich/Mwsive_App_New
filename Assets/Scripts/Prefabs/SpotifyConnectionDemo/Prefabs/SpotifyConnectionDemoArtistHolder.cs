using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpotifyConnectionDemoArtistHolder : MonoBehaviour
{
    public TextMeshProUGUI artistName;
    public Image trackPicture;

    public void Initialize(string _artistName)
    {
        artistName.text = _artistName;
    }

    public void Initialize(string _artistName, string _pictureURL)
    {
        artistName.text = _artistName;
        ImageManager.instance.GetImage(_pictureURL, trackPicture, (RectTransform)this.transform);
    }

    public void SetImage(string _pictureURL)
    {
        ImageManager.instance.GetImage(_pictureURL, trackPicture, (RectTransform)this.transform);
    }
}
