using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSize : MonoBehaviour
{

    public RectTransform _size;
    public Vector2 X;
    public Vector2 Y;
    // Start is called before the first frame update
    public void ChangeSize(){
        _size.offsetMax = new Vector2(X.y, Y.y);
        _size.offsetMin = new Vector2(X.x, Y.x);

    }
}
