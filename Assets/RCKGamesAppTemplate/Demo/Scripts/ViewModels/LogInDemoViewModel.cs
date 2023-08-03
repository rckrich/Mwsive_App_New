using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogInDemoViewModel : ViewModel
{
    [Header("UI Object Reference")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    private void Start()
    {
        Initialize<LogInDemoViewModel, LogInDemoPresenter, LogInDemoInteractor>(this, null);
    }

    public void OnClick_LogIn()
    {
        if(CheckInput())
        {
            CallPresenter(emailInput.text, passwordInput.text);
        }
        else
        {
            CallPopUP(PopUpViewModelTypes.MessageOnly, "Advertencia", "Debes de llenar los campos para poder iniciar sesión");
        }
    }

    private bool CheckInput()
    {
        return !emailInput.text.Equals("") || !passwordInput.text.Equals("");
    }
}
