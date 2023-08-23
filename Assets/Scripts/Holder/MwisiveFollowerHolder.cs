using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MwisiveFollowerHolder : ViewModel
{
    public TextMeshProUGUI name;
    public Image curatorPicture;
    public void Initialize(string _name)
    {
        if (_name != null)
        {
            if (_name.Length > 27)
            {
                string _text2 = "";
                for (int i = 0; i < 27; i++)
                {
                    _text2 = _text2 + _name[i];
                }
                _text2 = _text2 + "...";
                name.text = _text2;
            }
            else
            {
                name.text = _name;
            }
        }
    }

    public void Initialize(string _name, string _pictureURL)
    {
        if (_name != null)
        {
            if (_name.Length > 27)
            {
                string _text2 = "";
                for (int i = 0; i < 27; i++)
                {
                    _text2 = _text2 + _name[i];
                }
                _text2 = _text2 + "...";
                name.text = _text2;
            }
            else
            {
                name.text = _name;
            }
        }
        if (_pictureURL != null)
            ImageManager.instance.GetImage(_pictureURL, curatorPicture, (RectTransform)this.transform);
    }

    /*public void SetImage(string _pictureURL)
    {
        Debug.Log("image url: " + _pictureURL);
        ImageManager.instance.GetImage(_pictureURL, curatorPicture, (RectTransform)this.transform);
    }*/
}
