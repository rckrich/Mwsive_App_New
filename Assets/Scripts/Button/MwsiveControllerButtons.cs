using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MwsiveControllerButtons : MonoBehaviour
{
    public SurfManager Surf;
    public float AnimationDuration;
    
    public void OnClickOlaButton() {

        GameObject Instance = Surf.GetCurrentPrefab();
        Instance.GetComponentInChildren<MwsiveButton>().OnClickOlaButton(AnimationDuration);
    }
    public void OnClickAddToPlaylistButton() {
        GameObject Instance = Surf.GetCurrentPrefab();
        Instance.GetComponent<ButtonSurfPlaylist>().AddToPlaylistButton();
    }
    public void OnClickCompartirButton() {
        GameObject Instance = Surf.GetCurrentPrefab();
        Instance.GetComponentInChildren<MwsiveButton>().OnClickCompartirButton(AnimationDuration);
        NativeShareManager.instance.OnClickShareMwsiveSong(Instance.GetComponent<ButtonSurfPlaylist>().trackID, Instance.GetComponentInChildren<MwsiveButton>().GetIsItCompartirActive() );
    }

    public bool IsItOlaColorButtonActive() {
        GameObject Instance = Surf.GetCurrentPrefab();
        return Instance.GetComponentInChildren<MwsiveButton>().GetIsItOlaActive();
    }

    public void OnClickPlayStopButton()
    {
        GameObject Instance = Surf.GetCurrentPrefab();
        Instance.GetComponent<ButtonSurfPlaylist>().OnClic_StopAudioPreview();
    }

    public void OnClick_Spotify()
    {
        GameObject Instance = Surf.GetCurrentPrefab();
        Instance.GetComponent<ButtonSurfPlaylist>().OnClick_PlayOnSpotify();
    }
}
