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

    private SurfMiPlaylistViewModel surfMiPlaylistViewModel;


    public override void Initialize(params object[] list)
    {
        if(list.Length > 0)
        {
            surfMiPlaylistViewModel = (SurfMiPlaylistViewModel)list[0];
        }

        StartSearch();
        SpotifyConnectionManager.instance.GetCurrentUserProfile(Callback_GetUserProfile);
    }

    private void Callback_GetUserProfile(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        ProfileRoot profileRoot = (ProfileRoot)_value[1];
        id = profileRoot.id;

        EndSearch();
    }

    public void OnClick_CreatePlaylist()
    {
        if (!id.Equals("") && !playlistNameIputField.text.Equals("") && !playlistDescriptionIputField.Equals(""))
        {
            StartSearch();
            SpotifyConnectionManager.instance.CreatePlaylist(id, Callback_OnCLick_CreatePlaylist, playlistNameIputField.text, playlistDescriptionIputField, true);
        }
        else
        {
            //TODO Spawn little message downwards telling user to "put name to create playlist"
        }
    }

    private void Callback_OnCLick_CreatePlaylist(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        if(surfMiPlaylistViewModel != null)
        {
            surfMiPlaylistViewModel.RefreshPlaylistList();
            surfMiPlaylistViewModel = null;
        }

        OnClick_BackButton();

        EndSearch();

        //TODO Preguntarle a landy si va a ser una mensaje de exito o cambiaría la playlist actual
    }

    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
    }
}
