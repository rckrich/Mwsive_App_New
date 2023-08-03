using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogInDemoPresenter : Presenter
{
    private const string LOGIN_URL = "";

    public override void CallInteractor(params object[] list)
    {
        string email = (string)list[0];
        string password = (string)list[1];

        interactor.PerformSearch(LOGIN_URL, email, password);
    }

    public override void OnResult(params object[] list)
    {
        ScreenManager.instance.SetHheaderViewActive(false);
        ScreenManager.instance.ChangeView(ViewID.MainDemoViewModel);
    }
}
