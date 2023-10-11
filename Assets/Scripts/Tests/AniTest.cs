using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class AniTest : MonoBehaviour
{
    public Transform restPosition;
    private Vector2 initialValue;
    public GameObject Color, Mask;
    public RectTransform SecondWaveMask, restPosition2, FinalPosition;

    public bool Follow;
    Tweener ColorAni, MaskAni, MaskFinalAni;
    float starttween;

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



        StartAnimation();
    }


    public void StartAnimation()
    {


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

    private void FromCenterToLeft()
    {
        MaskAni = Mask.transform.DOMoveX(restPosition2.position.x, 3).SetLoops(-1).SetEase(Ease.Linear);
    }

    private void ColorUp()
    {

        MaskFinalAni = Mask.transform.DOMove(FinalPosition.position, 2).OnComplete(() => { MaskAni.Pause(); });
        MaskFinalAni.Pause();

        float twenable = starttween;

        MaskAni = DOTween.To(() => twenable, x => twenable = x, 0, 30f);
        MaskAni.OnUpdate(() => {
            Color.GetComponent<RectTransform>().offsetMax = new Vector2(Color.GetComponent<RectTransform>().offsetMax.x, twenable);
            Debug.Log(twenable);
            if (twenable > -15f)
            {

                MaskFinalAni.Play();

            }


        });

        MaskAni.SetEase(Ease.InSine);

        MaskAni.SetAutoKill(false);


    }

}
