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
public class NativeShare : MonoBehaviour
{

    private bool boolswitch = true;
    public SurfManager Surf;
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
            }
        }
        
    }

    private void OnUniversalLinkOpen (DeepLinkServicesDynamicLinkOpenResult result)
    {
        Debug.Log("Handle deep link : " + result.Url);
        string[] Link = result.Url.ToString().Split("/");

        for (int i = 0; i < Link.Length; i++)
        {
            Debug.Log(Link[i]);
            if(Link[i] == "song"){
                Debug.Log("aa");
                Surf.SpawnSharePrefab(Link[i+1]);
                NewScreenManager.instance.ChangeToMainView(ViewID.SurfViewModel);
            }
        }
    }

    public void OnClickShareMwsiveSong(string spotifyId){
        
        if(boolswitch){
            ShareSheet shareSheet = ShareSheet.CreateInstance();
        shareSheet.AddText("rck://mwsive/song" + spotifyId);
        shareSheet.AddURL(URLString.URLWithPath("rck://mwsive/song" + spotifyId));
        shareSheet.SetCompletionCallback((result, error) => {
            Debug.Log("Share Sheet was closed. Result code: " + result.ResultCode);
        });
        shareSheet.Show();
       
        }
        boolswitch = !boolswitch;
    }
    
}
