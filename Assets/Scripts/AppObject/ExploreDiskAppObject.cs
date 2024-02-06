using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreDiskAppObject : DsikAppObject
{
    private void OnEnable()
    {
        AddEventListener<ReadDiskAppEvent>(ChangedDisksEventListener);
        if (AppManager.instance.currentMwsiveUser != null)
        {
            if (AppManager.instance.isLogInMode)
            {
                diskCircle.SetActive(true);
                if (AppManager.instance.currentMwsiveUser.total_disks >= LIMIT_OF_DISPLAY_DISK)
                {
                    diskCounter.text = "+99";
                }
                else
                {
                    if (AppManager.instance.currentMwsiveUser.total_disks < 0)
                    {
                        diskCounter.text = "0";
                    }
                    else
                    {
                        diskCounter.text = AppManager.instance.currentMwsiveUser.total_disks.ToString();
                    }
                }
            }
        }
    }

    private void OnDisable()
    {
        RemoveEventListener<ReadDiskAppEvent>(ChangedDisksEventListener);
    }

    private void ChangedDisksEventListener(ReadDiskAppEvent _event)
    {
        if (AppManager.instance.isLogInMode)
        {
            if (AppManager.instance.isLogInMode)
            {
                diskCircle.SetActive(true);
                if (AppManager.instance.currentMwsiveUser.total_disks >= LIMIT_OF_DISPLAY_DISK)
                {
                    diskCounter.text = "+99";
                }
                else
                {
                    if (AppManager.instance.currentMwsiveUser.total_disks < 0)
                    {
                        diskCounter.text = "0";
                    }
                    else
                    {
                        diskCounter.text = AppManager.instance.currentMwsiveUser.total_disks.ToString();
                    }
                }
            }
        }
    }
}
