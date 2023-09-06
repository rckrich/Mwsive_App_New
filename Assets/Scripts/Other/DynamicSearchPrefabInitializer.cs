using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DynamicSearchPrefabInitializer : MonoBehaviour
{
    public Image Portada;
    
    public string SpotifyID;
    private string ExternalURL;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Subtitle;

    // Start is called before the first frame update

    public void InitializeMwsiveUser(string _text, string id){
        if(_text.Length > 27){
            string _text2 = "";
            for (int i = 0; i < 27; i++)
            {
                
                _text2 =_text2 + _text[i];
                
            }
            _text2 = _text2 + "...";
            Title.text = _text2;
        }else{
            Title.text = _text;
        }
        gameObject.SetActive(true);
        SpotifyID = id;
    }

    public void InitializeMwsiveUserwithImage(string _text, string id, string _image){
        if(_text.Length > 27){
            string _text2 = "";
            for (int i = 0; i < 27; i++)
            {
                
                _text2 =_text2 + _text[i];
                
            }
            _text2 = _text2 + "...";
            Title.text = _text2;
        }else{
            Title.text = _text;
        }
        gameObject.SetActive(true);
        SpotifyID = id;
        ImageManager.instance.GetImage(_image, Portada, (RectTransform)this.transform);
    }

    public void InitializeSingle(string _text, string id, string Url){
        if(_text.Length > 27){
            string _text2 = "";
            for (int i = 0; i < 27; i++)
            {
                
                _text2 =_text2 + _text[i];
                
            }
            _text2 = _text2 + "...";
            Title.text = _text2;
        }else{
            Title.text = _text;
        }
        
        gameObject.SetActive(true);
        SpotifyID = id;
        ExternalURL = Url;

    }
    public void InitializeSingleWithImage(string _name,  string _image, string id, string Url){
        if(_name.Length > 27){
            string _text2 = "";
            for (int i = 0; i < 27; i++)
            {
                
                _text2 =_text2 + _name[i];
                
            }
            _text2 = _text2 + "...";
            Title.text = _text2;
        }else{
            Title.text = _name;
        }
        ImageManager.instance.GetImage(_image, Portada, (RectTransform)this.transform);
        gameObject.SetActive(true);
        SpotifyID = id;
        ExternalURL = Url;
    }

    public void InitializeDoubleWithImage(string _Title, string _Subtitle, string _Image, string id,  string Url){
        if(_Title.Length > 27){
            string _text2 = "";
            for (int i = 0; i < 27; i++)
            {
                
                _text2 =_text2 + _Title[i];
                
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
        ImageManager.instance.GetImage(_Image, Portada, (RectTransform)this.transform);
        gameObject.SetActive(true);
        SpotifyID = id;
        ExternalURL = Url;
    }

    public void InitializeDouble(string _Title, string _Subtitle, string id, string Url){
        
        if(_Title.Length > 27){
            string _text2 = "";
            for (int i = 0; i < 27; i++)
            {
                
                _text2 =_text2 + _Title[i];
                
            }
            _text2 = _text2 + "...";
            Title.text = _text2;
        }else{
            Title.text = _Title;
        };
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
        gameObject.SetActive(true);
        SpotifyID = id;
        ExternalURL = Url;
    }

    public void OnClickOpenSpotify(){
        Application.OpenURL(ExternalURL);
    }

    public void OnClickInitializePrefabSong(){
        NewScreenManager.instance.ChangeToSpawnedView("cancion");
        GameObject Instance = NewScreenManager.instance.GetCurrentView().gameObject;
        Instance.transform.SetParent(GameObject.Find("SpawnableCanvas").transform); 
        Instance.GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
        Instance.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);
        Instance.transform.localScale = new Vector3(1,1,1);

        Instance.GetComponent<TrackViewModel>().trackID = SpotifyID;
    }


    public void OnClickInitializePrefabAlbum(){
        NewScreenManager.instance.ChangeToSpawnedView("album");
        GameObject Instance = NewScreenManager.instance.GetCurrentView().gameObject;
        Instance.transform.SetParent(GameObject.Find("SpawnableCanvas").transform); 
        Instance.GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
        Instance.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);
        Instance.transform.localScale = new Vector3(1,1,1);

        Instance.GetComponent<AlbumViewModel>().id = SpotifyID;
        
    }
    public void OnClickInitializeCurator(){
        NewScreenManager.instance.ChangeToSpawnedView("profile");
        GameObject Instance = NewScreenManager.instance.GetCurrentView().gameObject;
        Debug.Log(SpotifyID);
        Instance.GetComponent<ProfileViewModel>().Initialize(SpotifyID);
    }



    public void OnClickInitializePrefabPlaylist(){
        
        NewScreenManager.instance.ChangeToSpawnedView("playlist");
        GameObject Instance = NewScreenManager.instance.GetCurrentView().gameObject;
        Instance.transform.SetParent(GameObject.Find("SpawnableCanvas").transform); 
        Instance.GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
        Instance.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);
        Instance.transform.localScale = new Vector3(1,1,1);

        Instance.GetComponent<PlaylistViewModel>().SetPlaylistID(SpotifyID);
        Instance.GetComponent<PlaylistViewModel>().GetPlaylist();
    }

}
