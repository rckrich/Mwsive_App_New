using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DsikAppObject : AppObject
{
    public GameObject diskCircle;
    public TextMeshProUGUI diskCounter;

    private void OnEnable()
    {
        AddEventListener<ChangeDiskAppEvent>(ChangedDisksEventListener);
        if(AppManager.instance.currentMwsiveUser != null)
        {
            if (AppManager.instance.isLogInMode)
            {
                diskCircle.SetActive(true);
                diskCounter.text = AppManager.instance.currentMwsiveUser.total_disks.ToString();
            }
        }
        

        
    }

    private void OnDisable()
    {
        RemoveEventListener<ChangeDiskAppEvent>(ChangedDisksEventListener);
    }

    private void ChangedDisksEventListener(ChangeDiskAppEvent _event)
    {
        if (AppManager.instance.isLogInMode)
        {
            diskCircle.SetActive(true);
            diskCounter.text = AppManager.instance.currentMwsiveUser.total_disks.ToString();
        }
    }

}
