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
                onlyone = true;
                try {
                    SpotifyPreviewAudioManager.instance.SetNoInternet(true);
                }
                catch(System.NullReferenceException e)
                {
                    Debug.Log("SpotifyPreviewAudioManager not found. " + e);
                }
            }
        }
        else
        {
            if (onlyone)
            {
                SpotifyPreviewAudioManager.instance.SetNoInternet(false);
                if (NewScreenManager.instance.GetCurrentView().GetComponent<SurfViewModel>())
                {
                    onlyone = false;
                    try
                    {
                        if (SurfController.instance.ReturnMain().GetComponent<SurfManager>().IsManagerEmpty())
                        {
                            AppManager.instance.StartAppProcess();
                        }
                    }
                    catch (System.NullReferenceException e)
                    {
                        Debug.Log("SurfController not found. " + e);
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
