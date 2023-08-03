using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DynamicSearchPrefabInitializer : MonoBehaviour
{
    public Image Portada;
    
    public string SpotifyID;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Subtitle;


    // Start is called before the first frame update

    public void InitializeSingle(string _text, string id){
        
        Title.text = _text;
        gameObject.SetActive(true);
        SpotifyID = id;

    }
    public void InitializeSingleWithImage(string _name,  string _image, string id){
        Title.text = _name;
        ImageManager.instance.GetImage(_image, Portada, (RectTransform)this.transform);
        gameObject.SetActive(true);
        SpotifyID = id;
    }

    public void InitializeDoubleWithImage(string _Title, string _Subtitle, string _Image, string id){
        Title.text = _Title;
        Subtitle.text = _Subtitle;
        ImageManager.instance.GetImage(_Image, Portada, (RectTransform)this.transform);
        gameObject.SetActive(true);
        SpotifyID = id;
    }

    public void InitializeDouble(string _Title, string _Subtitle, string id){
        
        Title.text = _Title;
        Subtitle.text = _Subtitle;
        gameObject.SetActive(true);
        SpotifyID = id;
    }


}
