using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DescubrirButton : MonoBehaviour
{
    public GameObject HeaderBackground;
    public GameObject ResultadosBusqueda;
    public GameObject HeaderRestPosition;
    public GameObject HeaderPosPosition;
    public GameObject BuscadorBackground;
    public GameObject CancelarButton;
    public GameObject BuscarRestPosition;
    public GameObject BuscarPosPosition;

    public void OnClick_BuscarButton(GameObject ScrollView){
        float twenable = 0f;
        UIAniManager.instance.VerticalTransitionToCustomPosition(HeaderBackground, HeaderPosPosition,ScrollView, true );
        var sequence = DG.Tweening.DOTween.Sequence();
        sequence.Append(DOTween.To(()=> twenable, x => twenable = x, -70f, 0.5f ));
        sequence.OnUpdate(() => {BuscadorBackground.GetComponent<RectTransform>().offsetMax = new Vector2(twenable, BuscadorBackground.GetComponent<RectTransform>().offsetMax.y);});
        sequence.OnComplete(() => {CancelarButton.SetActive(true);UIAniManager.instance.FadeIn(ScrollView, .3f);});
        UIAniManager.instance.FadeIn(ResultadosBusqueda, 0.5f);
    }

    public void OnClick_CancelarButton(GameObject ScrollView){
        float twenable = -70f;
        CancelarButton.SetActive(false);
            UIAniManager.instance.FadeOut(ScrollView, 0.5f);
            UIAniManager.instance.VerticalTransitionToCustomPosition(HeaderBackground, HeaderRestPosition, ScrollView, false);
            var sequence = DG.Tweening.DOTween.Sequence();
            sequence.Append(DOTween.To(()=> twenable, x => twenable = x, 0f, 0.5f ));
            sequence.OnUpdate(() => {BuscadorBackground.GetComponent<RectTransform>().offsetMax = new Vector2(twenable, BuscadorBackground.GetComponent<RectTransform>().offsetMax.y);});
            sequence.OnComplete(() => {ScrollView.SetActive(false);});
            UIAniManager.instance.FadeOut(ResultadosBusqueda, 0.5f);
    }
}
