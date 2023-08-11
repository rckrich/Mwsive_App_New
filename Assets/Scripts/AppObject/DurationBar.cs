using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DurationBar : AppObject
{
    private Image durationImage;

    void Start()
    {
        durationImage = GetComponent<Image>();
    }

    void Update()
    {
        if (durationImage != null && SpotifyPreviewAudioManager.instance.audioSource.isPlaying)
        {
            durationImage.fillAmount = SpotifyPreviewAudioManager.instance.GetAudioSourceTime() / SpotifyPreviewAudioManager.instance.GetAudioClipLenght();
        }
    }
}
