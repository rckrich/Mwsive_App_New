using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

    public void GetTrack(string _audioURL)
    {
        StartCoroutine(CR_GetAudioClip(_audioURL));
    }

    public void StopTrack()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            
        }
    }

    private IEnumerator CR_GetAudioClip(string _audioURL)
    {
        Debug.Log("Starting to download the audio...");

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
                
                Debug.Log("Audio is playing");
            }
        }
    }
}
