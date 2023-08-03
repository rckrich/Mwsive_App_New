using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpotifyGetCurrentUserProfileViewModel : MonoBehaviour
{
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI spotifyID;
    public TextMeshProUGUI countryName;
    public TextMeshProUGUI emailName;
    public Image profilePicture;


    public void OnClick_GetCurrentUserProfile()
    {
        SpotifyConnectionManager.instance.GetCurrentUserProfile(Callback_OnCLick_GetUserProfile);
    }

    public void OnClick_CopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = spotifyID.text;
    }

    private void Callback_OnCLick_GetUserProfile(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        ProfileRoot profileRoot =(ProfileRoot)_value[1];
        displayName.text = profileRoot.display_name;
        spotifyID.text = profileRoot.id;
        countryName.text = profileRoot.country;
        emailName.text = profileRoot.email;

        if(profileRoot.images != null && profileRoot.images.Count > 0)
            ImageManager.instance.GetImage(profileRoot.images[0].url, profilePicture, (RectTransform)this.transform);
    }
}
