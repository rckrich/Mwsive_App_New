using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System;

public class VideoPlayer_Controller : MonoBehaviour
{
    [Header("Video Player")]
    public VideoPlayer videoPlayer;

    [Header("Botones Play - Pause")]
    public GameObject btn_Play_Pause;
    public Sprite play_Sprite;
    public Sprite pause_Sprite;

    [Header("Tiempo del Video")]
    public Text videoTimeText;
    public Slider videoTimeSlider;

    [Header("UI Container")]
    public GameObject VideoUI;

    [Header("Loading")]
    public GameObject loadingScreen;

    [Header("PopUP No Internet")]
    public GameObject popUp_NoInternet;
    //public GameObject loading_Check_For_Internet_Screen;
    string m_ReachabilityText;


    private void OnEnable()
    {
        Init_Btns();
        videoPlayer.targetTexture.Release();
    }

    public void Start()
    {
        // Liberamos la imagen del cache que la textura pueda tener de reproducir algun otro video
        videoPlayer.targetTexture.Release();
    }

    private void Update()
    {
        if (videoPlayer.clip != null || videoPlayer.url != null)
        {
            UpdateTimeText();
            UpdateTimeSlider();
        }

        // Cuando se termine de reproducir el video activamos el UI del Video Player
        if(videoTimeSlider.value >= 0.99f && !VideoUI.activeInHierarchy)
        {
            btn_Play_Pause.transform.GetChild(0).GetComponent<Image>().sprite = play_Sprite;
            Btn_OpenClose_VideoUI();
        }
    }


    void Init_Btns()
    {
        if (videoPlayer.isPlaying || videoPlayer.playOnAwake)
        {
            VideoUI.SetActive(false);
        }
        else
        {
            VideoUI.SetActive(true);
        }
    }


    void UpdateTimeSlider()
    {
        // FORMULA: (videoCurrentTime * sliderMaxValue) / videoLength

        if (videoPlayer.source == VideoSource.VideoClip)
        {
            float videoCurrentTime = (float)videoPlayer.time;
            float videoLength = (float)videoPlayer.clip.length;
            videoTimeSlider.value = (videoCurrentTime * 1) / videoLength;
        }
        else
        {
            float videoCurrentTime = (float)videoPlayer.frame;
            float videoLength = (float)videoPlayer.frameCount;
            videoTimeSlider.value = (videoCurrentTime * 1) / videoLength;
        }
    }


    void UpdateTimeText()
    {
        if (videoPlayer.source == VideoSource.VideoClip)
        {
            var ts1 = TimeSpan.FromSeconds(videoPlayer.time);
            var ts2 = TimeSpan.FromSeconds(videoPlayer.clip.length);

            videoTimeText.text = "<b>" + string.Format("{0:0}:{1:00}", ts1.Minutes, ts1.Seconds) + "</b>" + " / " + string.Format("{0:0}:{1:00}", ts2.Minutes, ts2.Seconds);
        }
        else
        {
            var ts1 = TimeSpan.FromSeconds(videoPlayer.time);
            var ts2 = TimeSpan.FromSeconds(videoPlayer.length);

            videoTimeText.text = "<b>" + string.Format("{0:0}:{1:00}", ts1.Minutes, ts1.Seconds) + "</b>" + " / " + string.Format("{0:0}:{1:00}", ts2.Minutes, ts2.Seconds);
        }
    }



    void Play_Video()
    {
        //Audio_Manager.Instance.PlaySFX(0);

        if (videoPlayer.isPrepared)
        {
            loadingScreen.SetActive(false);
            videoPlayer.Play();
        }
        else if (!videoPlayer.isPrepared)
        {
            StartCoroutine(PrepareVideo_Routine());
        }
    }

    void Pause_Video()
    {
        //Audio_Manager.Instance.PlaySFX(0);

        UpdateTimeText();
        UpdateTimeSlider();

        videoPlayer.Pause();
    }

    IEnumerator PrepareVideo_Routine()
    {
        videoPlayer.Prepare();

        // Aqui puedes activar una pantalla de loading
        if(loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        yield return new WaitUntil(() => videoPlayer.isPrepared);

        // Aqui puedes desactivar la pantalaa de loading
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }

        videoPlayer.Play();
    }


    public void Btn_Play_Pause_Video()
    {
        Audio_Manager.Instance.PlaySFX(0);

        if (videoPlayer.isPlaying)
        {
            Pause_Video();
            btn_Play_Pause.transform.GetChild(0).GetComponent<Image>().sprite = play_Sprite;
        }
        else
        {
            //Play_Video();
            Play_Video_If_There_Is_Internet();
            btn_Play_Pause.transform.GetChild(0).GetComponent<Image>().sprite = pause_Sprite;
            StartCoroutine(Hide_VideoUI_OnPlay());
        }
    }

    public void Btn_Exit()
    {
        Audio_Manager.Instance.PlaySFX(0);
        gameObject.SetActive(false);
    }

    public void Btn_OpenClose_VideoUI()
    {
        if (VideoUI.activeInHierarchy)
        {
            VideoUI.SetActive(false);
        }
        else
        {
            VideoUI.SetActive(true);
        }
    }

    IEnumerator Hide_VideoUI_OnPlay()
    {
        yield return new WaitForSeconds(2f);

        VideoUI.SetActive(false);
    }


    IEnumerator CheckInternet()
    {
        //loading_Check_For_Internet_Screen.SetActive(true);

        loadingScreen.SetActive(true);

        //Check if the device cannot reach the internet
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            yield return new WaitForSeconds(3f);
            popUp_NoInternet.SetActive(true);
            loadingScreen.SetActive(false);

            m_ReachabilityText = "Not Reachable.";
        }
        //Check if the device can reach the internet via a carrier data network
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            yield return new WaitForSeconds(1f);
            //loadingScreen.SetActive(false);
            Play_Video();

            m_ReachabilityText = "Reachable via carrier data network.";
        }
        //Check if the device can reach the internet via a LAN
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            yield return new WaitForSeconds(1f);
            //loadingScreen.SetActive(false);
            Play_Video();

            m_ReachabilityText = "Reachable via Local Area Network.";
        }

        //Output the network reachability to the console window
        Debug.Log("Internet : " + m_ReachabilityText);

    }

    void Play_Video_If_There_Is_Internet()
    {
        StartCoroutine(CheckInternet());
    }

    public void Btn_Close_PopUp_Video_Internet()
    {
        //Audio_Manager.Instance.PlaySFX(0);
        popUp_NoInternet.SetActive(false);
    }


}


