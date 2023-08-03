using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpotifyCreatePlaylistViewModel : MonoBehaviour
{
    public TMP_InputField userIDInputField;
    public TMP_InputField playlistNameIputField;
    public TMP_InputField playlistDescriptionIputField;
    public TextMeshProUGUI playListName;
    public TextMeshProUGUI creatorName;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI spotifyID;
    public TextMeshProUGUI isPublicText;
    public Image playListPicture;

    private bool isPublic = true;

    public void OnClick_CreatePlaylist()
    {
        if (!userIDInputField.text.Equals("") && !playlistNameIputField.text.Equals("") && !playlistDescriptionIputField.text.Equals(""))
            SpotifyConnectionManager.instance.CreatePlaylist(userIDInputField.text, Callback_OnCLick_CreatePlaylist, playlistNameIputField.text, playlistDescriptionIputField.text, isPublic);
    }

    public void OnValueChanged(bool _value)
    {
        isPublic = _value;
    }

    public void OnClick_CopyToClipboard()
    {
        if(!spotifyID.text.Equals("...") || !spotifyID.text.Equals(""))
            GUIUtility.systemCopyBuffer = spotifyID.text;
    }

    private void Callback_OnCLick_CreatePlaylist(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        CreatedPlaylistRoot createdPlaylistRoot = (CreatedPlaylistRoot)_value[1];
        playListName.text = createdPlaylistRoot.name;
        creatorName.text = createdPlaylistRoot.owner.display_name;
        descriptionText.text = createdPlaylistRoot.description;
        spotifyID.text = createdPlaylistRoot.id;
        isPublicText.text = createdPlaylistRoot.@public.ToString();

        if(createdPlaylistRoot.images != null && createdPlaylistRoot.images.Count > 0)
            ImageManager.instance.GetImage(createdPlaylistRoot.images[0].url, playListPicture, (RectTransform)this.transform);
    }
}
