using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MwsiveControllerButtons : MonoBehaviour
{
    public SurfManager Surf;
    public PF_SurfManager Pf_Surf;
    public float AnimationDuration;
    
    public void OnClickOlaButton() {


        GetPrefabFromSurf().GetComponentInChildren<MwsiveButton>().OnClickOlaButton(AnimationDuration, GetPrefabFromSurf().GetComponent<ButtonSurfPlaylist>().trackID, GetTime());
    }

    public void OnClickAddToPlaylistButton() {

        GetPrefabFromSurf().GetComponent<ButtonSurfPlaylist>().AddToPlaylistButton(GetPrefabFromSurf().GetComponent<ButtonSurfPlaylist>().trackID, GetTime());
    }

    public void OnClickCompartirButton() {
        GameObject Instance = GetPrefabFromSurf();
        Instance.GetComponentInChildren<MwsiveButton>().OnClickCompartirButton(AnimationDuration);
        NativeShareManager.instance.OnClickShareMwsiveSong(Instance.GetComponent<ButtonSurfPlaylist>().trackID, Instance.GetComponentInChildren<MwsiveButton>().GetIsItCompartirActive() );
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
        
        //GetPrefabFromSurf().GetComponent<ButtonSurfPlaylist>().();
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
