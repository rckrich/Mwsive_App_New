using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurfViewModel : ViewModel
{
    // Start is called before the first frame update

    public GameObject surfManager;
    public Image profilePicture;
    private string profileId;
    void Start()
    {
        surfManager.SetActive(true);
        SpotifyConnectionManager.instance.GetCurrentUserProfile(Callback_GetUserProfile);
    }
    private void OnEnable()
    {
        
    }
    private void Callback_GetUserProfile(object[] _value)
    {
        //if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        ProfileRoot profileRoot = (ProfileRoot)_value[1];
        profileId = profileRoot.id;
        ImageManager.instance.GetImage(profileRoot.images[0].url, profilePicture, (RectTransform)this.transform);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick_Profile()
    {
        NewScreenManager.instance.ChangeToSpawnedView("profile");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }

    public void OnClick_Discos()
    {
        NewScreenManager.instance.ChangeToMainView(ViewID.PopUpViewModel, true);
        PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(PopUpViewModelTypes.MessageOnly, "¿Qué son los discos?", "Cada vez que escuches una canción que te haga vibrar, puedes lanzar un disco para votar por tus favoritas y destacar en el ranking. (1 Disco = 1 Pik)" +
            " <br><b>Tus Piks: </b><br>En Mwsive tus piks nos ayudan a recomendar música a otros crowd-surfers y así descubrir juntos la música que hace olas.", "Aceptar");
        popUpViewModel.SetPopUpAction(() => { NewScreenManager.instance.BackToPreviousView(); });
        CallWaitAFrame();
        /*CallPopUP(PopUpViewModelTypes.MessageOnly, "¿Qué son los discos?", "Cada vez que escuches una canción que te haga vibrar, puedes lanzar un disco para votar por tus favoritas y destacar en el ranking. (1 Disco = 1 Pik) " +
            " < b >Tus Piks: < /b >"  +
            "En Mwsive tus piks nos ayudan a recomendar música a otros crowd-surfers y así descubrir juntos la música que hace olas.", "Aceptar");*/
    }

    public void OnClick_MyProfile()
    {
        NewScreenManager.instance.ChangeToMainView(ViewID.ProfileViewModel, true);
        
    }

    
}
