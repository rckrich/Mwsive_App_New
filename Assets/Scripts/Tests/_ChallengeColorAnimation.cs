using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class _ChallengeColorAnimation : MonoBehaviour
{

   
    public Transform restPosition;
    private Vector2 initialValue;
    public GameObject Color, Mask, colorsongended;
    public RectTransform SecondWaveMask, restPosition2, FinalPosition;

    

    public bool isCompleted;
    float MaskInitialValue;
    Tweener ColorAni, MaskAni, MaskFinalAni;
    float starttween;
    bool isPaused = false;
    float maskThickness;

    private void Update()
    {
    }

    void Start()
    {

        
        initialValue = Color.GetComponent<RectTransform>().offsetMax;
        CalculateBoundries();
        MaskInitialValue = Mask.GetComponent<RectTransform>().rect.y;
        
    }

    private void CalculateBoundries()
    {
        maskThickness = Mask.GetComponent<RectTransform>().rect.width;
        SecondWaveMask.offsetMin = new Vector2(maskThickness, SecondWaveMask.offsetMin.y);
        SecondWaveMask.offsetMax = new Vector2(maskThickness, SecondWaveMask.offsetMax.y);
        restPosition2.offsetMin = new Vector2(-maskThickness, SecondWaveMask.offsetMin.y);
        restPosition2.offsetMax = new Vector2(-maskThickness, SecondWaveMask.offsetMax.y);
    }

    public void PauseColor()
    {
        if (isPaused)
        {
            ColorAni.Play();
        }
        else
        {
            ColorAni.Pause();
        }
        isPaused = !isPaused;

        if(SpotifyPreviewAudioManager.instance.IsPlaying() == isPaused)
        {
            if (SpotifyPreviewAudioManager.instance.IsPlaying())
            {
                ColorAni.Play();
                isPaused = false;
            }
            else
            {
                ColorAni.Pause();
                isPaused = true;
            }
            
        }
        CalculateBoundries();

    }

    public void ForcePause()
    {
        isPaused = true;
        ColorAni.Pause();
    }

    public void ForceClear()
    {
        
        ColorAni.Restart();
        MaskFinalAni.Restart();
        MaskFinalAni.Pause();
        ColorAni.Pause();
        MaskAni.Restart();
        MaskAni.Pause();
        //colorsongended.SetActive(false);
        isCompleted = false;
    }

    public void CompleteAnimation()
    {
        isCompleted = true;
        
        StartAnimation();
        ColorAni.Complete();

        if(MaskFinalAni != null)
        {
            MaskFinalAni.Complete();
        }
        else
        {
            Mask.transform.position = FinalPosition.position;
            MaskAni.Pause();
        }

    }

    public void PauseMask()
    {
        MaskAni.Pause();
    }

    public void PlayMask()
    {
        MaskAni.Play();
    }

    public void StartAnimation()
    {
        if (SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().GetCurrentPrefab() == gameObject)
        {
            if (!isCompleted)
            {
                Mask.transform.position = restPosition.position;
            }
            if (MaskAni == null)
            {
                FromCenterToLeft();
            }
            else
            {
                MaskAni.Restart();
            }



            if (ColorAni == null)
            {
                Color.GetComponent<RectTransform>().offsetMax = initialValue;
                starttween = Color.GetComponent<RectTransform>().offsetMax.y;
                ColorUp();

            }
            else
            {

                ColorAni.Restart();
                MaskFinalAni.Restart();
                MaskFinalAni.Pause();
                MaskAni.Restart();
            }
        }
        else
        {
            Debug.LogWarning("Can not start animation is not in challenge");
            ForceClear();
        }
        
        


    }

    private void FromCenterToLeft()
    {
        MaskAni = Mask.transform.DOMoveX(restPosition2.position.x, 3).SetLoops(-1).SetEase(Ease.Linear);
    }

    private void MaskFinal()
    {
        
        MaskFinalAni = Mask.transform.DOMove(FinalPosition.position, 2).OnComplete(() => { MaskAni.Pause(); });
       
        
        
        
    }

    private void ColorUp()
    {
        float twenable = starttween;

        ColorAni = DOTween.To(() => twenable, x => twenable = x, 0, 30f);
        ColorAni.OnUpdate(() => {
            Color.GetComponent<RectTransform>().offsetMax = new Vector2(Color.GetComponent<RectTransform>().offsetMax.x, twenable);
            
            if (twenable > -15f)
            {
                if (!isCompleted)
                {
                    MaskFinal();
                }
                
                
            }


        });

        ColorAni.SetEase(Ease.Linear);

        ColorAni.SetAutoKill(false);


    }
   
}
