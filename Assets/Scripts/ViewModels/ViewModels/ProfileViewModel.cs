using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileViewModel : ViewModel
{
    private const int LIMIT_OF_BADGES = 3;

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
    [Header("Badges References")]
    public GameObject BadgesHolderPrefab;
    public Transform BadgesContent;
    public GameObject noBadges;
    [Header("Social Media Button")]


    private bool isCurrentUserProfileView = true;
    private bool isLogInFromOutside = false;

    private MwsiveUserRoot mwsiveUserRoot;

    private string profileId = "";
    private bool currentuser;
    private string externalUrl = "";
    private string tiktokUrl = "";
    private string instagramUrl = "";
    private string youtubeUrl = "";
    private bool areTrackBadges = true;
    private bool areEngagementBadges = true;

    private void OnEnable()
    {
        AddEventListener<ChangeProfileSpriteAppEvent>(Listener_ChangeProfileSpriteAppEvent);
    }

    private void OnDisable()
    {
        RemoveEventListener<ChangeProfileSpriteAppEvent>(Listener_ChangeProfileSpriteAppEvent);
    }

    public override void Initialize(params object[] list)
    {
        if (list.Length > 0)
        {
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
#if PLATFORM_ANDROID
                PopUpViewModel currentPopUp = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
                AppManager.instance.SetAndroidBackAction(() => {
                    currentPopUp.ExitButtonOnClick();
                    this.SetAndroidBackAction();
                });
#endif
                popUpViewModel.SetPopUpCancelAction(() => {
                    OnClick_BackButtonSurf();
                });

                popUpViewModel.SetPopUpAction(() => {
                    LogInManager.instance.StartLogInProcess(Callback_ProfileViewModelInitialize);
                    NewScreenManager.instance.BackToPreviousView();
                });
            }
            else
            {
                GetUserBasedOnEmptyProfileID(profileId);
            }
        }

#if PLATFORM_ANDROID
        SetAndroidBackAction();
#endif

        FollowButtonInitilization();
        SocialButtonsInitilization();
        if (BadgesContent != null)
            ClearScrollsBadges(BadgesContent);
        GetBadgesCall();
    }

    private void Callback_ProfileViewModelInitialize(object[] list)
    {
        AppManager.instance.StartAppProcessFromOutside(Callback_StartAppProcess_ProfileViewModelInitialize);
    }

    private void Callback_StartAppProcess_ProfileViewModelInitialize(object[] _value)
    {
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
        NewScreenManager.instance.GetCurrentView().GetComponent<InsigniasViewModel>().Initialize(profileId);
    }

    public void OnClick_SpawnADNButton(int identifier)
    {
        NewScreenManager.instance.ChangeToSpawnedView("adn");

        NewScreenManager.instance.GetCurrentView().gameObject.GetComponent<ADNDynamicScroll>().Initialize(identifier, currentuser, profileId);
        NewScreenManager.instance.GetCurrentView().GetComponent<AdnViewModel>().Initialize();
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
            NewScreenManager.instance.GetCurrentView().GetComponent<MiPlaylistViewModel>().Initialize();
        }
        else
        {
            NewScreenManager.instance.ChangeToSpawnedView("miPlaylist");
            NewScreenManager.instance.GetCurrentView().GetComponent<MiPlaylistViewModel>().GetUserPlaylist(profileId);
            NewScreenManager.instance.GetCurrentView().GetComponent<MiPlaylistViewModel>().Initialize();
        }

    }
    public void OnClick_SpawnPopUpButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("popUp");
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

        if (playlistRoot.items.Count < 6)
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

    public void OnClick_BackButtonSurf()
    {
        principalScroll.verticalNormalizedPosition = 1;
        surfManager.SetActive(true);
        OpenView(ViewID.SurfViewModel);
        NewScreenManager.instance.GetCurrentView().SetAndroidBackAction();
    }

    public void OnClick_OptionsButton()
    {
        OpenView(ViewID.OptionsViewModel);
        NewScreenManager.instance.GetCurrentView().GetComponent<OptionsViewModel>().GetSettings();
        NewScreenManager.instance.GetCurrentView().GetComponent<OptionsViewModel>().Initialize();
    }

    public void OnClick_Followers()
    {
        NewScreenManager.instance.ChangeToSpawnedView("followers");
        NewScreenManager.instance.GetCurrentView().SetAndroidBackAction();

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
        NewScreenManager.instance.GetCurrentView().SetAndroidBackAction();

        if (profileId.Equals(""))
        {
            NewScreenManager.instance.GetCurrentView().GetComponent<FollowersViewModel>().ProfileIDReset_GetFollowed();
            NewScreenManager.instance.GetCurrentView().GetComponent<FollowersViewModel>().Initialize();

        }
        else
        {
            NewScreenManager.instance.GetCurrentView().GetComponent<FollowersViewModel>().GetFollowed(profileId);
            NewScreenManager.instance.GetCurrentView().GetComponent<FollowersViewModel>().Initialize();
        }
    }


    public void OnClick_EditProfile()
    {
        NewScreenManager.instance.ChangeToMainView(ViewID.EditProfileViewModel, true);
        NewScreenManager.instance.GetCurrentView().GetComponent<EditProfileViewModel>().Initialize(!isCurrentUserProfileView);
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

    }

    public void OnClick_BackButtonPrefab()
    {
        NewScreenManager.instance.BackToPreviousView();
        NewScreenManager.instance.GetCurrentView().SetAndroidBackAction();
    }

    public void OnClick_Follow()
    {
        if (AppManager.instance.isLogInMode)
        {
            if (AppManager.instance.currentMwsiveUser.platform_id.Equals(profileId))
            {
                NewScreenManager.instance.ChangeToSpawnedView("editarPerfil");
                NewScreenManager.instance.GetCurrentView().GetComponent<EditProfileViewModel>().Initialize(true);
            }
            else
            {
                MwsiveConnectionManager.instance.PostFollow(profileId, Callback_PostFollow);
            }
        }
        else
        {
            CallPopUP(PopUpViewModelTypes.OptionChoice, "Neceseitas permiso", "Necesitas crear una cuenta de Mwsive para poder realizar está acción, presiona Crear Cuenta para hacer una.", "Crear Cuenta");
            PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
#if PLATFORM_ANDROID
            PopUpViewModel currentPopUp = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
            AppManager.instance.SetAndroidBackAction(() => {
                currentPopUp.ExitButtonOnClick();
                this.SetAndroidBackAction();
            });
#endif
            popUpViewModel.SetPopUpCancelAction(() => {
                NewScreenManager.instance.BackToPreviousView();

            });

            popUpViewModel.SetPopUpAction(() => {
                LogInManager.instance.StartLogInProcess(Callback_PostFollowInitialize);
                NewScreenManager.instance.BackToPreviousView();
            });
        }
    }

    private void Callback_PostFollowInitialize(object[] _value)
    {
        AppManager.instance.StartAppProcessFromOutside(Callback_PostStartAppProcess_PostFollow);
    }

    private void Callback_PostStartAppProcess_PostFollow(object[] _value)
    {
        isLogInFromOutside = true;
        MwsiveConnectionManager.instance.GetMwsiveUser(profileId, Callback_GetMwsiveUser);
    }

    private void Callback_PostFollow(object[] _value)
    {
        MwsiveConnectionManager.instance.GetMwsiveUser(profileId, Callback_GetMwsiveUser);
    }

    public void OnClick_Share()
    {
        NativeShareManager.instance.OnClickShareMwsiveProfile(profileId);
    }

    public void Callback_GetCurrentMwsiveUser(object[] _value)
    {
        mwsiveUserRoot = (MwsiveUserRoot)_value[1];

        if (mwsiveUserRoot.user.image_url != null)
            ImageManager.instance.GetImage(mwsiveUserRoot.user.image_url, profilePicture, (RectTransform)this.transform, "PROFILEIMAGE");

        followersText.text = mwsiveUserRoot.user.total_followers.ToString();
        followedText.text = mwsiveUserRoot.user.total_followed.ToString();

        displayName.text = mwsiveUserRoot.user.display_name;
        profileId = mwsiveUserRoot.user.platform_id;

        if (mwsiveUserRoot.user.user_links.Count != 0)
        {
            foreach (UserLink url in mwsiveUserRoot.user.user_links)
            {
                if (url.link == null)
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

        GetCurrentUserPlaylists();

    }

    public void Callback_GetMwsiveUser(object[] _value)
    {
        mwsiveUserRoot = (MwsiveUserRoot)_value[1];
        if (mwsiveUserRoot.user.image_url != null)
            ImageManager.instance.GetImage(mwsiveUserRoot.user.image_url, profilePicture, (RectTransform)this.transform, "PROFILEIMAGE");

        followersText.text = mwsiveUserRoot.user.total_followers.ToString();
        followedText.text = mwsiveUserRoot.user.total_followed.ToString();

        displayName.text = mwsiveUserRoot.user.display_name;
        profileId = mwsiveUserRoot.user.platform_id;

        GetCurrentUserPlaylists();
        if (mwsiveUserRoot.user.user_links.Count != 0)
        {
            foreach (UserLink url in mwsiveUserRoot.user.user_links)
            {
                if (url.link == null)
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


    public void OnClick_SurfButton()
    {
        MwsiveConnectionManager.instance.GetMwsiveUser(profileId, Callback_GetDNASeveralTracks);
        NewScreenManager.instance.ChangeToSpawnedView("surf");
        NewScreenManager.instance.GetCurrentView().GetComponent<PF_SurfViewModel>().Initialize();
    }

    public void Callback_GetDNASeveralTracks(object[] _value)
    {
        MwsiveUserRoot mwsiveuser = (MwsiveUserRoot)_value[1];

        List<string> tracks = new List<string>();

        if (mwsiveUserRoot.user.user_lists != null)
        {

            foreach (var item in mwsiveuser.user.user_lists)
            {
                if (item.type == "ON_REPEAT")
                {
                    foreach (var _track in item.items_list)
                    {
                        tracks.Add(_track);
                    }
                }
            }

            foreach (var item in mwsiveuser.user.user_lists)
            {
                if (item.type == "GUILTY_PLEASURE")
                {
                    foreach (var _track in item.items_list)
                    {
                        tracks.Add(_track);
                    }
                }
            }

            foreach (var item in mwsiveuser.user.user_lists)
            {
                if (item.type == "ON_LOVE")
                {
                    foreach (var _track in item.items_list)
                    {
                        tracks.Add(_track);
                    }
                }
            }

            foreach (var item in mwsiveuser.user.user_lists)
            {
                if (item.type == "LATEST_DISCOVERIES")
                {
                    foreach (var _track in item.items_list)
                    {
                        tracks.Add(_track);
                    }
                }
            }

            foreach (var item in mwsiveuser.user.user_lists)
            {
                if (item.type == "OST")
                {
                    foreach (var _track in item.items_list)
                    {
                        tracks.Add(_track);
                    }
                }
            }

        }

        if (tracks != null)
        {
            NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().SurfProfileADN(profileId, tracks);
        }
        else
        {
            NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().SurfProfileADN(profileId);
        }

    }

    private void FollowButtonInitilization()
    {
        if (AppManager.instance.isLogInMode)
        {
            if (AppManager.instance.currentMwsiveUser.platform_id.Equals(profileId))
            {
                followButtonText.text = "Editar perfil";
            }
            else
            {
                MwsiveConnectionManager.instance.GetIsFollowing(profileId, Callback_GetIsFollowing);
            }
        }
        else
        {
            followButtonText.text = "Seguir";
        }
    }

    private void SocialButtonsInitilization()
    {
        if (socialButtons == null) return;

        if (socialButtons.Length <= 0) return;

        foreach (GameObject button in socialButtons)
        {
            button.SetActive(false);
        }
    }

    private void Callback_GetIsFollowing(object[] _value)
    {
        IsFollowingRoot isFollowingRoot = (IsFollowingRoot)_value[1];

        if (isLogInFromOutside && !isFollowingRoot.is_following && !AppManager.instance.currentMwsiveUser.platform_id.Equals(profileId))
        {
            isLogInFromOutside = false;
            MwsiveConnectionManager.instance.PostFollow(profileId, Callback_PostFollow);
            return;
        }

        if (followButton == null) return;

        if (isFollowingRoot.is_following)
        {
            followButtonText.text = "Dejar de seguir";
            followButtonText.color = unfollowTextColor;
            followButtonImage.color = unfollowColor;
        }
        else
        {
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
        if (!tiktokUrl.Equals(""))
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

    private void Listener_ChangeProfileSpriteAppEvent(ChangeProfileSpriteAppEvent _event)
    {
        profilePicture.sprite = _event.newSprite;
    }

    public override void SetAndroidBackAction()
    {
#if PLATFORM_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            AppManager.instance.SetAndroidBackAction(() => {
                if (finishedLoading)
                {
                    if (isCurrentUserProfileView)
                    {
                        OnClick_BackButtonSurf();
                    }
                    else
                    {
                        OnClick_BackButtonPrefab();

                    }
                }
            });
        }
#endif
    }

    public void OnClick_NoBadges()
    {
        CallPopUP(PopUpViewModelTypes.MessageOnly, "<br>Has mas PIKS", "Has más PIK para conseguir Badges", "Cerrar");
#if PLATFORM_ANDROID
        PopUpViewModel currentPopUp = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
        AppManager.instance.SetAndroidBackAction(() => {
            currentPopUp.ExitButtonOnClick();
            this.SetAndroidBackAction();
        });
#endif
    }

    public void GetBadgesCall()
    {
        string _profileid = profileId;
        if (_profileid.Equals(""))
            _profileid = AppManager.instance.currentMwsiveUser.platform_id;
        MwsiveConnectionManager.instance.GetBadges(_profileid, "engagement", Callback_GetBadgesCall, 0, 3);
    }

    private void Callback_GetBadgesCall(object[] _value)
    {
        MwsiveBadgesRoot badgesRoot = (MwsiveBadgesRoot)_value[1];
        int count = 0;
        if (badgesRoot.badges.Count > 0)
        {
            if (badgesRoot != null)
            {

                foreach (Badge badge in badgesRoot.badges)
                {
                    BadgeHolder instance = GameObject.Instantiate(BadgesHolderPrefab, BadgesContent).GetComponent<BadgeHolder>();
                    instance.Initialize(badge);
                    count++;
                }
            }
            areEngagementBadges = true;
        }
        else
        {
            areEngagementBadges = false;
        }

        if (count < LIMIT_OF_BADGES)
        {
            count = LIMIT_OF_BADGES - count;
            string _profileid = profileId;

            if (_profileid.Equals(""))
                _profileid = AppManager.instance.currentMwsiveUser.platform_id;
            MwsiveConnectionManager.instance.GetBadges(_profileid, "track", Callback_GetBadgesTrack, 0, count);

        }
    }

    private void Callback_GetBadgesTrack(object[] _value)
    {
        MwsiveBadgesRoot badgesRoot = (MwsiveBadgesRoot)_value[1];

        if (badgesRoot.badges.Count > 0)
        {
            if (badgesRoot != null)
            {
                foreach (Badge badge in badgesRoot.badges)
                {
                    BadgeHolder instance = GameObject.Instantiate(BadgesHolderPrefab, BadgesContent).GetComponent<BadgeHolder>();
                    instance.Initialize(badge);
                }
            }

        }
        else
        {
            areTrackBadges = false;
        }

        TurnOn_NoBadges();
    }

    private void TurnOn_NoBadges()
    {

        if (!areEngagementBadges && !areTrackBadges)
        {
            noBadges.SetActive(true);
        }
        else
        {
            noBadges.SetActive(false);
        }


    }

    private void ClearScrollsBadges(Transform _scrolls)
    {
        for (int i = 1; i < _scrolls.childCount; i++)
        {
            Destroy(_scrolls.GetChild(i).transform.gameObject);
        }
    }
}

