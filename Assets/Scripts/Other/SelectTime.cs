using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTime : MonoBehaviour
{
    public void OnClick_AllTime()
    {
        NewScreenManager.instance.GetMainView(ViewID.RankingViewModel).GetComponent<RankingViewModel>().ChangeTimeType("AllTime");
    }

    public void OnClick_PastMonth()
    {
        NewScreenManager.instance.GetMainView(ViewID.RankingViewModel).GetComponent<RankingViewModel>().ChangeTimeType("PastMonth");
    }

    public void OnClick_PastWeek()
    {
        NewScreenManager.instance.GetMainView(ViewID.RankingViewModel).GetComponent<RankingViewModel>().ChangeTimeType("PastWeek");
    }
}
