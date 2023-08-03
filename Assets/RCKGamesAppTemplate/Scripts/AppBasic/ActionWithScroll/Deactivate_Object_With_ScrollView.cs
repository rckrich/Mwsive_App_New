using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deactivate_Object_With_ScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;
    public GameObject objectToDeactivate;
    [Range(-1f, 1f)]
    public float porcentageToActivateAction1;
    //[Range(0f, 1f)]
    //public float porcentageToDeactivateAction;

    //Hay que llamar esta funcion desde el Editor en el Scroll View -> On Value Changeg (Vector2)
    public void Activate_Action()
    {
        /*
        if (scrollRect.verticalNormalizedPosition >= porcentageToActivateAction1)
        {
            objectToDeactivate.SetActive(false);
        }
        */

        if (scrollRect.horizontalNormalizedPosition <= porcentageToActivateAction1)
        {
            StartCoroutine(Close_InfoLugar_Section());
        }
    }

    IEnumerator Close_InfoLugar_Section()
    {
        objectToDeactivate.GetComponent<Animator>().SetTrigger("Close");
        yield return new WaitForSeconds(0.55f);
        objectToDeactivate.SetActive(false);
        scrollRect.horizontalNormalizedPosition = 0f;

    }


}
