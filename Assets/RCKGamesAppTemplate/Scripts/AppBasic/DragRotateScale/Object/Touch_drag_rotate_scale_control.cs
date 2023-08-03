using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Touch_drag_rotate_scale_control : MonoBehaviour
{

    //init transform
    private Vector3 init_position;
    private Quaternion init_rotation;
    private Vector3 init_scale;

    //drag variable
    #region 
    [Header("Does it need to drag")]
    public bool is_dragable;

    [Header("long touch drag time")]
    public float long_touch_drag_time;

    private bool is_dragging = false;           //Is dragging
    private int start_time_stamp;               //Timestamp when the ray started
    private bool is_long_touch_timing = false;  //Whether it is timing, drag and hold
    private float distance_z;                   //The distance from the sending ray camera to the Z axis of the collision body
    private Vector3 drag_offset;                //When clicking and dragging, the deviation distance of the mouse to the center of the object
    #endregion

    //rotation variable
    #region 
    [Header("Does it need to rotate")]
    public bool is_rotation;

    //[Header("If it need rotate,choose a rotation type")]
    //public Rotation_type rotation_type;

    [Header("Rotational speed 0~1")]
    [Range(0, 1)]
    public float rotation_speed;
    #endregion

    //scale variable
    #region 
    [Header("Does it need to scale")]
    public bool is_scale;

    public bool resetScaleOnEnable = false;

    [Header("scale speed")][Range(0.001f, 0.02f)]
    public float scale_speed;

    [Header("max scale and min scale")]
    public float max_scale;
    public float min_scale;

    private Touch oldTouch1;  //Last touched point 1 (finger 1)
    private Touch oldTouch2;  //Last touched point 2 (finger 2)
    #endregion


    void OnEnable()
    {
        if (resetScaleOnEnable)
        {
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    //get the init transform
    void Start()
    {
        this.init_position = this.transform.position;
        this.init_rotation = this.transform.rotation;
        this.init_scale = this.transform.localScale;
    }

    void Update()
    {
        //Without touching, there is no dragging, and dragging is not timed
        #region 
        if (Input.touchCount <= 0)
        {
            //Not dragged
            this.is_dragging = false;
            this.is_long_touch_timing = false;
            return;
        }
        #endregion

        //Single touch
        #region 
        else if (Input.touchCount == 1)
        {
            //Get touch location
            Touch touch = Input.touches[0];
            Vector3 pos = touch.position;

            //Drag
            #region 
            if (this.is_dragable == true)
            {
                //Emission rays
                #region 
                if (this.is_dragging == false)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(pos);
                    if (Physics.Raycast(ray, out hit) && (hit.transform.name == this.gameObject.name))
                    {
                        //Timestamp when the ray started
                        if (this.is_long_touch_timing == false)
                        {
                            //Get timestamp
                            this.start_time_stamp = System.Environment.TickCount;

                            //Start timing when hitting an object
                            this.is_long_touch_timing = true;
                        }

                        if (System.Environment.TickCount - this.start_time_stamp >= 1000 * this.long_touch_drag_time)
                        {
                            this.is_dragging = true;
                            this.transform.localScale = this.transform.localScale * 0.9f;

                            //Get a deviation position and the Z-axis distance from the camera to the control object
                            this.distance_z = hit.transform.position.z - Camera.main.transform.position.z;
                            this.drag_offset = hit.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.distance_z));
                        }
                    }
                }
                #endregion

                //Drag object
                #region 
                else if (is_dragging && touch.phase == TouchPhase.Moved)
                {
                    //Limit the conditions that cannot be dragged off the screen
                    //if (Input.mousePosition.x > Screen.width / 5 && Input.mousePosition.x < Screen.width / 5 * 4 &&
                    // Input.mousePosition.y > Screen.height / 5 && Input.mousePosition.y < Screen.height / 5 * 4)
                    this.transform.position =
                        Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.distance_z)) + drag_offset;

                }
                #endregion

                //Raise your finger to exit the drag
                #region 
                else if (is_dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
                {
                    this.is_dragging = false;
                    this.is_long_touch_timing = false;
                    this.transform.localScale = this.transform.localScale * 1.112f;
                }
                #endregion
            }



            #endregion

            //Spin
            #region 
            if (this.is_rotation == true)
            {
                if (touch.phase == TouchPhase.Moved && this.is_dragging == false)
                {
                    Vector2 deltaPos = touch.deltaPosition;

                    if (Mathf.Abs(deltaPos.x) > Mathf.Abs(deltaPos.y))
                    {
                        if (Mathf.Abs(deltaPos.x) > 5)
                        {
                            //Cancel drag
                            this.is_dragging = false;
                            this.is_long_touch_timing = false;

                            transform.Rotate(Vector3.down * deltaPos.x * this.rotation_speed, Space.Self); //todo Next time optimize, put 0.1 out
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(deltaPos.y) > 5)
                        {
                            //Cancel drag
                            this.is_dragging = false;
                            this.is_long_touch_timing = false;

                            transform.Rotate(Vector3.right * deltaPos.y * this.rotation_speed, Space.World); //todo Next time optimize, put 0.1 out
                        }
                    }
                }
            }

           
            #endregion
        }
        #endregion

        //Multipoint, zoom
        #region 
        else
        {
            if (is_scale == true)
            {
                //No long press and drag, the timer is reset to zero
                this.is_dragging = false;
                this.is_long_touch_timing = false;

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
                float scaleFactor = offset * this.scale_speed;

                //Get current size
                Vector3 localScale = transform.localScale;

                //Modify scale
                if ((localScale.x + scaleFactor) < this.max_scale && (localScale.x + scaleFactor) > this.min_scale)
                {
                    transform.localScale = new Vector3(localScale.x + scaleFactor, localScale.y + scaleFactor, localScale.z + scaleFactor);
                }

                //Remember the latest touch point and use it next time
                this.oldTouch1 = newTouch1;
                this.oldTouch2 = newTouch2;
            }
        }
        #endregion
    }

    //reset the transform 
    public void reset_transform()
    {
        this.transform.position = this.init_position;
        this.transform.rotation = this.init_rotation;
        this.transform.localScale = this.init_scale;
    }

}
