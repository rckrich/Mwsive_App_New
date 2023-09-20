using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class AniTest : MonoBehaviour
{
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
}
