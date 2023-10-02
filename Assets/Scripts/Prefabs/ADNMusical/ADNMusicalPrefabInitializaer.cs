using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ADNMusicalPrefabInitializaer : MonoBehaviour
{
    public Image Portada;
    public Image Background;
    public List<Color32> Colors = new List<Color32>(); 
    private bool _IsTheObjectVisible;
    
    List<Artists> artists = new List<Artists>();
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Subtitle;
    private string id;

    // Start is called before the first frame update
    
    public void InitializeSingleWithBackgroundWithImage(string _text, string Image, string _id){
        ImageManager.instance.GetImage(Image, Portada, (RectTransform)this.transform);
        if(_text.Length > 27){
            string _text2 = "";
            for (int k = 0; k < 27; k++)
            {
                
                _text2 =_text2 + _text[k];
                
            }
            _text2 = _text2 + "...";
            Title.text = _text2;
        }else{
            Title.text = _text;
        }





        
        int i = int.Parse(gameObject.name.Split("-")[1]);
        i--;
        Color32 _color = Colors[i % 3];
        Background.GetComponent<Image>().color =_color;
        id = _id;
        gameObject.SetActive(true);
    }
    public void InitializeSingleWithBackgroundNoImage(string _text, string _id){
        
        if(_text.Length > 27){
            string _text2 = "";
            for (int k = 0; k < 27; k++)
            {
                
                _text2 =_text2 + _text[k];
                
            }
            _text2 = _text2 + "...";
            Title.text = _text2;
        }else{
            Title.text = _text;
        }
        int i = int.Parse(gameObject.name.Split("-")[1]);
        i--;
        Color32 _color = Colors[i % 3];
        Background.GetComponent<Image>().color =_color;
        id = _id;
        gameObject.SetActive(true);
    }


     public void InitializeDoubleWithBackgroundWithImage(string _Title, string _Subtitle, string Image, string _id){
        ImageManager.instance.GetImage(Image, Portada, (RectTransform)this.transform);
        if(_Title.Length > 27){
            string _text2 = "";
            for (int k = 0; k < 27; k++)
            {
                
                _text2 =_text2 + _Title[k];
                
            }
            _text2 = _text2 + "...";
            Title.text = _text2;
        }else{
            Title.text = _Title;
        }
        if(_Subtitle.Length > 27){
            string _text2 = "";
            for (int k = 0; k < 27; k++)
            {
                
                _text2 =_text2 + _Subtitle[k];
                
            }
            _text2 = _text2 + "...";
            Subtitle.text = _text2;
        }else{
            Subtitle.text = _Subtitle;
        }
        int i = int.Parse(gameObject.name.Split("-")[1]);
        i--;
        Color32 _color = Colors[i % 3];
        Background.GetComponent<Image>().color =_color;
        id = _id;
        gameObject.SetActive(true);
    }
    public void InitializeDoubleWithBackgroundNoImage(string _Title, string _Subtitle, string _id){
        
        if(_Title.Length > 27){
            string _text2 = "";
            for (int k = 0; k < 27; k++)
            {
                
                _text2 =_text2 + _Title[k];
                
            }
            _text2 = _text2 + "...";
            Title.text = _text2;
        }else{
            Title.text = _Title;
        }

        if(_Subtitle.Length > 27){
            string _text2 = "";
            for (int k = 0; k < 27; k++)
            {
                
                _text2 =_text2 + _Subtitle[k];
                
            }
            _text2 = _text2 + "...";
            Subtitle.text = _text2;
        }else{
            Subtitle.text = _Subtitle;
        }



        
        int i = int.Parse(gameObject.name.Split("-")[1]);
        i--;
        Color32 _color = Colors[i % 3];
        Background.GetComponent<Image>().color =_color;
        id = _id;
        gameObject.SetActive(true);
    }


    public void OnClickButton(){
        ADNDynamicScroll.instance.HideShowHeader();
        ADNDynamicScroll.instance.ShowAllInstances(Title.text, id);
    }
}
