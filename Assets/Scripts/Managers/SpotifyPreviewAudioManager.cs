using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public delegate void SpotifyAudioDownloaderCallback(object[] _list);

public class SpotifyPreviewAudioManager : MonoBehaviour
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

    public void GetTrack(string _audioURL, SpotifyAudioDownloaderCallback _callback = null)
    {
        StartCoroutine(CR_GetAudioClip(_audioURL, _callback));
    }

    public void StopTrack()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            
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
        }

        isPaused = !isPaused;
    }

    private IEnumerator CR_GetAudioClip(string _audioURL, SpotifyAudioDownloaderCallback _callback = null)
    {
        Debug.Log("Starting to download the audio... " + _audioURL);

        using (UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(
            _audioURL,
            AudioType.MPEG
        ))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                audioClip = DownloadHandlerAudioClip.GetContent(webRequest);
                audioSource.clip = audioClip;
                audioSource.time = 0f;
                audioSource.Play();

                if (_callback != null)
                    _callback(null);

                Debug.Log("Audio is playing");
            }
        }
    }
}
