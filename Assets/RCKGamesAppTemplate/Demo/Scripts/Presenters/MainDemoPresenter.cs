using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDemoPresenter : Presenter
{
    private const string GET_EXAMPLES = "https://axessbyaxe.com/api/store/";

    public override void CallInteractor(params object[] list)
    {
        interactor.PerformSearch(GET_EXAMPLES);
    }
}
