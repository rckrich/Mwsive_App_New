using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartConnectionSpotify : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpotifyConnectionManager.instance.StartConnection(FillTokenText);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void FillTokenText(object[] _value)
    {
        Debug.Log(_value); 
    }
}
