using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowButtonOnClick : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator animator;
    public int count = 0;
    public GameObject opciones;
    public GameObject animationManager;
    public List<GameObject> opcionesList;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnimatorOnClick()
    {
        if (count == 1)
        {
            
            animator.Play("ArrowSettingsPlay2_Anim");
            //animationManager.GetComponent<UIAniManager>().FadeOut(opciones);
            count--;
            opciones.SetActive(false);
        }
        else
        {
            
            animator.Play("ArrowSettingsPlay_Anim");
            count++;
            foreach (var opcion in opcionesList)
            {
                animationManager.GetComponent<UIAniManager>().FadeIn(opcion);
            }
            // animationManager.GetComponent<UIAniManager>().FadeIn(opciones);
            opciones.SetActive(true);
        }
        
    }
}
