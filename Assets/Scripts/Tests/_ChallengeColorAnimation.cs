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
    
    Tweener ColorAni, MaskAni, MaskFinalAni;
    float starttween;
    bool isPaused = false;
    float maskThickness;

    private void Update()
    {
    }

    void Start()
    {

        maskThickness = Mask.GetComponent<RectTransform>().rect.width;
        initialValue = Color.GetComponent<RectTransform>().offsetMax;
        

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
        MaskFinalAni.Complete();
        

    }

    public void StartAnimation()
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
