using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    public Sprite image;
    public Sprite oldImage;
    public int count = 0;
    public Vector3 newVector= new Vector3(-134f, 0f, 0f);
    public Vector3 oldVector = new Vector3(-150f, 0f, 0f);
    public Vector3 newSize = new Vector3(50f, 50f, 0f);
    public Vector3 oldSize = new Vector3(50f, 50f, 0f);

    public void OnClickToggle()
    {
        if (count == 0)
        {
            gameObject.GetComponent<Image>().sprite = image;
            gameObject.GetComponent<RectTransform>().anchoredPosition = newVector;
            gameObject.GetComponent<RectTransform>().sizeDelta = newSize;
            count++;
        }else if (count == 1)
        {
            gameObject.GetComponent<Image>().sprite = oldImage;
            gameObject.GetComponent<RectTransform>().anchoredPosition = oldVector;
            gameObject.GetComponent<RectTransform>().sizeDelta = oldSize;
            count--;
        }
       
    }

    public void True()
    {
        gameObject.GetComponent<Image>().sprite = image;
        gameObject.GetComponent<RectTransform>().anchoredPosition = newVector;
        count = 1;;
    }
}
