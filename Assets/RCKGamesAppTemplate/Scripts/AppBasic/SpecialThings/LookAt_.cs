using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt_ : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        transform.LookAt(target);
    }

}
