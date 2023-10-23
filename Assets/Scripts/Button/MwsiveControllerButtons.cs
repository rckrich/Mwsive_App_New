using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MwsiveControllerButtons : MonoBehaviour
{
    public SurfManager Surf;
    public PF_SurfManager Pf_Surf;
    public float AnimationDuration;

    bool ShareCallbackEnd = true;
    
    public void OnClickOlaButton() {

        if(Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (!AppManager.instance.isLogInMode)
            {
                UIMessage.instance.UIMessageInstanciate("Debes inciar sesión para poder realizar esta acción");
                return;
            }

            GetPrefabFromSurf().GetComponentInChildren<MwsiveButton>().OnClickOlaButton(AnimationDuration, GetPrefabFromSurf().GetComponent<ButtonSurfPlaylist>().trackID, GetTime());
        }
        else
        {
            UIMessage.instance.UIMessageInstanciate("El dispotivo no se encuentra conectado");
        }

        
    }

    public void OnClickAddToPlaylistButton() {

        if(Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (!AppManager.instance.isLogInMode)
            {
                UIMessage.instance.UIMessageInstanciate("Debes inciar sesión para poder realizar esta acción");
                return;
            }
            /*try
            {
                SurfController.instance.ReturnCurrentView().GetComponent<SurfManager>().SideScrollSuccess(true);
            }
            catch (System.NullReferenceException)
            {
                SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().SideScrollSuccess(true);
            }*/

            GetPrefabFromSurf().GetComponent<ButtonSurfPlaylist>().AddToPlaylistButton(GetPrefabFromSurf().GetComponent<ButtonSurfPlaylist>().trackID, GetTime());
        }
        else
        {
            UIMessage.instance.UIMessageInstanciate("El dispotivo no se encuentra conectado");
        }
    }

    public void OnClickCompartirButton() {

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            GameObject Instance = GetPrefabFromSurf();
            Instance.GetComponentInChildren<MwsiveButton>().OnClickCompartirButton(AnimationDuration);
            NativeShareManager.instance.OnClickShareMwsiveSong(Instance.GetComponent<ButtonSurfPlaylist>().trackID, Instance.GetComponentInChildren<MwsiveButton>().GetIsItCompartirActive());

        }
        else
        {
            UIMessage.instance.UIMessageInstanciate("El dispotivo no se encuentra conectado");
        }


    }

    public bool IsItOlaColorButtonActive() {
        
        return GetPrefabFromSurf().GetComponentInChildren<MwsiveButton>().GetIsItOlaActive();
    }

    public void OnClickPlayStopButton()
    {

        GetPrefabFromSurf().GetComponent<ButtonSurfPlaylist>().OnClic_StopAudioPreview();
    }

    public void OnClick_Spotify()
    {

        GetPrefabFromSurf().GetComponent<ButtonSurfPlaylist>().OnClick_PlayOnSpotify();
        
    }
    public void OnClick_Friends()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            GetPrefabFromSurf().GetComponent<ButtonSurfPlaylist>().OnClick_UserThatVoted();
            SurfController.instance.ReturnCurrentView().SetActive(false);
        }
        else
        {
            UIMessage.instance.UIMessageInstanciate("El dispotivo no se encuentra conectado");
        }
    }


    public GameObject GetPrefabFromSurf()
    {
        if(Surf != null) {
            GameObject Instance = Surf.GetCurrentPrefab();
            return Instance;
        }
        else
        {
            GameObject Instance = Pf_Surf.GetCurrentPrefab();
            return Instance;
        }
    }

    public float GetTime()
    {
        if (Surf != null)
        {
            
            return Surf.time;
        }
        else
        {
           return Pf_Surf.time;
        }
    }
}
