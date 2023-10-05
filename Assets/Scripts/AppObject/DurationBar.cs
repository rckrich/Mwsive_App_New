using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DurationBar : AppObject
{
    

    [HideInInspector]
    public Image durationImage;
    public bool canPlay = false;
    public bool CheckforPoints;
    private ButtonSurfPlaylist SurfPlaylist;
    private PF_SurfManager surf;

    void Start()
    {
        durationImage = GetComponent<Image>();
    }
    private void OnEnable()
    {
        try
        {
            surf = SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>();
        }
        catch (System.NullReferenceException)
        {

        }
    }
        
    public void ResetFillAmount()
    {
        if(durationImage != null)
        {
            durationImage.fillAmount = 0;
        }
        
    }


    private void OnDisable()
    {
        surf = null;
        CheckforPoints = false;
        canPlay = false;
    }

    void Update()
    {
        if (durationImage != null && SpotifyPreviewAudioManager.instance.audioSource.isPlaying && canPlay)
        {
            durationImage.fillAmount = SpotifyPreviewAudioManager.instance.GetAudioSourceTime() / SpotifyPreviewAudioManager.instance.GetAudioClipLenght();
            if(surf != null)
            {
                if (CheckforPoints && durationImage.fillAmount > .9f && !surf.GetCurrentMwsiveData().challenge_songeded)
                {

                    surf.GetCurrentMwsiveData().challenge_songeded = true;
                    
                    surf.GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().LastPosition();
                    Debug.Log("SongEnded");

                }
            }
            
        }
    }
    

}
