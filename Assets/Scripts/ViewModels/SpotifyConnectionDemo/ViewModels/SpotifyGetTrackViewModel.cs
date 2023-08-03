using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpotifyGetTrackViewModel : MonoBehaviour
{
    public TMP_InputField trackIDInputField;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI spotifyID;
    public TextMeshProUGUI albumName;
    public Image trackPicture;

    private string mp3URL;

    public void OnClick_GetTrack()
    {
        if (!trackIDInputField.text.Equals(""))
            SpotifyConnectionManager.instance.GetTrack(trackIDInputField.text, Callback_OnCLick_GetTrack);
    }

    public void OnClick_CopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = spotifyID.text;
    }

    public void OnClick_PlayAudioPreview()
    {
        if (!mp3URL.Equals(""))
            SpotifyPreviewAudioManager.instance.GetTrack(mp3URL);
    }

    private void Callback_OnCLick_GetTrack(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        TrackRoot trackRoot = (TrackRoot)_value[1];
        displayName.text = trackRoot.name;
        spotifyID.text = trackRoot.id;
        albumName.text = trackRoot.album.name;

        mp3URL = trackRoot.preview_url;

        ImageManager.instance.GetImage(trackRoot.album.images[0].url, trackPicture, (RectTransform)this.transform);
    }
}
