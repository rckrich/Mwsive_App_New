using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpotifyConnectionDemoTrackHolder : MonoBehaviour
{
    public TextMeshProUGUI trackName;
    public TextMeshProUGUI artistName;
    public Image trackPicture;

    private string trackSpotifyID;

    public void Initialize(string _trackName, string _artistName, string _trackSpotifyID)
    {
        trackName.text = _trackName;
        artistName.text = _artistName;

        trackSpotifyID = _trackSpotifyID;
    }

    public void Initialize(string _trackName, string _artistName, string _pictureURL, string _trackSpotifyID)
    {
        trackName.text = _trackName;
        artistName.text = _artistName;

        trackSpotifyID = _trackSpotifyID;

        ImageManager.instance.GetImage(_pictureURL, trackPicture, (RectTransform)this.transform);
    }

    public void SetImage(string _pictureURL)
    {
        ImageManager.instance.GetImage(_pictureURL, trackPicture, (RectTransform)this.transform);
    }

    public void OnCLick_CopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = trackSpotifyID;
    }
}
