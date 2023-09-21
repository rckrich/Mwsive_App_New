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
    private bool isComplete;
    
    Tween topDoMove;
    Tween colorDoMove;
    private float starttween;
    private bool amIEnabled = false;
    private bool PlaySwitch = false;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }



    public void Initialize()
    {
        if (!isComplete)
        {
            starttween = colorbackground.GetComponent<RectTransform>().offsetMax.y;
            
            FromCenterToLeft();

            ColorUp();
            amIEnabled = true;
            PlaySwitch = true;

            Debug.Log(waveMask.transform.position);

        }

    }

    public void ResumeAnimation()
    {
        if(topDoMove != null && colorDoMove != null)
        {
            colorDoMove.Play();
            
            topDoMove.Play();
        }
        
    }

    public void PauseAnimation()
    {
        if(!isComplete  && amIEnabled) {
            if (!PlaySwitch)
            {
                colorDoMove.Play();
                
            }
            else
            {
                colorDoMove.Pause();
                
            }
            PlaySwitch = !PlaySwitch;
        }
        
        
    }

    public void PauseTopMove()
    {
        topDoMove.Pause();
        colorDoMove.Pause();
    }

    public void ForcePauseAnimation()
    {
        if (!isComplete && amIEnabled)
        {
            colorDoMove.Pause();
            PlaySwitch = false;
        }
    }

    public void ForceRestart()
    {
        if (!isComplete && amIEnabled)
        {
            
            colorDoMove.Restart();
            Debug.Log(waveMask.transform.position);
            topDoMove.Restart();
        }
        
    }


    public void CompleteAnimation()
    {
        isComplete = true; 
        topDoMove.Complete(); 
        colorDoMove.Complete();
    }

    public void FromCenterToLeft()
    {
        waveMask.GetComponent<RectTransform>().anchoredPosition = centerRestPosition.GetComponent<RectTransform>().anchoredPosition;

        if (topDoMove == null)
        {
            
            topDoMove = waveMask.transform.DOMoveX(leftRestPosition.transform.position.x, 3).SetLoops(-1).SetEase(Ease.Linear).SetId(0);
           
        }
        
    }

    public void ColorUp()
    {
        
        if (colorDoMove == null)
        {

            float twenable = starttween;
            var sequence = DOTween.Sequence();
            colorDoMove = sequence;
            sequence.OnPlay(() =>
            {
                
                
            });
            sequence.Append(DOTween.To(() => twenable, x => twenable = x, 0f, 30f));
            sequence.OnUpdate(() => {
                colorbackground.GetComponent<RectTransform>().offsetMax = new Vector2(colorbackground.GetComponent<RectTransform>().offsetMax.x, twenable);
                if(twenable > -2)
                {
                    
                    waveMask.transform.DOMoveY(leftRestPosition.transform.position.y, 2f).OnComplete(() => {
                        
                    }).SetEase(Ease.Linear);
                }
            });
            
            sequence.SetEase(Ease.InSine);
            sequence.OnComplete(() =>
            {
                Debug.Log("aaa");
                topDoMove.Kill();
                isComplete = true;
            });
            sequence.SetId(1);
            sequence.OnKill(() =>
            {
                Debug.Log("aaa");
                
            });
        }
       

    }

}
