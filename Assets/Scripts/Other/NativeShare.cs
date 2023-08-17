using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// key namespaces
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit;
// internal namespace
public class NativeShare : MonoBehaviour
{

    private bool boolswitch = true;
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
    }

    private void OnUniversalLinkOpen (DeepLinkServicesDynamicLinkOpenResult result)
    {
        Debug.Log("Handle deep link : " + result.Url);
    }

    public void OnClickShareMwsiveSong(){
        
        if(boolswitch){
            ShareSheet shareSheet = ShareSheet.CreateInstance();
        shareSheet.AddText("Text");
        shareSheet.AddURL(URLString.URLWithPath("https://www.google.com"));
        shareSheet.SetCompletionCallback((result, error) => {
            Debug.Log("Share Sheet was closed. Result code: " + result.ResultCode);
        });
        shareSheet.Show();
       
        }
        boolswitch = !boolswitch;
    }
    
}
