using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SurfMiPlaylistViewModel : ViewModel
{
    // Start is called before the first frame update
    public GameObject playlistHolderPrefab;
    public Transform instanceParent;
    public ScrollRect scrollRect;
    public float end;
    public int offset = 21;
    int onlyone = 0;
    //public HolderManager holderManager;
    public List<Image> imagenes;
    public GameObject shimmer;

    private bool profilePictureException = false;
    private Sprite logInErrorSprite;

    void Start()
    {
        InitializePlaylistList();
        logInErrorSprite = Resources.Load<Sprite>("Zona_Peligrosa");
    }

    private void InitializePlaylistList()
    {
        profilePictureException = false;

        SpotifyPreviewAudioManager.instance.StopTrack();

        if (AppManager.instance.isLogInMode)
        {
            if (shimmer != null)
                shimmer.SetActive(true);
            SpotifyConnectionManager.instance.GetCurrentUserPlaylists(Callback_OnClick_GetCurrentUserPlaylists);

#if PLATFORM_ANDROID
            if (Application.platform == RuntimePlatform.Android)
            {
                AppManager.instance.SetAndroidBackAction(() =>
                {
                    if (finishedLoading)
                    {
                        OnClick_BackButton();
                    }
                    AppManager.instance.SetAndroidBackAction(null);
                });
            }
#endif

        }
        else
        {
            CallPopUP(PopUpViewModelTypes.OptionChoice, "Neceseitas permiso", "Necesitas crear una cuenta de Mwsive para poder realizar está acción, presiona Crear Cuenta para hacer una.", "Crear Cuenta");
            PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
#if PLATFORM_ANDROID
            PopUpViewModel currentPopUp = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
            AppManager.instance.SetAndroidBackAction(() =>
            {
                currentPopUp.ExitButtonOnClick();
                this.SetAndroidBackAction();
            });
#endif
            popUpViewModel.SetPopUpCancelAction(() =>
            {
                NewScreenManager.instance.BackToPreviousView();
                OnClick_BackButton();
            });

            popUpViewModel.SetPopUpAction(() =>
            {
                LogInManager.instance.StartLogInProcess(Callback_MiPlaylistViewModelInitialize);
                NewScreenManager.instance.BackToPreviousView();
            });

#if PLATFORM_ANDROID
            if (Application.platform == RuntimePlatform.Android)
            {
                AppManager.instance.SetAndroidBackAction(() =>
                {
                    if (finishedLoading)
                    {
                        NewScreenManager.instance.BackToPreviousView();
                        OnClick_BackButton();
                    }
                    AppManager.instance.SetAndroidBackAction(null);
                });
            }
#endif
        }
    }

    private void Callback_MiPlaylistViewModelInitialize(object[] _value)
    {
        AppManager.instance.StartAppProcessFromOutside(Callback_StartAppProcess_SurMiPlaylistViewModelInitialize);
    }

    private void Callback_StartAppProcess_SurMiPlaylistViewModelInitialize(object[] value)
    {
        if (shimmer != null)
            shimmer.SetActive(true);
        profilePictureException = true;
        SpotifyConnectionManager.instance.GetCurrentUserPlaylists(Callback_OnClick_GetCurrentUserPlaylists);
        MwsiveConnectionManager.instance.GetCurrentMwsiveUser(Callback_GetCurrentMwsiveUser);
    }

    private void Callback_GetCurrentMwsiveUser(object[] _value)
    {
        if (DoesMswiveUserExists((long)_value[0]))
            return;

        MwsiveUserRoot mwsiveUserRoot = (MwsiveUserRoot)_value[1];

        if (mwsiveUserRoot.user.image_url != null)
        {
            if (profilePictureException)
                ImageManager.instance.GetImage(mwsiveUserRoot.user.image_url, MainSurfProfileManager.instance.GetProfileImage(), (RectTransform)this.transform, "PROFILEIMAGE");
        }
    }

    private void Callback_OnClick_GetCurrentUserPlaylists(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        PlaylistRoot playlistRoot = (PlaylistRoot)_value[1];

        foreach (Item item in playlistRoot.items)
        {
            if (ProgressManager.instance.progress.userDataPersistance.current_playlist.Equals(item.id))
            {
                playlistHolderPrefab.GetComponent<Image>().enabled = true;

            }
            else
            {
                playlistHolderPrefab.GetComponent<Image>().enabled = false;
            }
            SurfMiplaylistHolder instance = GameObject.Instantiate(playlistHolderPrefab, instanceParent).GetComponent<SurfMiplaylistHolder>();
            instance.Initialize(item.name, item.id, item.owner.display_name, item.@public, item.description, item.external_urls);
            if (!item.@public) { instance.PublicTrue(); }
            if (item.images != null && item.images.Count > 0) { instance.SetImage(item.images[0].url); }
        }

        if (shimmer != null)
            shimmer.SetActive(false);
    }

    public void OnReachEnd()
    {
        if (onlyone == 0)
        {
            if (scrollRect.verticalNormalizedPosition <= end)
            {
                if(shimmer != null)
                    shimmer.SetActive(true);
                SpotifyConnectionManager.instance.GetCurrentUserPlaylists(Callback_GetMoreUserPlaylists, 20, offset);
                offset += 20;
                onlyone = 1;
            }
        }
    }

    private void Callback_GetMoreUserPlaylists(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        PlaylistRoot playlistRoot = (PlaylistRoot)_value[1];

        foreach (Item item in playlistRoot.items)
        {
            if (ProgressManager.instance.progress.userDataPersistance.current_playlist == item.id)
            {
                playlistHolderPrefab.GetComponent<Image>().enabled = true;

            }
            else
            {
                playlistHolderPrefab.GetComponent<Image>().enabled = false;
            }
            SurfMiplaylistHolder instance = GameObject.Instantiate(playlistHolderPrefab, instanceParent).GetComponent<SurfMiplaylistHolder>();
            instance.Initialize(item.name, item.id, item.owner.display_name, item.@public, item.description, item.external_urls);
            if (!item.@public) { instance.PublicTrue(); }
            if (item.images != null && item.images.Count > 0)
                instance.SetImage(item.images[0].url);

        }
        onlyone = 0;

        if (shimmer != null)
            shimmer.SetActive(false);
    }


    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
        NewScreenManager.instance.GetCurrentView().SetAndroidBackAction();
        SurfManager.instance.SetActive(true);
    }

    public void OnClick_SpawnCrearPlaylistButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("crearPlaylist");
        NewScreenManager.instance.GetCurrentView().GetComponent<CreatePlaylistViewModel>().Initialize(this);
    }

    public void RefreshPlaylistList()
    {
        for (int i = 0; i < instanceParent.transform.childCount; i++)
        {
            Destroy(instanceParent.transform.GetChild(i).gameObject);
        }

        InitializePlaylistList();
    }
    public override void SetAndroidBackAction()
    {
#if PLATFORM_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            AppManager.instance.SetAndroidBackAction(() =>
            {
                if (finishedLoading)
                {
                    OnClick_BackButton();
                }
            });
        }
#endif
    }

    private bool DoesMswiveUserExists(long _webCode)
    {
        if (_webCode.Equals(WebCallsUtils.NOT_FOUND_RESPONSE_CODE) || _webCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
        {
            EndSearch();
            DebugLogManager.instance.DebugLog(_webCode);
            NewScreenManager.instance.GetCurrentView().EndSearch();
            NewScreenManager.instance.ChangeToMainView(ViewID.PopUpViewModel, true);
            PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
            popUpViewModel.Initialize(PopUpViewModelTypes.MessageOnly, "Advertencia", "Este usuario ya no se encuentra registrado en Mwsive. Regresa a la pantalla anterior", "Aceptar", logInErrorSprite);
            popUpViewModel.SetPopUpAction(() =>
            {
                NewScreenManager.instance.BackToPreviousView();
                OnClick_BackButton();
            });

            return true;
        }
        else
        {
            return false;
        }
    }
}
