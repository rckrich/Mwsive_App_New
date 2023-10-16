using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class _ChallengeColorAnimation : MonoBehaviour
{

   
    public Transform restPosition;
    private Vector2 initialValue;
    public GameObject Color, Mask;
    public RectTransform SecondWaveMask, ThirdWaveMask, restPosition2, FinalPosition, ColorSide;

    

    public bool isCompleted;
    
    Tweener ColorAni, MaskAni, MaskFinalAni;
    float starttween;
    bool isPaused = false;
    bool MaskAniOnce = false;
    float maskThickness;

    private void Update()

    {
        if (!isCompleted)
        {
            
            
            if(Color.transform.eulerAngles.z  > .1 && !MaskAniOnce)
            {
                
                MaskAni.Pause();
                MaskAniOnce = true;
            }
            
            if (Color.transform.eulerAngles.z  <.1 && MaskAniOnce)
            {
                
                MaskAni.Restart();
                MaskAniOnce = false;
            }
        }


        Color.transform.eulerAngles = -gameObject.transform.eulerAngles;


    }

    void Start()
    {
        

        initialValue = Color.GetComponent<RectTransform>().offsetMax;
        

        CalculateBoundries();
        
    }

    private void CalculateBoundries()
    {
        Mask.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        Mask.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);

        maskThickness = Mask.GetComponent<RectTransform>().rect.width;
        SecondWaveMask.offsetMin = new Vector2(maskThickness, SecondWaveMask.offsetMin.y);
        SecondWaveMask.offsetMax = new Vector2(maskThickness, SecondWaveMask.offsetMax.y);
        ThirdWaveMask.offsetMin = new Vector2(maskThickness, SecondWaveMask.offsetMin.y);
        ThirdWaveMask.offsetMax = new Vector2(maskThickness, SecondWaveMask.offsetMax.y);
        restPosition2.offsetMin = new Vector2(-maskThickness, SecondWaveMask.offsetMin.y);
        restPosition2.offsetMax = new Vector2(-maskThickness, SecondWaveMask.offsetMax.y);

        float colorThickness = Color.GetComponent<RectTransform>().rect.width;

        ColorSide.offsetMin = new Vector2(colorThickness, SecondWaveMask.offsetMin.y);
        ColorSide.offsetMax = new Vector2(colorThickness, SecondWaveMask.offsetMax.y);

        

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
        
        isCompleted = false;
        
        Mask.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        Mask.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
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
        Mask.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        Mask.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);

    }

    public void FirstReset()
    {
        
        MaskAni.Kill();
        Mask.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        Mask.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
    }

    public void ResetMask()
    {
        Mask.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        Mask.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);


        SecondWaveMask.offsetMax = new Vector2(SecondWaveMask.offsetMax.x, 0);
        SecondWaveMask.offsetMin = new Vector2(SecondWaveMask.offsetMin.x, 0);
        ThirdWaveMask.offsetMax = new Vector2(ThirdWaveMask.offsetMax.x, 0);
        ThirdWaveMask.offsetMin = new Vector2(ThirdWaveMask.offsetMin.x, 0);

        CalculateBoundries();
        FromCenterToLeft();
        

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
        CalculateBoundries();
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
        MaskAni = Mask.transform.DOMoveX(restPosition2.position.x, 3).SetLoops(-1).SetEase(Ease.Linear).SetAutoKill(false);
        
        
    }

    private void MaskFinal()
    {
        isCompleted = true;
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
