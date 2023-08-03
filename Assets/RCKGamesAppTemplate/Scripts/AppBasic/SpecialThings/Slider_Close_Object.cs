using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Slider_Close_Object : MonoBehaviour
{
    public GameObject objectToCLose;
    public Slider slider;


    private void OnEnable()
    {
        slider.value = 0;
    }

    public void Btn_Close_With_Slider()
    {
        if(slider.value >= 1)
        {
            Audio_Manager.Instance.PlaySFX(0);
            //objectToCLose.SetActive(false);
            StartCoroutine(Close_InfoLugar_Section());
        }
    }

    IEnumerator Close_InfoLugar_Section()
    {
        objectToCLose.GetComponent<Animator>().SetTrigger("Close");
        yield return new WaitForSeconds(0.55f);
        objectToCLose.SetActive(false);
    }

}
