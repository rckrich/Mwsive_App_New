using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileViewModel : ViewModel
{
    public List<GameObject> DNAButtons = new List<GameObject>();
    public TextMeshProUGUI displayName;
    public Image profilePicture;
    public GameObject playlistHolderPrefab;
    public Transform instanceParent;
    public GameObject surfManager;
    public Transform playlistContent;
    public ScrollRect principalScroll;
    public GameObject[] socialButtons;
    public TextMeshProUGUI followersText;
    public TextMeshProUGUI followedText;
    public Button followButton;
    public TextMeshProUGUI followButtonText;
    public Image followButtonImage;
    public Color followColor;
    public Color unfollowColor;
    public Color followTextColor;
    public Color unfollowTextColor;

    [Header("Social Media Button")]

    
    private bool isCurrentUserProfileView = true;
    private MwsiveUserRoot mwsiveUserRoot;

    private string profileId = "";
    private bool currentuser;
    private string externalUrl = "";
    private string tiktokUrl = "";
    private string instagramUrl = "";
    private string youtubeUrl = "";

    public override void Initialize(params object[] list)
    {
        if (list.Length > 0) {
            profileId = (string)list[0];
            isCurrentUserProfileView = false;
        }

        if (ProgressManager.instance.progress.userDataPersistance.spotify_userTokenSetted)
        {
            GetUserBasedOnEmptyProfileID(profileId);
        }
        else
        {
            if (isCurrentUserProfileView)
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
            else {
                GetUserBasedOnEmptyProfileID(profileId);
            }
        }

#if PLATFORM_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            AppManager.instance.SetAndroidBackAction(() => {
                if (finishedLoading) {
                    if (!isCurrentUserProfileView)
                    {
                        OnClick_BackButtonSurf();
                    }
                    else {
                        OnClick_BackButtonPrefab();
                    }
                }
                AppManager.instance.SetAndroidBackAction(null);
            });
        }
