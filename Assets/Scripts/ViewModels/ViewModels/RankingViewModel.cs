using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RankingViewModel : ScrollViewModel
{
    private const int PODIUM_NUMBER = 3;
    private const int LIMIT = 20;
    private const float end = -0.01f;

    public GameObject selectTimePanel;
    public TextMeshProUGUI timeTypeText;
    public GameObject shimmer;
    public List<TextMeshProUGUI> profileName;
    public List<Image> profileImage;
    public List<string> idList;
    public TextMeshProUGUI userlastestRank;
    public RectTransform Scroll;
    public ScrollRect scrollRect;
    [Header("References")]
    public Transform rankingContent;
    public GameObject rankingHolder;

    private string timeType = "AllTime";
    private int onlyone = 0;
    private int offset = 0;

    public override void Initialize(params object[] list)
    {
        offset = 0;
        onlyone = 0;
        shimmer.SetActive(true);
        //ChangeTimeType(timeType);
        /*if(AppManager.instance.currentMwsiveUser.latest_ranking != null)
            userlastestRank.text = AppManager.instance.currentMwsiveUser.latest_ranking.id.ToString();*/
        MwsiveConnectionManager.instance.GetCurrentMwsiveUser(Callback_GetCurrentMwsiveUser);
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

    private void Callback_GetCurrentMwsiveUser(object[] _value)
    {
        
        MwsiveUserRoot mwsiveUserRoot = (MwsiveUserRoot)_value[1];
        if(mwsiveUserRoot.user.latest_ranking != null)
            userlastestRank.text = mwsiveUserRoot.user.latest_ranking.id.ToString();
        ChangeTimeType(timeType);
    }

    private void Callback_GetRanking(object[] _list)
    {
        MwsiveRankingRoot mwsiveRankingRoot = (MwsiveRankingRoot)_list[1];

        //InstanceObjects<MwsiveUser>(MmsiveRankingRoot.users);

        //TODO: adjust ranking to use mwsive ranking id in GetRanking return call

       if(mwsiveRankingRoot.ranking.Count != 0)
        {
            Debug.Log(mwsiveRankingRoot.ranking.Count);
            if (mwsiveRankingRoot.ranking.Count > 3)
            {
                for (int i = 0; i < PODIUM_NUMBER; i++)
                {
                    profileName[i].text = mwsiveRankingRoot.ranking[i].mwsive_user.display_name;
                    idList[i] = mwsiveRankingRoot.ranking[i].mwsive_user.platform_id;                    
                    if (mwsiveRankingRoot.ranking[i].mwsive_user.image_url != null)
                        ImageManager.instance.GetImage(mwsiveRankingRoot.ranking[i].mwsive_user.image_url, profileImage[i], (RectTransform)this.transform);
                }

                for(int i = PODIUM_NUMBER; i < mwsiveRankingRoot.ranking.Count; i++)
                {
                    RankingHolder instance = GameObject.Instantiate(rankingHolder, rankingContent).GetComponent<RankingHolder>();
                    instance.Initialize(mwsiveRankingRoot.ranking[i].mwsive_user, mwsiveRankingRoot.ranking[i].id);
                }
                offset += 21;
            }
            else
            {
                for (int i = 0; i < mwsiveRankingRoot.ranking.Count; i++)
                {
                    profileName[i].text = mwsiveRankingRoot.ranking[i].mwsive_user.display_name;
                    idList[i] = mwsiveRankingRoot.ranking[i].mwsive_user.platform_id;
                    if (mwsiveRankingRoot.ranking[i].mwsive_user.image_url != null)
                        ImageManager.instance.GetImage(mwsiveRankingRoot.ranking[i].mwsive_user.image_url, profileImage[i], (RectTransform)this.transform);
                    offset++;
                }

            }
        } 
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
                MwsiveConnectionManager.instance.GetRanking("AllTime", Callback_GetRanking, offset, LIMIT);
                break;
            case "PastMonth":
                timeTypeText.text = "Mes pasado";
                ClearScrolls(rankingContent);
                MwsiveConnectionManager.instance.GetRanking("PastMonth", Callback_GetRanking, offset, LIMIT);
                break;
            case "PastWeek":
                timeTypeText.text = "Semana pasada";
                ClearScrolls(rankingContent);
                MwsiveConnectionManager.instance.GetRanking("PastWeek", Callback_GetRanking, offset, LIMIT);
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


    public void OnReachEnd()
    {
        
        if (onlyone == 0)
        {
            if (scrollRect.verticalNormalizedPosition <= end)
            {
                MwsiveConnectionManager.instance.GetRanking("AllTime", Callback_GetRanking, offset, LIMIT);
            }
        }
    }
}
