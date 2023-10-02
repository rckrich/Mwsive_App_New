using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingViewModel : ScrollViewModel
{
    public GameObject selectTimePanel;
    public TextMeshProUGUI timeTypeText;
    public SurfManager surfManager;

    private string timeType = "AllTime";


    public override void Initialize(params object[] list)
    {
        StartSearch();
        ChangeTimeType(timeType);
        MwsiveConnectionManager.instance.GetRanking(timeType, Callback_GetRanking);

#if PLATFORM_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            AppManager.instance.SetAndroidBackAction(() => {
                if (finishedLoading)
                {
                    GameObject.FindObjectOfType<MenuOptions>().OnClick(1);
                }
                AppManager.instance.SetAndroidBackAction(null);
            });
        }
#endif
    }

    private void Callback_GetRanking(object[] _list)
    {
        MwsiveRankingRoot MmsiveRankingRoot = (MwsiveRankingRoot)_list[1];

        //InstanceObjects<MwsiveUser>(MmsiveRankingRoot.users);

        EndSearch();
    }

    public void ChangeTimeType(string _value)
    {
        timeType = _value;

        switch (timeType)
        {
            case "AllTime":
                timeTypeText.text = "Todo el tiempo";
                break;
            case "PastMonth":
                timeTypeText.text = "Mes pasado";
                break;
            case "PastWeek":
                timeTypeText.text = "Semana pasada";
                break;
        }
    }

    public void OnClick_SelectTime()
    {
        selectTimePanel.SetActive(true);
    }

    public override void SetAndroidBackAction()
    {
#if PLATFORM_ANDROID
        AppManager.instance.ResetAndroidBackAction();
#endif
    }
}
