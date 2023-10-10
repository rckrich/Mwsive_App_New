using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

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
    public DescubrirPaginas Descubrir;
    public Descubrir_ViewModel DescubrirVW;
    public Button button;
    public GameObject PlaceHolder;
    public TMP_InputField TextArea;
    public GameObject ScrollView;


    public void Initialize()
    {
        if (!button.enabled)
        {
#if PLATFORM_ANDROID
            SetAndroidBackAction();
#endif
        }
    }

    public void OnClick_BuscarButton(){
#if PLATFORM_ANDROID
        SetAndroidBackAction();
#endif
        float twenable = 0f;
        UIAniManager.instance.VerticalTransitionToCustomPosition(HeaderBackground, HeaderPosPosition,ScrollView, true );
        var sequence = DG.Tweening.DOTween.Sequence();
        sequence.Append(DOTween.To(()=> twenable, x => twenable = x, -70f, 0.5f ));
        sequence.OnUpdate(() => {BuscadorBackground.GetComponent<RectTransform>().offsetMax = new Vector2(twenable, BuscadorBackground.GetComponent<RectTransform>().offsetMax.y);});
        sequence.OnComplete(() => {
            DescubrirVW.KillAllPrefabLists();
            CancelarButton.SetActive(true);
            UIAniManager.instance.FadeIn(ScrollView, .3f);});
        UIAniManager.instance.FadeIn(ResultadosBusqueda, 0.5f);
    }

    public void OnClick_CancelarButton(){
        Descubrir.HideAllText();
        TextArea.enabled = false;
        float twenable = -70f;
        CancelarButton.SetActive(false);
            UIAniManager.instance.FadeOut(ScrollView, 0.5f);
            UIAniManager.instance.VerticalTransitionToCustomPosition(HeaderBackground, HeaderRestPosition, ScrollView, false);
            var sequence = DG.Tweening.DOTween.Sequence();
            sequence.Append(DOTween.To(()=> twenable, x => twenable = x, 0f, 0.5f ));
            sequence.OnUpdate(() => {BuscadorBackground.GetComponent<RectTransform>().offsetMax = new Vector2(twenable, BuscadorBackground.GetComponent<RectTransform>().offsetMax.y);});
            sequence.OnComplete(() => {ScrollView.SetActive(false);

                DescubrirVW.KillAllPrefabLists();
                TextArea.enabled = true;
            });
            UIAniManager.instance.FadeOut(ResultadosBusqueda, 0.5f);
        
        CancelarActions();
    }

    public void CancelarActions(){
        Descubrir.HideEscena();
        DescubrirVW.KillAllPrefabLists();
        TextArea.enabled = false;
        PlaceHolder.SetActive(true);
        button.enabled = true;
    }

    public void SetAndroidBackAction()
    {
#if PLATFORM_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            AppManager.instance.SetAndroidBackAction(() => {
                OnClick_CancelarButton();
            });
        }
#endif
    }
}