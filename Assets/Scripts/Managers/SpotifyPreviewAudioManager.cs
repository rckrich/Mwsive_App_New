using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public delegate void SpotifyAudioDownloaderCallback(object[] _list);

public class SpotifyPreviewAudioManager : Manager
{
    private static SpotifyPreviewAudioManager _instance;
    public TrackHolder trackHolder;

    public static SpotifyPreviewAudioManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SpotifyPreviewAudioManager>();
            }
            return _instance;
        }
    }

    public AudioSource audioSource;

    private AudioClip audioClip;
    private bool isPaused = false;

    public IEnumerator GetTrack(string _audioURL, SpotifyAudioDownloaderCallback _callback = null)
    {
        StopCoroutine("CR_GetAudioClip");
        IEnumerator coroutine = CR_GetAudioClip(_audioURL, _callback);
        StartCoroutine(coroutine);
        return coroutine;

    }

    public void StopTrack()
    {

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            InvokeEvent<TimerAppEvent>(new TimerAppEvent() { type = "STOP" });
            isPaused = true;

        }
    }

    public void Pause()
    {

        if (isPaused)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
            InvokeEvent<TimerAppEvent>(new TimerAppEvent() { type = "PAUSE" });
        }

        isPaused = !isPaused;
    }

    public void ForcePlay()
    {
        if (!audioSource.isPlaying && !isPaused)
        {
            audioSource.Play();
        }
    }

    public void ForcePause()
    {
        audioSource.Pause();
        isPaused = true;
        InvokeEvent<TimerAppEvent>(new TimerAppEvent() { type = "PAUSE" });
    }

    public float GetAudioSourceTime() { return audioSource.time; }

    public float GetAudioClipLenght() { return audioClip.length; }


    private IEnumerator CR_GetAudioClip(string _audioURL, SpotifyAudioDownloaderCallback _callback = null)
    {
        DebugLogManager.instance.DebugLog("Starting to download the audio... " + _audioURL);

        using (UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(
            _audioURL,
            AudioType.MPEG
        ))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                DebugLogManager.instance.DebugLog(webRequest.error);
            }
            else
            {
                audioClip = DownloadHandlerAudioClip.GetContent(webRequest);
                audioSource.clip = audioClip;
                audioSource.time = 0f;
                audioSource.Play();
                isPaused = false;

                if (_callback != null)
                    _callback(new object[] { audioClip.length });

                DebugLogManager.instance.DebugLog("Audio is playing");
                InvokeEvent<TimerAppEvent>(new TimerAppEvent() { type = "KILL" });
                InvokeEvent<TimerAppEvent>(new TimerAppEvent() { type = "START" });

            }
        }
    }

    public void StopCustomCoroutine(IEnumerator _coroutine)
    {
        StopCoroutine(_coroutine);
    }
}
