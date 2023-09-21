using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UsersThatVotedViewModel : ViewModel
{
    private const int LIMIT_OF_CURATORS = 20;

    [Header("Curators Gameobject References")]
    public GameObject curatorPrefab;
    public Transform curatorScrollContent;

    private string trackId;
    private int offset;
    public ScrollRect scrollRect;
    public float end;
    int onlyone = 0;
    public RectTransform Scroll;

    public override void Initialize(params object[] list)
    {
        trackId = list[0].ToString();
        GetCuratorsThatVoted();
    }

    public void GetCuratorsThatVoted()
    {
        MwsiveConnectionManager.instance.GetCuratorsThatVoted(trackId, Callback_GetCuratorsThatVoted, offset, LIMIT_OF_CURATORS);
    }

    private void Callback_GetCuratorsThatVoted(object[] _value)
    {
        MwsiveCuratorsRoot mwsiveCuratorsRoot = (MwsiveCuratorsRoot)_value[1];

        foreach(MwsiveUser mwsiveUser in mwsiveCuratorsRoot.curators)
        {
            GameObject curatorInstance = GameObject.Instantiate(curatorPrefab, curatorScrollContent);
            curatorInstance.GetComponent<CuratorAppObject>().Initialize(mwsiveUser);
        }
        offset += 20;
    }

    public void OnReachEnd()
    {
        if (onlyone == 0)
        {
            if (scrollRect.verticalNormalizedPosition <= end)
            {
                GetCuratorsThatVoted();
                onlyone = 1;
            }
        }
    }

    public void OnClick_Backbutton()
    {
        NewScreenManager.instance.BackToPreviousView();
    }

}
