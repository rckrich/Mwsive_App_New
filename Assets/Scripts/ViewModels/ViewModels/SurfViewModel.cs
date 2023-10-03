using GG.Infrastructure.Utils.Swipe;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SurfViewModel : ViewModel
{
    public Sprite image;
    public GameObject surfManager;
    public Image profilePicture;
    private string profileId;
    public ButtonSurfPlaylist buttonSurfPlaylist;
    public GameObject disk;
    public TextMeshProUGUI diskCount;

    private void Start()
    {
        GetProfile();
    }

    private void OnEnable()
    {
        AddEventListener<ChangeProfileSpriteAppEvent>(Listener_ChangeProfileSpriteAppEvent);
    }

    private void OnDisable()
    {
        RemoveEventListener<ChangeProfileSpriteAppEvent>(Listener_ChangeProfileSpriteAppEvent);
    }

    public void GetProfile()
    {

        if (ProgressManager.instance.progress.userDataPersistance.userTokenSetted)
            MwsiveConnectionManager.instance.GetCurrentMwsiveUser(Callback_GetCurrentUserProfile);
            
    }
    private void Callback_GetCurrentUserProfile(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        MwsiveUserRoot mwsiveUserRoot = (MwsiveUserRoot)_value[1];
        profileId = mwsiveUserRoot.user.platform_id;

        if(mwsiveUserRoot.user.image_url != null)
            ImageManager.instance.GetImage(mwsiveUserRoot.user.image_url, profilePicture, (RectTransform)this.transform, "PROFILEIMAGE");
    }

    public void OnClick_Profile()
    {
        surfManager.SetActive(false);
        SpotifyPreviewAudioManager.instance.StopTrack();
        NewScreenManager.instance.ChangeToSpawnedView("profile");
        NewScreenManager.instance.GetCurrentView().Initialize(profileId);
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }

    public void OnClick_Discos()
    {       
        CallPopUP(PopUpViewModelTypes.MessageOnly, "¿Qué son los discos?", "Cada vez que escuches una canción que te haga vibrar, puedes lanzar un disco para votar por tus favoritas y destacar en el ranking. <color=#7C7DF5>(1 Disco = 1 Pik)</color> ", "Aceptar", image);
#if PLATFORM_ANDROID
        PopUpViewModel currentPopUp = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
        AppManager.instance.SetAndroidBackAction(() => {
            currentPopUp.ExitButtonOnClick();
            this.SetAndroidBackAction();
        });
#endif   
    }

    public void OnClick_MyProfile()
    {
        surfManager.SetActive(false);
        SpotifyPreviewAudioManager.instance.StopTrack();
        NewScreenManager.instance.ChangeToMainView(ViewID.ProfileViewModel, true);
        NewScreenManager.instance.GetCurrentView().Initialize();
    }

    private void Listener_ChangeProfileSpriteAppEvent(ChangeProfileSpriteAppEvent _event)
    {
        profilePicture.sprite = _event.newSprite;
    }

    public override void SetAndroidBackAction()
    {
#if PLATFORM_ANDROID
        AppManager.instance.ResetAndroidBackAction();
#endif
    }
}
