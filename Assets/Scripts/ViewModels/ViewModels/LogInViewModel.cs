using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LogInViewModel : ViewModel
{
    private Sprite logInErrorSprite;

    private void Start()
    {
        logInErrorSprite = Resources.Load<Sprite>("Zona_Peligrosa");
    }

    public void OnClick_SpotifyButton()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            LogInManager.instance.StartLogInProcess();
        }
        else {
            NewScreenManager.instance.ChangeToMainView(ViewID.PopUpViewModel, true);
            PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
            popUpViewModel.Initialize(PopUpViewModelTypes.MessageOnly, "Advertencia", "No hay conexión en estos momentos. Volver a intentar más tarde.", "Aceptar", logInErrorSprite);
            return;
        }
    }

    public void OnClick_NoLogInButton()
    {

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            GameObject.FindAnyObjectByType<NoLogInConnectionManager>().StartConnection((object[] _value) => { SceneManager.LoadScene("MainScene_Ricardo"); });
        }
        else
        {
            NewScreenManager.instance.ChangeToMainView(ViewID.PopUpViewModel, true);
            PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
            popUpViewModel.Initialize(PopUpViewModelTypes.MessageOnly, "Advertencia", "No hay conexión en estos momentos. Volver a intentar más tarde.", "Aceptar", logInErrorSprite);
            return;
        }
    }
}
