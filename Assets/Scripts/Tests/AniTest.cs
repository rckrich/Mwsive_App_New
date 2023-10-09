using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class AniTest : MonoBehaviour
{
    public Transform restPosition, left;
    private Vector2 initialValue;

    public RectTransform SecondWaveMask, restPosition2;

    public bool Follow;
    Tweener ani;
    float starttween;
    

    private void Update()
    {
        if (Follow)
        {
            Vector3 Pos = restPosition.transform.position;
            Debug.Log(gameObject.GetComponent<RectTransform>().rect.height);
            //Pos.y = Pos.y - gameObject.GetComponent<RectTransform>().rect.height;

            //gameObject.transform.position = Pos;
        }
        
    }

    void Start()
    {
        if (!Follow)
        {
            initialValue = gameObject.GetComponent<RectTransform>().offsetMax;
        }
        else
        {
            Debug.Log(gameObject.GetComponent<RectTransform>().rect.width);

            float val = gameObject.GetComponent<RectTransform>().rect.width;


            SecondWaveMask.offsetMin = new Vector2(val, SecondWaveMask.offsetMin.y);
            SecondWaveMask.offsetMax = new Vector2(val, SecondWaveMask.offsetMax.y);
            restPosition2.offsetMin = new Vector2(-val, SecondWaveMask.offsetMin.y);
            restPosition2.offsetMax = new Vector2(-val, SecondWaveMask.offsetMax.y);

        }
                
        StartAnimation();
    }


    public void StartAnimation()
    {
        if (Follow)
        {
            if(ani == null)
            {
                FromCenterToLeft();
            }
            else
            {
                ani.Restart();
            }
        }
        else
        {
            if (ani == null)
            {
                gameObject.GetComponent<RectTransform>().offsetMax = initialValue;
                starttween = gameObject.GetComponent<RectTransform>().offsetMax.y;
                ColorUp();

            }
            else
            {
                ani.Restart();
            }
        }

    }

    private void FromCenterToLeft()
    {
        ani = gameObject.transform.DOMoveX(left.position.x, 3).SetLoops(-1).SetEase(Ease.Linear);
    }

    private void ColorUp()
    {

        float twenable = starttween;
        
        ani = DOTween.To(() => twenable, x => twenable = x, 0f, 30f);
        ani.OnUpdate(() => {
            gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(gameObject.GetComponent<RectTransform>().offsetMax.x, twenable);
            if (twenable > -2)
            {

                //waveMask.transform.DOMoveY(leftRestPosition.transform.position.y, 2f).OnComplete(() => {

                //}).SetEase(Ease.Linear);
            }
        });

        ani.SetEase(Ease.InSine);
        ani.OnComplete(() =>
        {

            ani.Complete();

        });
        ani.SetAutoKill(false);


    }


    /*
    public GameObject Top1, Position3;
    public GameObject Color;
    private Vector3 Left;
    Tween TopDoMove;
    Tween ColorDoMove;
    
    // Start is called before the first frame update
    void Start()
    {
        Left = Position3.transform.position; 

        FromCenterToLeft(Top1);
        
        ColorUp();
        

    }

    public void FromCenterToLeft(GameObject Target)
    {
        TopDoMove = Target.transform.DOMoveX(Left.x, 3).SetLoops(-1).SetEase(Ease.Flash);
    }

    public void ColorUp()
    {
            
            float twenable = -380f;
            var sequence = DOTween.Sequence();
            ColorDoMove = sequence;
            sequence.Append(DOTween.To(() => twenable, x => twenable = x, 0f, 28f));
            sequence.OnUpdate(() => { Color.GetComponent<RectTransform>().offsetMax = new Vector2(Color.GetComponent<RectTransform>().offsetMax.x, twenable);
            });
            sequence.OnComplete(() => {
                Top1.transform.DOMoveY(Left.y, 2f).OnComplete(() => {
                    TopDoMove.Kill();
                }).SetEase(Ease.Flash);
            });
            sequence.SetEase(Ease.Flash);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
