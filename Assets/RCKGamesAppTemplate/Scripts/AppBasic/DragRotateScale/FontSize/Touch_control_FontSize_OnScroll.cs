using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Touch_control_FontSize_OnScroll : MonoBehaviour
{
    //scale variable
    #region 
    [Header("Does it need to change font Size")]
    public bool is_changeSize;

    [Header("scale speed")]
    [Range(0.001f, 0.02f)]
    public float changeSize_speed;

    //change font size
    [Header("Font Size")]
    public TextMeshProUGUI text;
    public int minFontSize;
    public int maxFontSize;


    private Touch oldTouch1;  //Last touched point 1 (finger 1)
    private Touch oldTouch2;  //Last touched point 2 (finger 2)
    #endregion


    void OnEnable()
    {

    }

    void Update()
    {
        //Without touching, there is no dragging, and dragging is not timed
        if (Input.touchCount <= 0)
        {
            return;
        }
        //Single touch
        else if (Input.touchCount == 1)
        {
            //Get touch location
            Touch touch = Input.touches[0];
            Vector3 pos = touch.position;
        }
        //Multipoint, zoom
        else
        {
            if (is_changeSize == true)
            {
                //Multi-touch, zoom in and zoom out
                Touch newTouch1 = Input.GetTouch(0);
                Touch newTouch2 = Input.GetTouch(1);

                //Point 2 just started to touch the screen, only record, no processing
                if (newTouch2.phase == TouchPhase.Began)
                {
                    this.oldTouch2 = newTouch2;
                    this.oldTouch1 = newTouch1;
                    return;
                }

                //The difference between the two distances, positive means zoom in gesture, negative means zoom out gesture
                float offset = Vector2.Distance(newTouch1.position, newTouch2.position) - Vector2.Distance(oldTouch1.position, oldTouch2.position);

                //Magnification factor, one pixel is calculated as 0.01 times (100 adjustable)
                float scaleFactor = offset * this.changeSize_speed;

                //Modify scale
                if ((text.fontSize + scaleFactor) <= maxFontSize && (text.fontSize + scaleFactor) >= minFontSize)
                {
                    text.fontSize = (text.fontSize + scaleFactor);
                }

                if(text.fontSize < minFontSize)
                {
                    text.fontSize = minFontSize;
                }

                if (text.fontSize > maxFontSize)
                {
                    text.fontSize = maxFontSize;
                }

                //Remember the latest touch point and use it next time
                this.oldTouch1 = newTouch1;
                this.oldTouch2 = newTouch2;
            }
        }
    }

}

