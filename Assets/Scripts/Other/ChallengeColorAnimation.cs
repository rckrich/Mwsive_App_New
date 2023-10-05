using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class ChallengeColorAnimation : MonoBehaviour
{

    public GameObject waveMask;
    public Transform leftRestPosition, centerRestPosition;
    public GameObject colorbackground;

    
    
    Tweener[] topDoMove;
    Tweener[] colorDoMove;
    private float starttween;
    private Vector2 InitialValue;
    private bool isPaused = false;


    // Start is called before the first frame update
    void Start()
    {

        InitialValue = colorbackground.GetComponent<RectTransform>().offsetMax;
    }


    public void StartAnimation()
    {
        if(topDoMove == null)
        {
            colorbackground.GetComponent<RectTransform>().offsetMax = InitialValue;
            starttween = colorbackground.GetComponent<RectTransform>().offsetMax.y;
            FromCenterToLeft();

            ColorUp();
        }
        else
        {
            topDoMove[0].Restart();
            colorDoMove[0].Restart();
        }
        
        Debug.Log("START ANIMATION");
    }

    public void CompleteAnimation()
    {


        topDoMove[0].Complete();
        colorDoMove[0].Complete();
        Debug.Log("COMPLETE ANIMATION");
    }

    public void PauseAnimation()
    {
        if (!isPaused)
        {
            topDoMove[0].Pause();
            colorDoMove[0].Pause();
            
        }
        else
        {
            topDoMove[0].Play();
            colorDoMove[0].Play();
        }

        isPaused = !isPaused;
        Debug.Log("Pause ANIMATION");
    }

    public void ForcePauseAnimation()
    {
        
         topDoMove[0].Pause();
         colorDoMove[0].Pause();

        isPaused = true;


        Debug.Log("FORCE PAUSE ANIMATION");
    }


    public void KillAnimation()
    {
        topDoMove[0].Restart();
        colorDoMove[0].Restart();
        
        topDoMove[0].Kill();
        colorDoMove[0].Kill();
        Debug.Log("KILL ANIMATION");
    }

    public void ForceClear()
    {
        if(topDoMove != null)
        {
            topDoMove[0].Restart();
            topDoMove[0].Pause();
            colorDoMove[0].Restart();
            colorDoMove[0].Pause();
            colorbackground.GetComponent<RectTransform>().offsetMax = InitialValue;
        }

        Debug.Log("FORCE CLEAR ANIMATION");
    }

    public void FromCenterToLeft()
    {
        waveMask.GetComponent<RectTransform>().anchoredPosition = centerRestPosition.GetComponent<RectTransform>().anchoredPosition;
        topDoMove = new Tweener[1];
        topDoMove[0] = waveMask.transform.DOMoveX(leftRestPosition.transform.position.x, 3).SetLoops(-1).SetEase(Ease.Linear);
         topDoMove[0].SetAutoKill(false);


    }

    public void ColorUp()
    {

            float twenable = starttween;
            colorDoMove = new Tweener[1];
            colorDoMove[0] =  DOTween.To(() => twenable, x => twenable = x, 0f, 30f);
            colorDoMove[0].OnUpdate(() => {
                colorbackground.GetComponent<RectTransform>().offsetMax = new Vector2(colorbackground.GetComponent<RectTransform>().offsetMax.x, twenable);
                if(twenable > -2)
                {
                    
                    waveMask.transform.DOMoveY(leftRestPosition.transform.position.y, 2f).OnComplete(() => {
                        
                    }).SetEase(Ease.Linear);
                }
            });

            colorDoMove[0].SetEase(Ease.InSine);
            colorDoMove[0].OnComplete(() =>
            {
                
                topDoMove[0].Complete();
  
            });
            colorDoMove[0].SetAutoKill(false);





    }

}
