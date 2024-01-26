using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowButtonOnClick : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject arrowObject;
    public int count = 0;
    public GameObject opciones;
    
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

            //animator.Play("ArrowSettingsPlay2_Anim");
            arrowObject.transform.DORotate(new Vector3(0, 0, 0), .5F);
            //animationManager.GetComponent<UIAniManager>().FadeOut(opciones);
            count--;
            opciones.SetActive(false);
        }
        else
        {
            arrowObject.transform.DORotate(new Vector3(0, 0, -90), .5F);
            //animator.Play("ArrowSettingsPlay_Anim");
            count++;
            foreach (var opcion in opcionesList)
            {
                UIAniManager.instance.FadeIn(opcion);
            }
            // animationManager.GetComponent<UIAniManager>().FadeIn(opciones);
            opciones.SetActive(true);
        }
        
    }

    private void OnDisable()
    {
        if(count == 1)
        {
            arrowObject.transform.eulerAngles = Vector3.zero;
            opciones.SetActive(false);
            count--;
        }
    }


}
