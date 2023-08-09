using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneViewModel : ViewModel
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

   public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
    }
}
