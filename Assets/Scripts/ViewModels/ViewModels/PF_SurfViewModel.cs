using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_SurfViewModel : ViewModel
{

    public void OnClick_BackButton()
    {
        StopAllCoroutines();
        SpotifyPreviewAudioManager.instance.StopTrack();
        NewScreenManager.instance.BackToPreviousView();
        
    }

    private void OnDestroy() {
        SpotifyPreviewAudioManager.instance.StopTrack();
    }

}
