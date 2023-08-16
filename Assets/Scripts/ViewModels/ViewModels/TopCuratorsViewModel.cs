using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopCuratorsViewModel : ViewModel
{

    public GameObject curatorPrefab;
    public Transform curatorScrollContent;

    void Start()
    {
        MwsiveConnectionManager.instance.GetRecommendedCurators(Callback_GetRecommendedCurators);
    }

    private void Callback_GetRecommendedCurators(object[] _list)
    {

        MwsiveRecommendedCuratorsRoot mwsiveRecommendedCuratorsRoot = (MwsiveRecommendedCuratorsRoot)_list[1];

        foreach (Curator curator in mwsiveRecommendedCuratorsRoot.curators)
        {
            GameObject curatorInstance = GameObject.Instantiate(curatorPrefab, curatorScrollContent);
            curatorInstance.GetComponent<CuratorAppObject>().Initialize(curator.mwsive_user);
        }

    }
}
