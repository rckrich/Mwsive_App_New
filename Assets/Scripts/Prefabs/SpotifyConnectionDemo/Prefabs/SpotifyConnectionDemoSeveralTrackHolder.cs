using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpotifyConnectionDemoSeveralTrackHolder : MonoBehaviour
{
    public TextMeshProUGUI trackName;
    public TextMeshProUGUI artistName;
    public Image trackPicture;

    private Track track;

    public void Initialize(Track _track)
    {
        track = _track;

        trackName.text = _track.name;
        artistName.text = _track.artists[0].name;
        if (track.album.images != null && track.album.images.Count > 0)
            ImageManager.instance.GetImage(_track.album.images[0].url, trackPicture, (RectTransform)this.transform);
    }

    public void SetImage(string _pictureURL)
    {
        ImageManager.instance.GetImage(_pictureURL, trackPicture, (RectTransform)this.transform);
    }

    public void OnCLick_SeeDetails()
    {
        if (track != null) {
            TrackDetailsPanel trackDetailsPanel = GameObject.FindObjectOfType<SpotifyGetSeveralTracksViewModel>().trackDetailsPanel;
            trackDetailsPanel.Initialize(track);
            trackDetailsPanel.gameObject.SetActive(true);
        }
    }
}
