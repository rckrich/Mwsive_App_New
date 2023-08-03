using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDemoViewModel : ScrollViewModel
{
    private void Start()
    {
        Initialize<MainDemoViewModel, MainDemoPresenter, MainDemoInteractor>(this, null);
    }

    public override void DisplayOnResult(params object[] list)
    {
        InstanceableExampleRoot instanceableExampleRoot = (InstanceableExampleRoot)list[0];

        instanceables.Clear();

        foreach (InstanceableExample instanceableExample in instanceableExampleRoot.examples)
        {
            instanceables.Add(instanceableExample);
        }

        InstanceObjects<DemoInstaceableAppObject>(instanceables);

        EndSearch();

        CallWaitAFrame();
    }
}
