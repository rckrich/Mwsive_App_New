using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsigniasViewModel : ViewModel
{
    private const int LIMIT = 21;
    // Start is called before the first frame update
    public ScrollRect scrollRect;
    public int offset = 0;
    public GameObject noBadgesObject;

    [Header("Instance Referecnes")]
    public GameObject BadgeHolderPrefab;
    public Transform instanceParent;

    private string userid;
    private bool areTrackBadges = false;
    private bool areEngagementBadges = false;

    public override void Initialize(params object[] list)
    {
#if PLATFORM_ANDROID
        SetAndroidBackAction();
#endif
        StartSearch();
        userid = list[0].ToString();
        if (userid.Equals(""))
            userid = AppManager.instance.currentMwsiveUser.platform_id;
        MwsiveConnectionManager.instance.GetBadges(userid, "engagement", Callback_GetBadgesEngagemend, offset, LIMIT);
    }

    public void OnClick_SpawnPopUpButton()
    {
        CallPopUP(PopUpViewModelTypes.MessageOnly, "Consigue mas insignias", "Sigue haciendo PIK para conseguir insignias", "Cerrar");
    }

    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
        NewScreenManager.instance.GetCurrentView().GetComponent<ProfileViewModel>().SetAndroidBackAction();
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
                offset++;
            }
            areEngagementBadges = true;
        }
        if (offset == 0)
        {
            areEngagementBadges = false;
        }
        MwsiveConnectionManager.instance.GetBadges(userid, "track", Callback_GetBadgesTrack, offset, LIMIT);
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
                offset++;
            }
            areTrackBadges = true;
        }
        if (offset == 0)
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

    public void SetAndroidBackAction()
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
}




