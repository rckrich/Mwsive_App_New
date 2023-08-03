using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextGradiant : MonoBehaviour
{
    // Start is called before the first frame update
   // public GradientColorKey[] color = new GradientColorKey[2];
    private Color colorGradient;
    public Color color1;
    public Color color2;
    public float soilMoisture = 0;
    public Gradient gradient = new Gradient();


    void Start()
    {

        
        /*var color = new GradientColorKey[2];
        color[0] = new GradientColorKey(color1, 0.0f);
        color[1] = new GradientColorKey(color2, 1.0f);
        var alphas = new GradientAlphaKey[2];
        alphas[0] = new GradientAlphaKey(1.0f, 0.0f);
        alphas[1] = new GradientAlphaKey(0.0f, 1.0f);
        gradient.SetKeys(color, alphas);*/
        colorGradient = gradient.Evaluate(soilMoisture);
        gameObject.GetComponent<Image>().color = colorGradient;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
