using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopCuratorsViewModel : ViewModel
{

    public GameObject curatorPrefab;
    public Transform curatorScrollContent;
    public int count;
    public ScrollRect scrollRect;

    private float end = -0.01f;

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
            curatorInstance.GetComponent<TopCuratorAppObject>().Initialize(curator.mwsive_user);
            count++;
        }

    }

    public void OnReachEnd()
    {
        if (scrollRect.verticalNormalizedPosition <= end)
        {
            MwsiveConnectionManager.instance.GetRecommendedCurators(Callback_GetRecommendedCurators, count, 20);
        }
    }

    public void OnClick_BackButton()
    {
        NewScreenManager.instance.ChangeToMainView(ViewID.ExploreViewModel);
    }
}
