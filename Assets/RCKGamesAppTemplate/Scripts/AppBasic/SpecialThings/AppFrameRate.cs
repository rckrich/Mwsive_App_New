using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppFrameRate : MonoBehaviour
{

    void Start()
    {
        Application.targetFrameRate = 60;
    }

}
