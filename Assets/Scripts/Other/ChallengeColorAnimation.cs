using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class ChallengeColorAnimation : MonoBehaviour
{

    public GameObject waveMask ;
    public Transform leftRestPosition;
    public GameObject colorbackground;
    private bool isComplete;
    Tween topDoMove;
    Tween colorDoMove;

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
            FromCenterToLeft();

            ColorUp();
            amIEnabled = true;
            PlaySwitch = true;
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
        if (isComplete && amIEnabled)
        {
            colorDoMove.Restart();
            
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
        topDoMove = waveMask.transform.DOMoveX(leftRestPosition.transform.position.x, 3).SetLoops(-1).SetEase(Ease.Flash);
    }

    public void ColorUp()
    {

        float twenable = -380f;
        var sequence = DOTween.Sequence();
        colorDoMove = sequence;
        sequence.Append(DOTween.To(() => twenable, x => twenable = x, 0f, 28f));
        sequence.OnUpdate(() => {
            colorbackground.GetComponent<RectTransform>().offsetMax = new Vector2(colorbackground.GetComponent<RectTransform>().offsetMax.x, twenable);
        });
        sequence.OnComplete(() => {
            waveMask.transform.DOMoveY(leftRestPosition.transform.position.y, 2f).OnComplete(() => {
                topDoMove.Kill();
                isComplete = true;
            }).SetEase(Ease.Flash);
        });
        sequence.SetEase(Ease.Flash);

    }

}
