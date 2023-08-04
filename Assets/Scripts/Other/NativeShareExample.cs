using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

public class NativeShareExample : MonoBehaviour
{
    private void OnEnable()
    {
        DeepLinkServices.OnCustomSchemeUrlOpen += OnCustomSchemeUrlOpen;
        DeepLinkServices.OnUniversalLinkOpen += OnUniversalLinkOpen;
    }

    private void OnDisable()
    {
        DeepLinkServices.OnCustomSchemeUrlOpen -= OnCustomSchemeUrlOpen;
        DeepLinkServices.OnUniversalLinkOpen -= OnUniversalLinkOpen;
    }

    private void OnCustomSchemeUrlOpen(DeepLinkServicesDynamicLinkOpenResult result)
    {
        Debug.Log("Handle deep link : " + result.Url);
    }

    private void OnUniversalLinkOpen(DeepLinkServicesDynamicLinkOpenResult result)
    {
        Debug.Log("Handle deep link : " + result.Url);
    }
}
