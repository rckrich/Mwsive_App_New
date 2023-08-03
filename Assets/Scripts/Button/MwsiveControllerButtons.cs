using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MwsiveControllerButtons : MonoBehaviour
{
    public SurfManager Surf;
    public float AnimationDuration;
    public void OnClickOlaButton(){
        
        GameObject Instance = Surf.GetCurrentPrefab();
        Instance.GetComponentInChildren<MwsiveButton>().OnClickOlaButton(AnimationDuration);
    }
    public void OnClickAñadirButton(){
        GameObject Instance = Surf.GetCurrentPrefab();
        Instance.GetComponentInChildren<MwsiveButton>().OnClickAñadirButton(AnimationDuration);
    }
    public void OnClickCompartirButton(){
        GameObject Instance = Surf.GetCurrentPrefab();
        Instance.GetComponentInChildren<MwsiveButton>().OnClickCompartirButton(AnimationDuration);
    }

    public bool IsItOlaColorButtonActive(){
        GameObject Instance = Surf.GetCurrentPrefab();
        return Instance.GetComponentInChildren<MwsiveButton>().GetIsItOlaActive();
    }
}
