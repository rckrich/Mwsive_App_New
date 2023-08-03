using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AudioTrack
{
    public AudioTrack() { }
    public AudioTrack(string _name, string _url, string _albumImageURL, string _artistName)
    {
        name = _name;
        url = _url;
        albumImageURL = _albumImageURL;
        artistName = _artistName;
    }
    public string name;
    public string url;
    public string albumImageURL;
    public string artistName;
}

public class AudioDownloadManager : Manager
{
    private static AudioDownloadManager _instance;

    public static AudioDownloadManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<AudioDownloadManager>();
            }
            return _instance;
        }
    }

    private const int millisecondsMultiplier = 1000;

    [Header("GameObject Reference")]
    public Button PlayButton;
    public Button PauseButton;
    public Button NextButton;
    public Button PreviousButton;
    public Slider currentProgressSlider;
    public TextMeshProUGUI currentProgressText, totalProgressText;
    public Slider volumeSlider;
    public Button muteButton;
    [Header("Track Information UI")]
    public TextMeshProUGUI currentlyPlayingAudioName;
    public TextMeshProUGUI currentlyArtistText;
    public TextMeshProUGUI currentlyPlayingAudioNumber;

    AudioSource audioSource;
    AudioClip audioClip;

    List<AudioTrack> audioTracks = new List<AudioTrack>();
    bool isPaused = false;
    int index = 0;
    int totalTracks;
    AudioTrack currentTrack;
    private bool progressStartDrag = false;
    private float progressDragNewValue = -1.0f;
    private bool volumeStartDrag = false;
    private float volumeDragNewValue = -1.0f;
    private float volumeLastValue = -1;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        audioSource = GetComponent<AudioSource>();

        if (muteButton != null)
        {
            muteButton.onClick.AddListener(() => this.OnToggleMute());
        }

        // Update Volume slider
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0;
            volumeSlider.maxValue = 1;
            volumeSlider.value = audioSource.volume;

            // Enable interaction
            volumeSlider.interactable = true;

            // Listen to value change on slider
            volumeSlider.onValueChanged.AddListener(this.OnVolumeSliderValueChanged);
            // Add EventTrigger component, listen to mouse up/down events
            EventTrigger eventTrigger = volumeSlider.gameObject.AddComponent<EventTrigger>();
            // Mouse Down event
            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };
            entry.callback.AddListener(this.OnVolumeSliderMouseDown);
            eventTrigger.triggers.Add(entry);
            // Mouse Up event
            entry = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerUp
            };
            entry.callback.AddListener(this.OnVolumeSliderMouseUp);
            eventTrigger.triggers.Add(entry);
        }

        // Configure progress slider
        if (currentProgressSlider != null)
        {
            // Enable interaction
            currentProgressSlider.interactable = true;

            // Listen to value change on slider
            currentProgressSlider.onValueChanged.AddListener(this.OnProgressSliderValueChanged);
            // Add EventTrigger component, listen to mouse up/down events
            EventTrigger eventTrigger = currentProgressSlider.gameObject.AddComponent<EventTrigger>();
            // Mouse Down event
            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };
            entry.callback.AddListener(this.OnProgressSliderMouseDown);
            eventTrigger.triggers.Add(entry);
            // Mouse Up event
            entry = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerUp
            };
            entry.callback.AddListener(this.OnProgressSliderMouseUp);
            eventTrigger.triggers.Add(entry);
        }
    }

    private void Update()
    {
        // Update Track slider
        if (currentTrack != null && audioClip != null)
        {
            if (totalProgressText != null)
            {
                //totalProgressText.text = S4UUtility.MsToTimeString((int)Mathf.Round(audioClip.length * millisecondsMultiplier));
            }
            if (currentProgressSlider != null)
            {
                currentProgressSlider.minValue = 0;
                currentProgressSlider.maxValue = audioClip.length;

                // Update position when user is not dragging slider
                if (!progressStartDrag)
                {
                    currentProgressSlider.value = audioSource.time;
                }
            }
        }
    }

    private IEnumerator CR_GetAudioClip(AudioTrack _track, Button _button)
    {
        _button.interactable = false;

        Debug.Log("Starting to download the audio...");

        using (UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(
            _track.url,
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
                currentTrack = _track;
                currentlyPlayingAudioName.text = _track.name;
                currentlyArtistText.text = _track.artistName;
                ImageManager.instance.GetImage(_track.albumImageURL, null);
                Debug.Log("Audio is playing");
            }
        }

        _button.interactable = true;
    }

    public void AddTrackToList(string _name, string _url, string _albumImageURL, string _artistName)
    {
        AudioTrack track = new AudioTrack
        {
            name = _name,
            url = _url,
            albumImageURL = _albumImageURL,
            artistName = _artistName
        };

        audioTracks.Add(track);
    }

    public void StartPlayList()
    {
        if (audioTracks.Count <= 0)
        {
            Debug.Log("No hay url en la lista.");
        }
        else
        {
            index = 0;
            currentlyPlayingAudioNumber.text = index + 1 + " / " + totalTracks;
            StopAllCoroutines();
            StartCoroutine(CR_GetAudioClip(audioTracks[index], PlayButton));
        }
    }

    public void PauseAudio()
    {
        if (isPaused)
        {
            audioSource.UnPause();
        }
        else
        {
            audioSource.Pause();
        }

        isPaused = !isPaused;
        StartCoroutine(CR_WaitBeforeInteraction(PauseButton));
    }

    public void NextSong()
    {
        index++;

        if (index > (audioTracks.Count - 1))
            index = 0;

        currentlyPlayingAudioNumber.text = index + 1 + " / " + totalTracks;

        StopAllCoroutines();
        StartCoroutine(CR_GetAudioClip(audioTracks[index], NextButton));
        StartCoroutine(CR_WaitBeforeInteraction(NextButton));
    }

    public void PreviousSong()
    {
        index--;

        if (index < 0)
            index = (audioTracks.Count - 1);

        currentlyPlayingAudioNumber.text = index + 1 + " / " + totalTracks;

        StopAllCoroutines();
        StartCoroutine(CR_GetAudioClip(audioTracks[index], PreviousButton));
        StartCoroutine(CR_WaitBeforeInteraction(PreviousButton));
    }

    public void SetTotalTracksNumber(int _val)
    {
        totalTracks = _val;
        currentlyPlayingAudioNumber.text = "X / " + totalTracks;
    }

    private IEnumerator CR_WaitBeforeInteraction(Button _button)
    {
        _button.interactable = false;
        yield return new WaitForSeconds(1f);
        _button.interactable = true;
    }

    private void OnToggleMute()
    {
        float? volume = audioSource.volume;
        float targetVolume;
        TextMeshProUGUI muteText = muteButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (volume.HasValue && volume > 0)
        {
            // Set target volume to 0, sprite to muted
            targetVolume = 0;
            muteText.text = "Unmute";
            // Save current volume for unmute press
            volumeLastValue = volume.Value;
        }
        else
        {
            // Set target to last volume value before mute
            if (volumeLastValue > 0)
            {
                targetVolume = volumeLastValue;
                volumeLastValue = -1;
            }
            else
            {
                // If no value, use default value
                targetVolume = 1;
            }

            // Update sprite
            muteText.text = "Mute";
        }

        audioSource.volume = targetVolume;
    }

    private void OnProgressSliderMouseDown(BaseEventData arg0)
    {
        progressStartDrag = true;
    }

    private void OnProgressSliderValueChanged(float newValueMs)
    {
        progressDragNewValue = newValueMs;

        //currentProgressText.text = S4UUtility.MsToTimeString((int)Mathf.Round(progressDragNewValue * millisecondsMultiplier));
    }

    private void OnProgressSliderMouseUp(BaseEventData arg0)
    {
        if (progressStartDrag && progressDragNewValue > 0)
        {
            audioSource.time = progressDragNewValue;

            // Set value in slider
            currentProgressSlider.value = progressDragNewValue;

            // Reset variables
            progressStartDrag = false;
            progressDragNewValue = -1.0f;
        }
    }

    private void OnVolumeSliderMouseDown(BaseEventData arg0)
    {
        volumeStartDrag = true;
    }

    private void OnVolumeSliderValueChanged(float newValueMs)
    {
        volumeDragNewValue = newValueMs;
    }

    private void OnVolumeSliderMouseUp(BaseEventData arg0)
    {
        if (volumeStartDrag && volumeDragNewValue > 0)
        {
            audioSource.volume = volumeDragNewValue;

            // Set value in slider
            Debug.Log(volumeDragNewValue);
            volumeSlider.value = volumeDragNewValue;

            // Reset variables
            volumeStartDrag = false;
            volumeDragNewValue = -1.0f;
        }
    }
}
