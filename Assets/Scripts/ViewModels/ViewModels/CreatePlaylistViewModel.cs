using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreatePlaylistViewModel : ViewModel
{
    // Start is called before the first frame update
    public TMP_InputField playlistNameIputField;
    public string playlistDescriptionIputField = "Creado por Mwsive";
    public string id;


    private void Start()
    {
        SpotifyConnectionManager.instance.GetCurrentUserProfile(Callback_GetUserProfile);
    }
    private void Callback_GetUserProfile(object[] _value)
    {
        //  if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        ProfileRoot profileRoot = (ProfileRoot)_value[1];
        id = profileRoot.id;
        
    }
    public void OnClick_CreatePlaylist()
    {
       
        if (!id.Equals("") && !playlistNameIputField.text.Equals("") && !playlistDescriptionIputField.Equals(""))
            SpotifyConnectionManager.instance.CreatePlaylist(id, Callback_OnCLick_CreatePlaylist, playlistNameIputField.text, playlistDescriptionIputField, true);

        
    }

    private void Callback_OnCLick_CreatePlaylist(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

      /*  CreatedPlaylistRoot createdPlaylistRoot = (CreatedPlaylistRoot)_value[1];
        playListName.text = createdPlaylistRoot.name;
        creatorName.text = createdPlaylistRoot.owner.display_name;
        descriptionText.text = createdPlaylistRoot.description;
        spotifyID.text = createdPlaylistRoot.id;
        isPublicText.text = createdPlaylistRoot.@public.ToString();

        if (createdPlaylistRoot.images != null && createdPlaylistRoot.images.Count > 0)
            ImageManager.instance.GetImage(createdPlaylistRoot.images[0].url, playListPicture, (RectTransform)this.transform);*/
    }

    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
    }
}
