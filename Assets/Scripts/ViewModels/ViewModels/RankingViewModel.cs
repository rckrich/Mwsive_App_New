using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingViewModel : ScrollViewModel
{
   public GameObject selectTime;

    public override void Initialize(params object[] list)
    {
        StartSearch();
        MwsiveConnectionManager.instance.GetRanking("", Callback_GetRanking);
    }

    private void Callback_GetRanking(object[] _list)
    {
        EndSearch();
    }

    public void OnClick_SelectTime()
    {
        selectTime.SetActive(true);
    }
}
