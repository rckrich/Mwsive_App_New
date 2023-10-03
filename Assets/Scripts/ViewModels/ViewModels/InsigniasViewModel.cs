using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsigniasViewModel : ViewModel
{
    private const int LIMIT = 21;
    // Start is called before the first frame update
    public ScrollRect scrollRect;
    public int engamentOffset = 0;
    public int trackOffset = 0;
    public GameObject noBadgesObject;

    [Header("Instance Referecnes")]
    public GameObject BadgeHolderPrefab;
    public Transform instanceParent;

    private string userid;
    private bool areTrackBadges = false;
    private bool areEngagementBadges = false;
    private float end = -0.01f;
    int onlyone = 0;

    public override void Initialize(params object[] list)
    {
#if PLATFORM_ANDROID
        SetAndroidBackAction();
#endif
        StartSearch();
        userid = list[0].ToString();
        if (userid.Equals(""))
            userid = AppManager.instance.currentMwsiveUser.platform_id;
        MwsiveConnectionManager.instance.GetBadges(userid, "engagement", Callback_GetBadgesEngagemend, engamentOffset, LIMIT);
    }

    public void OnClick_SpawnPopUpButton()
    {
        CallPopUP(PopUpViewModelTypes.MessageOnly, "Consigue mas insignias", "Sigue haciendo PIK para conseguir insignias", "Cerrar");
#if PLATFORM_ANDROID
        PopUpViewModel currentPopUp = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
        AppManager.instance.SetAndroidBackAction(() => {
            currentPopUp.ExitButtonOnClick();
            this.SetAndroidBackAction();
        });
#endif
    }

    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
        NewScreenManager.instance.GetCurrentView().SetAndroidBackAction();
    }

    private void Callback_GetBadgesEngagemend(object[] _value)
    {
        MwsiveBadgesRoot badgesRoot = (MwsiveBadgesRoot)_value[1];
        if (badgesRoot != null)
        {
            foreach (Badge badge in badgesRoot.badges)
            {
                BadgeHolder instance = GameObject.Instantiate(BadgeHolderPrefab, instanceParent).GetComponent<BadgeHolder>();
                instance.Initialize(badge);
                engamentOffset++;
            }
            areEngagementBadges = true;
        }
        if (engamentOffset == 0)
        {
            areEngagementBadges = false;
        }
        MwsiveConnectionManager.instance.GetBadges(userid, "track", Callback_GetBadgesTrack, trackOffset, LIMIT);
    }

    private void Callback_GetBadgesTrack(object[] _value)
    {
        MwsiveBadgesRoot badgesRoot = (MwsiveBadgesRoot)_value[1];
        if (badgesRoot != null)
        {
            foreach (Badge badge in badgesRoot.badges)
            {
                BadgeHolder instance = GameObject.Instantiate(BadgeHolderPrefab, instanceParent).GetComponent<BadgeHolder>();
                instance.Initialize(badge);
                trackOffset++;
            }
            areTrackBadges = true;
        }
        if (trackOffset == 0)
        {
            areTrackBadges = false;
        }
        TurnOn_NoBadges();
        EndSearch();
    }

    private void TurnOn_NoBadges()
    {

        if (!areEngagementBadges && !areTrackBadges)
        {
            noBadgesObject.SetActive(true);
        }
        else
        {
            noBadgesObject.SetActive(false);
        }


    }

    public override void SetAndroidBackAction()
    {
#if PLATFORM_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            AppManager.instance.SetAndroidBackAction(() => {
                if (finishedLoading)
                {
                    OnClick_BackButton();
                }
            });
        }
# endif
    }

    public void OnReachEnd()
    {
          if (onlyone == 0)
            {
                if (scrollRect.verticalNormalizedPosition <= end)
                {
                     MwsiveConnectionManager.instance.GetBadges(userid, "track", Callback_GetBadgesTrack, trackOffset, LIMIT);
                     onlyone = 1;
                }
            }
      

    }
}




