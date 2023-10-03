using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DsikAppObject : AppObject
{
    protected const int LIMIT_OF_DISPLAY_DISK = 100;
    protected const int DISK_FOR_A_SONG = 1;

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
                if(AppManager.instance.currentMwsiveUser.total_disks >= LIMIT_OF_DISPLAY_DISK)
                {
                    diskCounter.text = "+99";
                }
                else
                {
                    diskCounter.text = AppManager.instance.currentMwsiveUser.total_disks.ToString();
                }
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
            if (_event.type == null)
            {
                diskCircle.SetActive(true);
                if (AppManager.instance.currentMwsiveUser.total_disks >= LIMIT_OF_DISPLAY_DISK)
                {
                    diskCounter.text = "+99";
                }
                else
                {
                    diskCounter.text = AppManager.instance.currentMwsiveUser.total_disks.ToString();
                }
            }
            else if (_event.type.Equals("SUBSTRACT"))
            {
                AppManager.instance.currentMwsiveUser.total_disks -= DISK_FOR_A_SONG;
                if (AppManager.instance.currentMwsiveUser.total_disks >= LIMIT_OF_DISPLAY_DISK)
                {
                    diskCounter.text = "+99";
                }
                else
                {
                    diskCounter.text = AppManager.instance.currentMwsiveUser.total_disks.ToString();
                }
            }
            InvokeEvent<ReadDiskAppEvent>(new ReadDiskAppEvent());
        }
    }
}