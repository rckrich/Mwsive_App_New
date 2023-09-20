using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class ChallengeColorAnimation : MonoBehaviour
{

    public GameObject waveMask, leftRestPosition;
    public GameObject colorbackground;
    private Vector3 left;
    private bool isComplete;
    Tween topDoMove;
    Tween colorDoMove;


    // Start is called before the first frame update
    void Start()
    {
        left = leftRestPosition.transform.position;
        Initialize();
        PauseAnimation(false);
    }

    private void OnEnable()
    {
        
        
    }

    public void Initialize()
    {
        FromCenterToLeft();

        ColorUp();
    }

    public void PauseAnimation(bool _value)
    {
        if(isComplete) {
            if (_value)
            {
                topDoMove.Play();
                colorDoMove.Play();
            }
            else
            {
                topDoMove.Pause();
                colorDoMove.Pause();
            }
        }
        
        
    }

    public void ForceRestart()
    {
        if (isComplete)
        {
            topDoMove.Restart();
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
        topDoMove = waveMask.transform.DOMoveX(left.x, 3).SetLoops(-1).SetEase(Ease.Flash);
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
            waveMask.transform.DOMoveY(left.y, 2f).OnComplete(() => {
                topDoMove.Kill();
                isComplete = true;
            }).SetEase(Ease.Flash);
        });
        sequence.SetEase(Ease.Flash);

    }

}
