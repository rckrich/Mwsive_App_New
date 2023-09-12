using GG.Infrastructure.Utils.Swipe;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SurfViewModel : ViewModel
{
    public GameObject surfManager;
    public Image profilePicture;
    private string profileId;
    public ButtonSurfPlaylist buttonSurfPlaylist;
    public GameObject disk;
    public TextMeshProUGUI diskCount;

    private void OnEnable()
    {
        GetProfile();
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
            ImageManager.instance.GetImage(mwsiveUserRoot.user.image_url, profilePicture, (RectTransform)this.transform);
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
        CallPopUP(PopUpViewModelTypes.MessageOnly, "¿Qué son los discos?", "Cada vez que escuches una cancióin que te haga vibrar, puedes lanzar un disco para votar por tus favoritas y destacar en el ranking. (1 Disco = 1 Pik) " +
            "<br><b>Tus Piks: </b><br>"  +
            "En Mwsive tus piks nos ayudan a recomendar m�sica a otros crowd-surfers y as� descubrir juntos la música que hace olas.", "Aceptar");
        
    }

    public void OnClick_MyProfile()
    {
        surfManager.SetActive(false);
        SpotifyPreviewAudioManager.instance.StopTrack();
        NewScreenManager.instance.ChangeToMainView(ViewID.ProfileViewModel, true);
        NewScreenManager.instance.GetCurrentView().Initialize();
    }
}
