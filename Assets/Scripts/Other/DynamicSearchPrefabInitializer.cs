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
    public GameObject PrefabMenu;


    // Start is called before the first frame update

    public void InitializeSingle(string _text, string id, string Url){
        
        Title.text = _text;
        gameObject.SetActive(true);
        SpotifyID = id;
        ExternalURL = Url;

    }
    public void InitializeSingleWithImage(string _name,  string _image, string id, string Url){
        Title.text = _name;
        ImageManager.instance.GetImage(_image, Portada, (RectTransform)this.transform);
        gameObject.SetActive(true);
        SpotifyID = id;
        ExternalURL = Url;
    }

    public void InitializeDoubleWithImage(string _Title, string _Subtitle, string _Image, string id,  string Url){
        Title.text = _Title;
        Subtitle.text = _Subtitle;
        ImageManager.instance.GetImage(_Image, Portada, (RectTransform)this.transform);
        gameObject.SetActive(true);
        SpotifyID = id;
        ExternalURL = Url;
    }

    public void InitializeDouble(string _Title, string _Subtitle, string id, string Url){
        
        Title.text = _Title;
        Subtitle.text = _Subtitle;
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
        Instance.transform.SetParent(GameObject.Find("Main Canvas").transform); 
        Instance.GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
        Instance.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);
        Instance.transform.localScale = new Vector3(1,1,1);

        Instance.GetComponent<TrackViewModel>().trackID = SpotifyID;
        Instance.GetComponent<TrackViewModel>().GetTrack();
        Instance.GetComponent<TrackViewModel>().GetRecommendations();
    }


    public void OnClickInitializePrefabAlbum(){
        NewScreenManager.instance.ChangeToSpawnedView("cancion");
        GameObject Instance = NewScreenManager.instance.GetCurrentView().gameObject;
        Instance.transform.SetParent(GameObject.Find("Main Canvas").transform); 
        Instance.GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
        Instance.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);
        Instance.transform.localScale = new Vector3(1,1,1);

        Instance.GetComponent<TrackViewModel>().trackID = SpotifyID;
        Instance.GetComponent<TrackViewModel>().GetTrack();
        Instance.GetComponent<TrackViewModel>().GetRecommendations();
    }



    public void OnClickInitializePrefabPlaylist(){
        
        NewScreenManager.instance.ChangeToSpawnedView("playlist");
        GameObject Instance = NewScreenManager.instance.GetCurrentView().gameObject;
        Instance.transform.SetParent(GameObject.Find("Main Canvas").transform); 
        Instance.GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
        Instance.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);
        Instance.transform.localScale = new Vector3(1,1,1);

        Instance.GetComponent<PlaylistViewModel>().id = SpotifyID;
        Instance.GetComponent<PlaylistViewModel>().GetPlaylist();
    }

}
