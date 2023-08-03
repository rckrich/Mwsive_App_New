using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdnViewModel : ViewModel
{
   
    public void OnClick_BackButton()
    {
        Debug.Log("Botón");
        NewScreenManager.instance.BackToPreviousView();
    }
}
