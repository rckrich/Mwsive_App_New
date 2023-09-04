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

    void Start()
    {
        durationImage = GetComponent<Image>();
    }

    void Update()
    {
        if (durationImage != null && SpotifyPreviewAudioManager.instance.audioSource.isPlaying && canPlay)
        {
            durationImage.fillAmount = SpotifyPreviewAudioManager.instance.GetAudioSourceTime() / SpotifyPreviewAudioManager.instance.GetAudioClipLenght();
            if(CheckforPoints && durationImage.fillAmount > .9f && SurfPlaylist != null && !SurfPlaylist.SuccesfulEnded){
                SurfPlaylist.SuccesfulEnded = true;
                SurfPlaylist.LastPosition();
                Debug.Log("SongEnded");
                
            }
        }
    }
    public void SetCallBack(ButtonSurfPlaylist _SurfPlaylist){
        SurfPlaylist = _SurfPlaylist;
    }

}
