using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TrackDetailsPanel : MonoBehaviour
{
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI spotifyID;
    public TextMeshProUGUI albumName;
    public Image trackPicture;

    private string mp3URL;

    public void OnClick_CopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = spotifyID.text;
    }

    public void OnClick_PlayAudioPreview()
    {
        if (!mp3URL.Equals(""))
            SpotifyPreviewAudioManager.instance.GetTrack(mp3URL);
    }

    public void Initialize(Track _track)
    {
        Track track = _track;
        displayName.text = track.name;
        spotifyID.text = track.id;
        albumName.text = track.album.name;

        mp3URL = track.preview_url;

        ImageManager.instance.GetImage(track.album.images[0].url, trackPicture, (RectTransform)this.transform);
    }
}
