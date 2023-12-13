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


    public void ForceReset()
    {
        surf = null;
        CheckforPoints = false;
        canPlay = false;
        if (durationImage != null)
        {
            durationImage.fillAmount = 0;
        }
    }

    void Update()
    {
        if (durationImage != null && SpotifyPreviewAudioManager.instance.audioSource.isPlaying && canPlay)
        {

            durationImage.fillAmount = SpotifyPreviewAudioManager.instance.GetAudioSourceTime() / SpotifyPreviewAudioManager.instance.GetAudioClipLenght();


        }

    }
    

}
