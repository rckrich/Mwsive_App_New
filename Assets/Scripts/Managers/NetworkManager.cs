using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private bool onlyone = false;

    private void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {         
            if (!onlyone)
            {
                UIMessage.instance.UIMessageInstanciate("No hay internet. Ocurrió un error de conexión");
                SpotifyPreviewAudioManager.instance.enabled = false;
                onlyone = true;
            }
        }
        else
        {
            if (onlyone)
            {

                SpotifyPreviewAudioManager.instance.enabled = true;
                if (NewScreenManager.instance.GetCurrentView().GetComponent<SurfViewModel>())
                {
                    onlyone = false;
                    if (SurfController.instance.ReturnMain().GetComponent<SurfManager>().IsManagerEmpty())
                    {
                        AppManager.instance.StartAppProcess();                      
                    }
                    
                }
                else
                {
                    onlyone = false;
                }
            }
        }
    }
}
