using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileViewModel : ViewModel
{
    public TextMeshProUGUI displayName;
    public Image profilePicture;
    public GameObject playlistHolderPrefab;
    public Transform instanceParent;
    public GameObject surfManager;

    private string profileId = "";

    public override void Initialize(params object[] list)
    {
        if (list.Length > 0)
            profileId = (string)list[0];

        if (ProgressManager.instance.progress.userDataPersistance.spotify_userTokenSetted)
        {
            if (profileId.Equals(""))
            {
                // Significa que estamos queriendo abrir el perfil del usuario de la app
                SpotifyConnectionManager.instance.GetCurrentUserProfile(Callback_GetUserProfile);
            }
            else
            {
                // Significa que estamos queriendo abrir el perfil de otro usuario de la app
                SpotifyConnectionManager.instance.GetUserProfile(profileId, Callback_GetUserProfile);
            }
        }
        else
        {
            CallPopUP(PopUpViewModelTypes.OptionChoice, "Neceseitas permiso", "Necesitas crear una cuenta de Mwsive para poder realizar está acción, preisona Crear Cuenta para hacer una.", "Crear Cuenta");
            PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);

            popUpViewModel.SetPopUpCancelAction(() => {
                NewScreenManager.instance.BackToPreviousView();
                NewScreenManager.instance.BackToPreviousView();
            });

            popUpViewModel.SetPopUpAction(() => {
                LogInManager.instance.StartLogInProcess(Callback_ProfileViewModelInitialize);
                NewScreenManager.instance.BackToPreviousView();
            });
        }
    }

    private void Callback_ProfileViewModelInitialize(object[] list)
    {
        if (profileId.Equals(""))
        {
            SpotifyConnectionManager.instance.GetCurrentUserProfile(Callback_GetUserProfile);
        }
        else
        {
            SpotifyConnectionManager.instance.GetUserProfile(profileId, Callback_GetUserProfile);
        }
    }

    public void OnClick_SpawnInsigniasButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("insignias");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }

    public void OnClick_SpawnADNButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("adn");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }

    public void OnClick_SpawnMiPlaylistButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("miPlaylist");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }
    public void OnClick_SpawnPopUpButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("popUp");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }
    

    private void Callback_GetUserProfile(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        ProfileRoot profileRoot = (ProfileRoot)_value[1];
        displayName.text = profileRoot.display_name;
        profileId = profileRoot.id;
        ImageManager.instance.GetImage(profileRoot.images[0].url, profilePicture, (RectTransform)this.transform);
        GetCurrentUserPlaylists();
    }

    public void GetCurrentUserPlaylists()
    {
        if (!profileId.Equals(""))
            SpotifyConnectionManager.instance.GetUserPlaylists(profileId, Callback_OnClick_GetUserPlaylists);
    }

    private void Callback_OnClick_GetUserPlaylists(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        PlaylistRoot playlistRoot = (PlaylistRoot)_value[1];
        
        for(int i = 0; i < 6; i++)
        {
            PlaylisHolder instance = GameObject.Instantiate(playlistHolderPrefab, instanceParent).GetComponent<PlaylisHolder>();
            instance.Initialize(playlistRoot.items[i].name, playlistRoot.items[i].id, playlistRoot.items[i].owner.display_name, playlistRoot.items[i].@public);           
            if (playlistRoot.items[i].images != null && playlistRoot.items[i].images.Count > 0)
                instance.SetImage(playlistRoot.items[i].images[0].url);
        }
    }
    public void OnClick_BackButton()
    {
        OpenView(ViewID.SurfViewModel);
    }

    public void OnClick_OptionsButton()
    {
        OpenView(ViewID.OptionsViewModel);
    }

    public void OnClick_FollowFollowers()
    {
        OpenView(ViewID.FollowersViewModel);
    }

    public void OnClick_EditProfile()
    {
        OpenView(ViewID.EditProfileViewModel);
    }

    public void OnClick_Surf()
    {
        surfManager.SetActive(true);
        OpenView(ViewID.SurfViewModel);
    }
    private void OpenView(ViewID _value)
    {
        NewScreenManager.instance.ChangeToMainView(_value, false);
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);

    }
}

