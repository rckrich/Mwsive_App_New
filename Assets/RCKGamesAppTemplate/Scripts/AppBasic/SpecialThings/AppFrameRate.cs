using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppFrameRate : MonoBehaviour
{

    void OnEnable()
    {
        Application.targetFrameRate = 60;
    }

}
