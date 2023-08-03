using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Mouse_drag_rotate_scale_control : MonoBehaviour
{
    //init transform
    private Vector3 init_position;
    private Quaternion init_rotation;
    private Vector3 init_scale;

    //drag variable
    #region 
    [Header("Does it need to drag")]
    public bool is_dragable;

    //is dragging
    private bool is_dragging;

    //The distance from the sending ray camera to the Z axis of the collision body
    private float distance_z;

    //When clicking and dragging, the deviation distance of the mouse to the center of the object
    private Vector3 drag_offset;
    #endregion

    //rotation variable
    #region 
    [Header("Does it need to rotate")]
    public bool is_rotation;

    [Header("Rotational speed 0~1")]
    [Range(0, 1)]
    public float rotation_speed;
    #endregion

    //scale variable
    #region 
    [Header("Does it need to scale")]
    public bool is_scale;

    public bool resetScaleOnEnable = false;

    [Header("max scale and min scale")]
    public float max_scale;
    public float min_scale;
    #endregion

    [Header("mouse drag texture")]
    public Texture2D mouse_drag_texture;

    //the statu if the mouse is down
    private bool is_mouse_down = false;

    //the mouse position
    private float mouse_position_x;
    private float mouse_position_y;


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

    // Update is called once per frame
    void Update()
    {
        //set the mouse is down
        #region 
        if (Input.GetMouseButtonDown(0))
        {
            this.is_mouse_down = true;
            this.mouse_position_x = Input.mousePosition.x;
            this.mouse_position_y = Input.mousePosition.y;

            //Send a ray, drag the object when you hit it
            #region 
            if (this.is_dragable)
            {
                if (this.is_dragging == false)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.name == this.gameObject.name)
                        {
                            //Set mouse style
                            Cursor.SetCursor(mouse_drag_texture, Vector2.zero, CursorMode.Auto);

                            //Start dragging
                            this.is_dragging = true;

                            //Reduce size and give user feedback
                            this.transform.localScale = this.transform.localScale * 0.9f;

                            //Get a deviation position and the Z-axis distance from the camera to the control object
                            this.distance_z = hit.transform.position.z - Camera.main.transform.position.z;
                            this.drag_offset = hit.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.distance_z));
                        }
                    }
                }
            }
            #endregion
        }
        else if (Input.GetMouseButtonUp(0))
        {
            this.is_mouse_down = false;

            //Raise the mouse, no longer drag, the size is restored
            if (this.is_dragging == true)
            {
                this.is_dragging = false;
                this.transform.localScale = this.transform.localScale / 9f * 10;

                //Restore mouse style
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }

        }
        #endregion

        //drag and rotate
        #region 
        if (this.is_mouse_down)
        {
            //drag
            #region 
            if (this.is_dragable && this.is_dragging)
            {
                this.transform.position =
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.distance_z)) + drag_offset;
            }
            #endregion

            //rotation
            #region 
            else if (this.is_rotation == true)
            {
                //if (this.rotation_type == Rotation_type.rotation_along_x)
                //{
                //    transform.Rotate(Vector3.down * (Input.mousePosition.x - this.mouse_position_x) * this.rotation_speed, Space.World);
                //}
                //else if (this.rotation_type == Rotation_type.rotation_along_y)
                //{
                //    transform.Rotate(Vector3.right * (Input.mousePosition.y - this.mouse_position_y) * this.rotation_speed, Space.World);
                //}

                Vector2 deltaPos = new Vector2((Input.mousePosition.x - this.mouse_position_x), (Input.mousePosition.y - this.mouse_position_y));

                if (Mathf.Abs(deltaPos.x) > Mathf.Abs(deltaPos.y))
                {
                    if (Mathf.Abs(deltaPos.x) > 5)
                    {
                        transform.Rotate(Vector3.down * deltaPos.x * this.rotation_speed, Space.Self); //todo 下次优化 把0.1提出去
                    }
                }
                else
                {
                    if (Mathf.Abs(deltaPos.y) > 5)
                    {
                        transform.Rotate(Vector3.right * deltaPos.y * this.rotation_speed, Space.World); //todo 下次优化 把0.1提出去
                    }
                }

                this.mouse_position_x = Input.mousePosition.x;
                this.mouse_position_y = Input.mousePosition.y;
            }
            #endregion
        }
        #endregion

        //scale
        #region 
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (this.transform.localScale.x > this.min_scale)
                this.transform.localScale = this.transform.localScale * 0.98f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (this.transform.localScale.x < this.max_scale)
                this.transform.localScale = this.transform.localScale * 1.02f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") == 0)
        {

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

