using GG.Infrastructure.Utils.Swipe;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurfViewModel : ViewModel
{
    public GameObject surfManager;
    public Image profilePicture;
    private string profileId;
    public ButtonSurfPlaylist buttonSurfPlaylist;


    

    public void GetProfile()
    {
        if (ProgressManager.instance.progress.userDataPersistance.userTokenSetted)
            SpotifyConnectionManager.instance.GetCurrentUserProfile(Callback_GetUserProfile);
    }
    private void Callback_GetUserProfile(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        ProfileRoot profileRoot = (ProfileRoot)_value[1];
        profileId = profileRoot.id;
        ImageManager.instance.GetImage(profileRoot.images[0].url, profilePicture, (RectTransform)this.transform);
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
