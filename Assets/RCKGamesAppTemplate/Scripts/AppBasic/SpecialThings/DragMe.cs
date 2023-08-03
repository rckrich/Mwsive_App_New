using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DragMe : MonoBehaviour
{
    public void Btn_BeginDrag()
    {
        //transform.localScale = new Vector3(1.05f, 1.05f, 1f);
        transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
    }

    public void Btn_Drag()
    {
        transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public void Btn_EndDrag()
    {
        //transform.localScale = new Vector3(1f, 1f, 1f);
        GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

    }

}
