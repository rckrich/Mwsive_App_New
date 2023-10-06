using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private bool onlyone = false;
    void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (!onlyone)
            {
                UIMessage.instance.UIMessageInstanciate("Ocurrió un error de conexión");
                onlyone = true;
            }
        }

        if (onlyone)
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                onlyone = false;
                if (NewScreenManager.instance.GetCurrentView().GetComponent<SurfViewModel>())
                {
                    if (SurfController.instance.ReturnMain().GetComponent<SurfManager>().IsManagerEmpty())
                    {
                        AppManager.instance.StartAppProcess();
                    }
                }

            }
        }
    }
}
