using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_SurfViewModel : ViewModel
{
    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
    }
}
