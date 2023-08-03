using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class Mouse_control_FontSize_OnScroll : MonoBehaviour
{
    //change font size
    [Header("Font Size")]
    public TextMeshProUGUI text;
    public int minFontSize;
    public int maxFontSize;

    //the statu if the mouse is down
    //private bool is_mouse_down = false;

    //the mouse position
    private float mouse_position_x;
    private float mouse_position_y;



    // Update is called once per frame
    void Update()
    {
        //set the mouse is down, Whether the mouse is on the object
        #region 
        if (Input.GetMouseButtonDown(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            //this.is_mouse_down = true;
            this.mouse_position_x = Input.mousePosition.x;
            this.mouse_position_y = Input.mousePosition.y;
        }
        #endregion

        //font size
        #region 
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (text.fontSize > minFontSize)
            {
                text.fontSize -= 1;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {

            if (text.fontSize < maxFontSize)
            {
                text.fontSize += 1;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") == 0)
        {
        }
        #endregion

    }
}
