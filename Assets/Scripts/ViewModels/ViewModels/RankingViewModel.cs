using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RankingViewModel : ScrollViewModel
{
    private const int PODIUM_NUMBER = 3;

    public GameObject selectTimePanel;
    public TextMeshProUGUI timeTypeText;
    public GameObject shimmer;
    public List<TextMeshProUGUI> profileName;
    public List<Image> profileImage;
    public List<string> idList;
    [Header ("References")]
    public Transform rankingContent;
    public GameObject rankingHolder;

    private string timeType = "AllTime";

    public override void Initialize(params object[] list)
    {
        shimmer.SetActive(true);
        ChangeTimeType(timeType);
        //TODO Get user rank
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
        MwsiveRankingRoot mwsiveRankingRoot = (MwsiveRankingRoot)_list[1];

        //InstanceObjects<MwsiveUser>(MmsiveRankingRoot.users);

        //TODO: adjust ranking to use mwsive ranking id in GetRanking return call

       /*if(mwsiveRankingRoot.users.Count != 0)
        {
            if (mwsiveRankingRoot.users.Count < 3)
            {
                for (int i = 0; i < PODIUM_NUMBER; i++)
                {
                    profileName[i].text = mwsiveRankingRoot.users[i].display_name;
                    idList[i] = mwsiveRankingRoot.users[i].platform_id;
                    if (mwsiveRankingRoot.users[i].image_url != null)
                        ImageManager.instance.GetImage(mwsiveRankingRoot.users[i].image_url, profileImage[i], (RectTransform)this.transform);
                }

                for(int i = PODIUM_NUMBER; i < mwsiveRankingRoot.users.Count; i++)
                {
                    CuratorAppObject instance = GameObject.Instantiate(rankingHolder, rankingContent).GetComponent<CuratorAppObject>();
                    instance.Initialize();
                }
            }
            else
            {
                for (int i = 0; i < mwsiveRankingRoot.users.Count; i++)
                {
                    profileName[i].text = mwsiveRankingRoot.users[i].display_name;
                    idList[i] = mwsiveRankingRoot.users[i].platform_id;
                    if (mwsiveRankingRoot.users[i].image_url != null)
                        ImageManager.instance.GetImage(mwsiveRankingRoot.users[i].image_url, profileImage[i], (RectTransform)this.transform);
                }

            }
        } */
        shimmer.SetActive(false);
    }

    public void ChangeTimeType(string _value)
    {
        timeType = _value;

        switch (timeType)
        {
            case "AllTime":
                timeTypeText.text = "Todo el tiempo";
                ClearScrolls(rankingContent);
                MwsiveConnectionManager.instance.GetRanking("AllTime", Callback_GetRanking);
                break;
            case "PastMonth":
                timeTypeText.text = "Mes pasado";
                ClearScrolls(rankingContent);
                MwsiveConnectionManager.instance.GetRanking("PastMonth", Callback_GetRanking);
                break;
            case "PastWeek":
                timeTypeText.text = "Semana pasada";
                ClearScrolls(rankingContent);
                MwsiveConnectionManager.instance.GetRanking("PastWeek", Callback_GetRanking);
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

    private void ClearScrolls(Transform _scrolls)
    {
        for (int i = 1; i < _scrolls.childCount; i++)
        {
            Destroy(_scrolls.GetChild(i).gameObject);
        }

    }

    public void OnClick_OpenProfile(int value)
    {
        switch (value)
        {
            case 1:
                NewScreenManager.instance.ChangeToSpawnedView("profile");
                NewScreenManager.instance.GetCurrentView().Initialize(idList[0]);
                break;
            case 2:
                NewScreenManager.instance.ChangeToSpawnedView("profile");
                NewScreenManager.instance.GetCurrentView().Initialize(idList[1]);
                break;
            case 3:
                NewScreenManager.instance.ChangeToSpawnedView("profile");
                NewScreenManager.instance.GetCurrentView().Initialize(idList[2]);
                break;
        }
            

    }

    public void OnClick_ShareRank()
    {
        //ToDo Share Ranking
    }
}
