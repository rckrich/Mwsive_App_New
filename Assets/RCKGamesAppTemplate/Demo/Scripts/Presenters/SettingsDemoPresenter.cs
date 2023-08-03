using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsDemoPresenter : Presenter
{
    private const string POST_LOG_OUT = "https://axessbyaxe.com/api/auth/logout";

    public override void CallInteractor(params object[] list)
    {
        interactor.PerformSearch(POST_LOG_OUT, ProgressManager.instance.progress.userDataPersistance.access_token);
        return;
    }

    public override void OnResult(params object[] list)
    {
        viewModel.DisplayOnResult();

        ScreenManager.instance.ChangeView(ViewID.SplashDemoViewModel);
        ScreenManager.instance.GetView(ViewID.SplashDemoViewModel).Initialize();
    }

}
