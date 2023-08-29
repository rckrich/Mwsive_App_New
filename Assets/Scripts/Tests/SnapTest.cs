using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class SnapTest : MonoBehaviour
{
    public List<GameObject> Images;
    public GameObject Content;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(int num){
       Content.transform.DOMoveX(-Images[num].transform.position.x, .5f);
    }


}
