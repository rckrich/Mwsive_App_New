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
                Debug.Log("AAA");
                UIMessage.instance.UIMessageInstanciate("No hay internet. Ocurrió un error de conexión.");
                onlyone = true;
                try {
                    SpotifyPreviewAudioManager.instance.enabled = false;
                }
                catch (System.NullReferenceException)
                {
                    Debug.Log("SpotifyPreviewAudioManager instance is null");
                }
            }
        }
        else
        {
            if (onlyone)
            {
                try
                {
                    SpotifyPreviewAudioManager.instance.enabled = true;
                }
                catch (System.NullReferenceException)
                {
                    Debug.Log("SpotifyPreviewAudioManager instance is null");
                }

                if (NewScreenManager.instance.GetCurrentView().GetComponent<SurfViewModel>())
                {
                    Debug.Log("BBB");
                    onlyone = false;
                    if (SurfController.instance.ReturnMain().GetComponent<SurfManager>().IsManagerEmpty())
                    {
                        AppManager.instance.StartAppProcess();                      
                    }
                }
                else
                {
                    Debug.Log("CCC");
                    onlyone = false;
                }
            }
        }
    }
}
