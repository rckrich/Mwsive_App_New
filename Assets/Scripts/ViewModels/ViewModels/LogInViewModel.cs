using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class LogInViewModel : ViewModel
{
    public void OnClick_SpotifyButton()
    {
        LogInManager.instance.StartLogInProcess();
    }
}
