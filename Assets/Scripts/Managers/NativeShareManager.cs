using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// key namespaces
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit;
// internal namespace
public class NativeShareManager : MonoBehaviour
{

    private static NativeShareManager _instance;

    public static NativeShareManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<NativeShareManager>();
            }
            return _instance;
        }
    }
    
    public SurfManager Surf;
    public MenuOptions Menu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable ()
    {
        DeepLinkServices.OnCustomSchemeUrlOpen     += OnCustomSchemeUrlOpen;
        DeepLinkServices.OnUniversalLinkOpen       += OnUniversalLinkOpen;
    }

    private void OnDisable ()
    {
        DeepLinkServices.OnCustomSchemeUrlOpen    -= OnCustomSchemeUrlOpen;
        DeepLinkServices.OnUniversalLinkOpen      -= OnUniversalLinkOpen;
    }
    private void OnCustomSchemeUrlOpen (DeepLinkServicesDynamicLinkOpenResult result)
    {
        Debug.Log("Handle deep link : " + result.Url);
        string[] Link = result.Url.ToString().Split("/");

        for (int i = 0; i < Link.Length; i++)
        {
            Debug.Log(Link[i]);
            if(Link[i] == "song"){
                
                Surf.SpawnSharePrefab(Link[i+1]);
                NewScreenManager.instance.ChangeToMainView(ViewID.SurfViewModel);
                Menu.OnClick(1);
            }
            if(Link[i] == "profile"){
                Surf.GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().OnClickForcePausePreview();
                Surf.canSwipe = false;
                NewScreenManager.instance.ChangeToSpawnedView("profile");
                NewScreenManager.instance.GetCurrentView().Initialize(Link[i+1]);
            }
        }
        
    }



    public void TestShareSong(){

        SpotifyConnectionManager.instance.GetTrack("6kFDvPj3FVpQ90HZ5PacxE", Callback_SpawnSharePrefab);  
        
    }

    private void Callback_SpawnSharePrefab(object[] _value)
    {

        TrackRoot trackRoot = (TrackRoot)_value[1];
        if (trackRoot != null)
        {
            if (trackRoot.preview_url != null)
            {
                MwsiveData instance = new MwsiveData();
                string artists = "";

                foreach (Artist artist in trackRoot.artists)
                {
                    artists = artists + artist.name + ", ";
                }

                artists = artists.Remove(artists.Length - 2);
                string currentPlayListName = AppManager.instance.isLogInMode ? AppManager.instance.GetCurrentPlaylist().name : "";

                instance.playlist_name = currentPlayListName;
                instance.song_name = trackRoot.name;
                instance.album_name = trackRoot.album.name;
                instance.artists = artists;
                instance.album_image_url = trackRoot.album.images[0].url;
                instance.id = trackRoot.id;
                instance.uri = trackRoot.uri;
                instance.preview_url = trackRoot.preview_url;
                instance.external_url = trackRoot.external_urls.spotify;


                Surf.MwsiveSongsData.Insert(Surf.CurrentPosition, instance);

                if(NewScreenManager.instance.GetCurrentView().viewID == ViewID.SurfViewModel && SurfController.instance.ReturnCurrentView() == Surf.gameObject)
                {
                    PoolManager.instance.RecoverPooledObject(Surf.gameObject);
                    Surf.SurfManagerLogicInitialize();
                }
                else
                {
                    NewScreenManager.instance.ChangeToMainView(ViewID.SurfViewModel);
                    
                    Surf.gameObject.SetActive(true);
                    Debug.Log(SurfController.instance.ReturnCurrentView().name);
                }

                
                
            }
            else
            {
                UIMessage.instance.UIMessageInstanciate("Esta canción no esta disponible");
            }
        }
        else
        {
            UIMessage.instance.UIMessageInstanciate("Esta canción no esta disponible");
        }


    }



    public void TestShareProfile()
    {
        
        
        NewScreenManager.instance.ChangeToSpawnedView("profile");
        if (SurfController.instance.ReturnCurrentView().activeSelf)
        {
            SurfController.instance.ReturnCurrentView().SetActive(false);
            NewScreenManager.instance.GetCurrentView().GetComponent<ProfileViewModel>().IsNativeShare = true;
        }
        NewScreenManager.instance.GetCurrentView().Initialize("12133115564");
        SpotifyPreviewAudioManager.instance.ForcePause();
    }

    private void OnUniversalLinkOpen (DeepLinkServicesDynamicLinkOpenResult result)
    {
        Debug.Log("Handle deep link : " + result.Url);
        string[] Link = result.Url.ToString().Split("/");

        for (int i = 0; i < Link.Length; i++)
        {
            Debug.Log(Link[i]);
            if(Link[i] == "song"){
                
                Surf.SpawnSharePrefab(Link[i+1]);
                NewScreenManager.instance.ChangeToMainView(ViewID.SurfViewModel);
            }
            if (Link[i] == "profile")
            {
                Surf.GetCurrentPrefab().GetComponent<ButtonSurfPlaylist>().OnClickForcePausePreview();
                Surf.canSwipe = false;
                NewScreenManager.instance.ChangeToSpawnedView("profile");
                NewScreenManager.instance.GetCurrentView().Initialize(Link[i + 1]);
            }
        }
    }

    public void OnClickShareMwsiveSong(string spotifyId, bool boolswitch){
        
        if(boolswitch){
            ShareSheet shareSheet = ShareSheet.CreateInstance();
        shareSheet.AddText("rck://mwsive/song" + spotifyId);
        shareSheet.AddURL(URLString.URLWithPath("rck://mwsive/song" + spotifyId));
        shareSheet.SetCompletionCallback((result, error) => {
            Debug.Log("Share Sheet was closed. Result code: " + result.ResultCode);
        });
        shareSheet.Show();
       
        }
        
    }

    public void OnClickShareMwsiveProfile(string profileid){
       
            ShareSheet shareSheet = ShareSheet.CreateInstance();
        shareSheet.AddText("rck://mwsive/profile" + profileid);
        shareSheet.AddURL(URLString.URLWithPath("rck://mwsive/profile" + profileid));
        shareSheet.SetCompletionCallback((result, error) => {
            Debug.Log("Share Sheet was closed. Result code: " + result.ResultCode);
        });
        shareSheet.Show();
        
    }
    
}