#endif

        FollowButtonInitilization();
        SocialButtonsInitilization();
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

    public void OnClick_SpawnADNButton(int identifier)
    {
        NewScreenManager.instance.ChangeToSpawnedView("adn");
        
        NewScreenManager.instance.GetCurrentView().gameObject.GetComponent<ADNDynamicScroll>().Initialize(identifier, currentuser, profileId);

    }
    public void OnClick_SetEditableDNA()
    {
        currentuser = true;
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

    public void GetCurrentUserPlaylists()
    {
        if (!profileId.Equals(""))
            SpotifyConnectionManager.instance.GetUserPlaylists(profileId, Callback_OnClick_GetUserPlaylists);
    }

    private void Callback_OnClick_GetUserPlaylists(object[] _value)
    {
        Clear();
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
        principalScroll.verticalNormalizedPosition = 1;
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
        NewScreenManager.instance.GetCurrentView().GetComponent<EditProfileViewModel>().Initialize();
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
        MwsiveConnectionManager.instance.PostFollow(profileId, Callback_PostFollow);
    }

    private void Callback_PostFollow(object[] _value)
    {
        MwsiveConnectionManager.instance.GetMwsiveUser(profileId, Callback_GetMwsiveUser);
    }

    public void OnClick_Share(){
        Debug.Log(profileId);
        NativeShareManager.instance.OnClickShareMwsiveProfile(profileId);
    }

    public void Callback_GetCurrentMwsiveUser(object[] _value)
    {
        mwsiveUserRoot = (MwsiveUserRoot)_value[1];

        if(mwsiveUserRoot.user.image != null)
            ImageManager.instance.GetImage(mwsiveUserRoot.user.image, profilePicture, (RectTransform)this.transform);

        followersText.text = mwsiveUserRoot.user.total_followers.ToString();
        followedText.text = mwsiveUserRoot.user.total_followed.ToString();

        displayName.text = mwsiveUserRoot.user.display_name;
        profileId = mwsiveUserRoot.user.platform_id;

        if (mwsiveUserRoot.user.user_links.Count != 0)
        {
            foreach (UserLink url in mwsiveUserRoot.user.user_links)
            {
                switch (url.type)
                {
                    case "TIK_TOK":
                        tiktokUrl = url.link;
                        socialButtons[1].SetActive(true);
                        break;
                    case "INSTAGRAM":
                        instagramUrl = url.link;
                        socialButtons[2].SetActive(true);
                        break;
                    case "YOU_TUBE":
                        youtubeUrl = url.link;
                        socialButtons[3].SetActive(true);
                        break;
                    case "EXTERNAL":
                        externalUrl = url.link;
                        socialButtons[0].SetActive(true);
                        break;
                }
            }
        }

        GetCurrentUserPlaylists();
        
    }

    public void Callback_GetMwsiveUser(object[] _value)
    {
        mwsiveUserRoot = (MwsiveUserRoot)_value[1];
        if (mwsiveUserRoot.user.image != null)
            ImageManager.instance.GetImage(mwsiveUserRoot.user.image, profilePicture, (RectTransform)this.transform);

        followersText.text = mwsiveUserRoot.user.total_followers.ToString();
        followedText.text = mwsiveUserRoot.user.total_followed.ToString();

        displayName.text = mwsiveUserRoot.user.display_name;
        profileId = mwsiveUserRoot.user.platform_id;

        GetCurrentUserPlaylists();

        if (mwsiveUserRoot.user.user_links.Count != 0)
        {
            foreach (UserLink url in mwsiveUserRoot.user.user_links)
            {
                if (url.link.Equals(""))
                {
                    switch (url.type)
                    {
                        case "TIK_TOK":
                            socialButtons[1].SetActive(false);
                            break;
                        case "INSTAGRAM":
                            socialButtons[2].SetActive(false);
                            break;
                        case "YOU_TUBE":
                            socialButtons[3].SetActive(false);
                            break;
                        case "EXTERNAL":
                            socialButtons[0].SetActive(false);
                            break;
                    }
                }
                else
                {
                    switch (url.type)
                    {
                        case "TIK_TOK":
                            tiktokUrl = url.link;
                            socialButtons[1].SetActive(true);
                            break;
                        case "INSTAGRAM":
                            instagramUrl = url.link;
                            socialButtons[2].SetActive(true);
                            break;
                        case "YOU_TUBE":
                            youtubeUrl = url.link;
                            socialButtons[3].SetActive(true);
                            break;
                        case "EXTERNAL":
                            externalUrl = url.link;
                            socialButtons[0].SetActive(true);
                            break;
                    }
                }
               
            }
        }

        FollowButtonInitilization();
    }


    public void OnClick_SurfButton(){
        MwsiveConnectionManager.instance.GetMwsiveUser(profileId, Callback_GetDNASeveralTracks);
        NewScreenManager.instance.ChangeToSpawnedView("surf");
        
        
    }

    public void Callback_GetDNASeveralTracks(object[] _value){
        MwsiveUserRoot mwsiveuser = (MwsiveUserRoot)_value[1];

        List<string> tracks = new List<string>();
        
        if(mwsiveUserRoot.user.user_lists != null){
            
            foreach (var item in mwsiveuser.user.user_lists)
            {
                if(item.type == "OST" || item.type == "LATEST_DISCOVERIES" || item.type == "ON_LOVE" || item.type == "GUILTY_PLEASURE" || item.type == "ON_REPEAT"){
                    foreach (var _track in item.items_list)
                    {
                        tracks.Add(_track);
                    }
                }
            }
        }
        
        if(tracks != null){
            NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().SurfProfileADN(profileId, tracks);
        }else{
            NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().SurfProfileADN(profileId);
        }
        
    }

    private void FollowButtonInitilization()
    {
        if (followButton == null) return;

        followButton.interactable = AppManager.instance.isLogInMode;

        if (AppManager.instance.isLogInMode)
        {
            MwsiveConnectionManager.instance.GetIsFollowing(profileId, Callback_GetIsFollowing);
        }
    }

    private void SocialButtonsInitilization()
    {
        if (socialButtons == null) return;

        if (socialButtons.Length <= 0) return;

        foreach(GameObject button in socialButtons)
        {
            button.SetActive(false);
        }
    }

    private void Callback_GetIsFollowing(object[] _value)
    {
        IsFollowingRoot isFollowingRoot = (IsFollowingRoot)_value[1];

        if (followButton == null) return;

        if (isFollowingRoot.is_following)
        {
            followButtonText.text = "Dejar de seguir";
            followButtonText.color = unfollowTextColor;
            followButtonImage.color = unfollowColor;
        }
        else {
            followButtonText.text = "Seguir";
            followButtonText.color = followTextColor;
            followButtonImage.color = followColor;
        }
    }

    private void ClearScrolls(Transform _scrolls)
    {
        foreach (Transform child in _scrolls.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void Clear()
    {
        ClearScrolls(playlistContent);
    }

    public void OnClick_TiktokButton()
    {
        if(!tiktokUrl.Equals(""))
            Application.OpenURL(tiktokUrl);
    }

    public void OnClick_InstagramButton()
    {
        if (!instagramUrl.Equals(""))
            Application.OpenURL(instagramUrl);
    }

    public void OnClick_YoutubeButton()
    {
        if (!youtubeUrl.Equals(""))
            Application.OpenURL(youtubeUrl);
    }

    public void OnClick_ExternalButton()
    {
        if (!externalUrl.Equals(""))
            Application.OpenURL(externalUrl);
    }
}

