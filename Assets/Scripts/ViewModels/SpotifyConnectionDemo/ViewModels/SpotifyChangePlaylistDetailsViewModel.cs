using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpotifyChangePlaylistDetailsViewModel : MonoBehaviour
{
    public TMP_InputField playlistIDInputField;
    public TMP_InputField playlistNameIputField;
    public TMP_InputField playlistDescriptionIputField;
    public TextMeshProUGUI playListName;
    public TextMeshProUGUI creatorName;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI spotifyID;
    public TextMeshProUGUI isPublicText;
    public Image playListPicture;

    private bool isPublic = true;

    public void OnClick_ChangePlaylistDetails()
    {
        if (!playlistIDInputField.text.Equals("") && !playlistNameIputField.text.Equals("") && !playlistDescriptionIputField.text.Equals(""))
            SpotifyConnectionManager.instance.ChangePlaylistDetails(playlistIDInputField.text, Callback_OnCLick_ChangePlaylistDetails, playlistNameIputField.text, playlistDescriptionIputField.text, isPublic);
    }

    public void OnValueChanged(bool _value)
    {
        isPublic = _value;
    }

    public void OnClick_CopyToClipboard()
    {
        if (!spotifyID.text.Equals("...") || !spotifyID.text.Equals(""))
            GUIUtility.systemCopyBuffer = spotifyID.text;
    }

    private void Callback_OnCLick_ChangePlaylistDetails(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        SpotifyConnectionManager.instance.GetPlaylist(playlistIDInputField.text, Callback_OnCLick_GetPlaylist);
    }

    private void Callback_OnCLick_GetPlaylist(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        SearchedPlaylist searchedPlaylist = (SearchedPlaylist)_value[1];
        playListName.text = searchedPlaylist.name;
        creatorName.text = searchedPlaylist.owner.display_name;
        spotifyID.text = searchedPlaylist.id;
        descriptionText.text = searchedPlaylist.description;
        isPublicText.text = searchedPlaylist.@public.ToString();

        if (searchedPlaylist.images != null && searchedPlaylist.images.Count > 0)
            ImageManager.instance.GetImage(searchedPlaylist.images[0].url, playListPicture, (RectTransform)this.transform);
    }
}
