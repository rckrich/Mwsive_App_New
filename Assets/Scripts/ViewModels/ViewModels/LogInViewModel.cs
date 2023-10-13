using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LogInViewModel : ViewModel
{
    public void OnClick_SpotifyButton()
    {
        LogInManager.instance.StartLogInProcess();
    }

    public void OnClick_NoLogInButton()
    {
      GameObject.FindAnyObjectByType<NoLogInConnectionManager>().StartConnection((object[] _value) => { SceneManager.LoadScene("MainScene"); });
    }
}
