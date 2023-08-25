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

    public TextMeshProUGUI followersText;
    public TextMeshProUGUI followedText;

    private string profileId = "";

    public override void Initialize(params object[] list)
    {
        if (list.Length > 0)
            profileId = (string)list[0];

        if (ProgressManager.instance.progress.userDataPersistance.spotify_userTokenSetted)
        {
            GetUserBasedOnEmptyProfileID(profileId);
        }
        else
        {
            CallPopUP(PopUpViewModelTypes.OptionChoice, "Neceseitas permiso", "Necesitas crear una cuenta de Mwsive para poder realizar está acción, presiona Crear Cuenta para hacer una.", "Crear Cuenta");
            PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);

            popUpViewModel.SetPopUpCancelAction(() => {
                OnClick_BackButtonSurf();
            });

            popUpViewModel.SetPopUpAction(() => {
                LogInManager.instance.StartLogInProcess(Callback_ProfileViewModelInitialize);
                NewScreenManager.instance.BackToPreviousView();
            });
        }

#if PLATFORM_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            AppManager.instance.SetAndroidBackAction(() => {
                if (finishedLoading) {
                    OnClick_BackButtonSurf();
                }
                AppManager.instance.SetAndroidBackAction(null);
            });
        }
#endif
    }

    private void Callback_ProfileViewModelInitialize(object[] list)
    {
        AppManager.instance.StartAppProcessFromOutside();
        GetUserBasedOnEmptyProfileID(profileId);
    }

    private void GetUserBasedOnEmptyProfileID(string _profileId)
    {
        if (_profileId.Equals(""))           
        {
            StartSearch();           
            //MwsiveConnectionManager.instance.GetFollowers(Callback_GetFollowers);
            //MwsiveConnectionManager.instance.GetFollowed(Callback_GetFollowed);
            MwsiveConnectionManager.instance.GetCurrentMwsiveUser(Callback_GetCurrentMwsiveUser);
        }
        else
        {
            MwsiveConnectionManager.instance.GetMwsiveUser(_profileId, Callback_GetMwsiveUser);                     
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
        if (profileId.Equals(""))
        {
            NewScreenManager.instance.ChangeToSpawnedView("miPlaylist");
            NewScreenManager.instance.GetCurrentView().GetComponent<MiPlaylistViewModel>().GetCurrentUserPlaylist();
            Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
        }
        else
        {
            NewScreenManager.instance.ChangeToSpawnedView("miPlaylist");
            NewScreenManager.instance.GetCurrentView().GetComponent<MiPlaylistViewModel>().GetUserPlaylist(profileId);
        }
        
    }
    public void OnClick_SpawnPopUpButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("popUp");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }
    
    private void Callback_GetUserProfile(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        bool profileImageSetted = false;

        ProfileRoot profileRoot = (ProfileRoot)_value[1];
        displayName.text = profileRoot.display_name;
        profileId = profileRoot.id;
       /* if (followersText.text.Equals("-"))
        {
            followersText.text = profileRoot.followers.ToString();
        }*/

        if (profileRoot.images != null)
        {
            if (profileRoot.images.Count > 0)
            {

                foreach (SpotifyImage image in profileRoot.images)
                {
                    if (image.height == 300 && image.width == 300)
                    {
                        ImageManager.instance.GetImage(image.url, profilePicture, (RectTransform)this.transform);
                        profileImageSetted = true;
                        break;
                    }
                }

                if (!profileImageSetted)
                    ImageManager.instance.GetImage(profileRoot.images[0].url, profilePicture, (RectTransform)this.transform);
            }
        }

        
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
        
       if(playlistRoot.items.Count < 6)
        {
            for (int i = 0; i < playlistRoot.items.Count; i++)
            {
                PlaylisHolder instance = GameObject.Instantiate(playlistHolderPrefab, instanceParent).GetComponent<PlaylisHolder>();
                instance.Initialize(playlistRoot.items[i].name, playlistRoot.items[i].id, playlistRoot.items[i].owner.display_name, playlistRoot.items[i].@public);
                if (playlistRoot.items[i].images != null && playlistRoot.items[i].images.Count > 0)
                    instance.SetImage(playlistRoot.items[i].images[0].url);
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                PlaylisHolder instance = GameObject.Instantiate(playlistHolderPrefab, instanceParent).GetComponent<PlaylisHolder>();
                instance.Initialize(playlistRoot.items[i].name, playlistRoot.items[i].id, playlistRoot.items[i].owner.display_name, playlistRoot.items[i].@public);
                if (playlistRoot.items[i].images != null && playlistRoot.items[i].images.Count > 0)
                    instance.SetImage(playlistRoot.items[i].images[0].url);
            }
        }    
        EndSearch();
    }

    private void Callback_GetFollowers(object[] _value)
    {
        MwsiveFollowersRoot mwsiveFollowersRoot = (MwsiveFollowersRoot)_value[1];

        followersText.text = (mwsiveFollowersRoot.followers != null) ? mwsiveFollowersRoot.followers.Count.ToString() : "-";

    }

    private void Callback_GetFollowed(object[] _value)
    {
        MwsiveFollowedRoot mwsiveFollowingRoot = (MwsiveFollowedRoot)_value[1];

        followedText.text = (mwsiveFollowingRoot.followed != null) ? mwsiveFollowingRoot.followed.Count.ToString() : "-";

    }

    public void OnClick_BackButtonSurf()
    {
        surfManager.SetActive(true);
        OpenView(ViewID.SurfViewModel);
#if PLATFORM_ANDROID
        AppManager.instance.ResetAndroidBackAction();
#endif
    }

    public void OnClick_OptionsButton()
    {
        OpenView(ViewID.OptionsViewModel);
    }

    public void OnClick_Followers()
    {
        NewScreenManager.instance.ChangeToSpawnedView("followers");
        if (profileId.Equals(""))
        {
            NewScreenManager.instance.GetCurrentView().GetComponent<FollowersViewModel>().ProfileIDReset_GetFollowers();
            
        }
        else
        {
            NewScreenManager.instance.GetCurrentView().GetComponent<FollowersViewModel>().GetFollowers(profileId);
        }
            
    }

    public void OnClick_Followed()
    {
        NewScreenManager.instance.ChangeToSpawnedView("followers");
        if (profileId.Equals(""))
        {
            NewScreenManager.instance.GetCurrentView().GetComponent<FollowersViewModel>().ProfileIDReset_GetFollowed();
            
        }
        else
        {
            NewScreenManager.instance.GetCurrentView().GetComponent<FollowersViewModel>().GetFollowed(profileId);
        }
    }
    

    public void OnClick_EditProfile()
    {
        OpenView(ViewID.EditProfileViewModel);
    }

    public void OnClick_Surf()
    {
        surfManager.SetActive(true);
        OpenView(ViewID.SurfViewModel);
        #if PLATFORM_ANDROID
        AppManager.instance.ResetAndroidBackAction();
#endif
    }

    private void OpenView(ViewID _value)
    {
        if (_value == ViewID.SurfViewModel)
            SurfManager.instance.canSwipe = true;

        NewScreenManager.instance.ChangeToMainView(_value, false);
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);

    }

    public void OnClick_BackButtonPrefab()
    {
        NewScreenManager.instance.BackToPreviousView();
    }

    public void OnClick_Follow()
    {
        MwsiveConnectionManager.instance.PostFollow(profileId);
        MwsiveConnectionManager.instance.GetMwsiveUser(profileId, Callback_GetMwsiveUser);
    }

    public void OnClick_Share(){
        Debug.Log(profileId);
        NativeShareManager.instance.OnClickShareMwsiveProfile(profileId);
    }

    public void Callback_GetCurrentMwsiveUser(object[] _value)
    {
        MwsiveUserRoot mwsiveUserRoot = (MwsiveUserRoot)_value[1];
        followersText.text = mwsiveUserRoot.user.total_followers.ToString();
        followedText.text = mwsiveUserRoot.user.total_followed.ToString();
        SpotifyConnectionManager.instance.GetCurrentUserProfile(Callback_GetUserProfile);
    }

    public void Callback_GetMwsiveUser(object[] _value)
    {
        MwsiveUserRoot mwsiveUserRoot = (MwsiveUserRoot)_value[1];
        followersText.text = mwsiveUserRoot.user.total_followers.ToString();
        followedText.text = mwsiveUserRoot.user.total_followed.ToString();
        SpotifyConnectionManager.instance.GetUserProfile(profileId, Callback_GetUserProfile);
    }
}

