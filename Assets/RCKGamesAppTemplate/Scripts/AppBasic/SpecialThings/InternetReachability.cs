using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Revisa si el cel o la app tienen conexión a internet
public class InternetReachability : MonoBehaviour
{
    public GameObject screen_NoInternet;
    string m_ReachabilityText;

    private void OnEnable()
    {
        CheckInternet();
    }

    public void Btn_CheckInternet()
    {
        CheckInternet();
    }

    public void Btn_Close_NoInternetScreen()
    {
        Audio_Manager.Instance.PlaySFX(0);
        screen_NoInternet.SetActive(false);
    }

    void CheckInternet()
    {
        //Check if the device cannot reach the internet
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //Change the Text
            m_ReachabilityText = "Not Reachable.";
            screen_NoInternet.SetActive(true);
        }
        //Check if the device can reach the internet via a carrier data network
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            m_ReachabilityText = "Reachable via carrier data network.";
        }
        //Check if the device can reach the internet via a LAN
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            m_ReachabilityText = "Reachable via Local Area Network.";
        }

        //Output the network reachability to the console window
        Debug.Log("Internet : " + m_ReachabilityText);
    }
}
