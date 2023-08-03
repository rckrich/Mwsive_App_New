using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpsViewModel : ViewModel
{
    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
    }
}
