using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreDisksManager : Manager
{
    private const int LIMIT_OF_DISPLAY_DISK = 100;

    public ExploreDiskAppObject diskAppObject;

    private void OnEnable()
    {
        AddEventListener<ReadDiskAppEvent>(ChangedDisksEventListener);
    }

    private void OnDisable()
    {
        RemoveEventListener<ReadDiskAppEvent>(ChangedDisksEventListener);
    }

    private void ChangedDisksEventListener(ReadDiskAppEvent _event)
    {
        if (AppManager.instance.isLogInMode)
        {
            diskAppObject.diskCircle.SetActive(true);
            if (AppManager.instance.currentMwsiveUser.total_disks >= LIMIT_OF_DISPLAY_DISK)
            {
                diskAppObject.diskCounter.text = "+99";
            }
            else
            {
                diskAppObject.diskCounter.text = AppManager.instance.currentMwsiveUser.total_disks.ToString();
            }
        }
    }
}
