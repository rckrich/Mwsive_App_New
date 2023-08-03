using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacionEntradaADN : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject imagen;
   
    private void OnEnable()
    {
        UIAniManager.instance.ScaleAnimationEnter(imagen);
    }
}
