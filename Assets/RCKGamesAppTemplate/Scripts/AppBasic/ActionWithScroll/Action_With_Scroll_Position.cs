using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Action_With_Scroll_Position : MonoBehaviour
{
    public ScrollRect scrollRect;
    public GameObject objectToInteractWith;
    [Range(0f, 1f)]
    public float porcentageToActivateAction1;
    //[Range(0f, 1f)]
    //public float porcentageToDeactivateAction;



    //Hay que llamar esta funcion desde el Editor en el Scroll View -> On Value Changeg (Vector2)
    public void Activate_Action_DisableEnable_Object()   // Object must start Enable in Editor
    {
        if(scrollRect.verticalNormalizedPosition >= porcentageToActivateAction1)
        {
            objectToInteractWith.SetActive(true);
        }
        else
        {
            objectToInteractWith.SetActive(false);
        }
    }

    //Hay que llamar esta funcion desde el Editor en el Scroll View -> On Value Changeg (Vector2)
    public void Activate_Action_EnableDisable_Object()  // Object must start Disable in Editor
    {
        if (scrollRect.verticalNormalizedPosition >= porcentageToActivateAction1)
        {
            objectToInteractWith.SetActive(false);
        }
        else
        {
            objectToInteractWith.SetActive(true);
        }
    }

    //Hay que llamar esta funcion desde el Editor en el Scroll View -> On Value Changeg (Vector2)
    public void Activate_Action_HeaderSpecific_Object()  // Object must start Disable in Editor
    {
        if (scrollRect.verticalNormalizedPosition >= porcentageToActivateAction1)
        {
            objectToInteractWith.GetComponent<Animator>().SetBool("Activate", false);
        }
        else
        {
            objectToInteractWith.GetComponent<Animator>().SetBool("Activate", true);
        }
    }





}
