using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoTest : MonoBehaviour
{
    private static InfoTest _instance;

    public static InfoTest instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<InfoTest>();
            }
            return _instance;
        }
    }

    public string[] GettracksID;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
