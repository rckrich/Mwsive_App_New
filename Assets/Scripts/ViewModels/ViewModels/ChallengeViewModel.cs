using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChallengeViewModel : ViewModel
{
    private const int ARRAY_WITH_NEWLOADINGSYSTEM = 1;
    private const int ITEM_LIMIT = 20;

    public GameObject challengePrefabR;
    public Transform challengeScrollContentR;
    public GameObject challengePrefabC;
    public Transform challengeScrollContentC;
    public GameObject ScrollResents;
    public GameObject ScrollComplete;
    public TextMeshProUGUI resentText;
    public TextMeshProUGUI completeText;
    public GameObject shimmer;
    public GameObject completados;

    private int offsetResent;
    private int offsetComplete;

    public override void Initialize(params object[] list)
    {
#if PLATFORM_ANDROID
        SetAndroidBackAction();
#endif

        ClearChallenges(challengeScrollContentR);
        GetChallenges();
    }

    public void GetChallenges()
    {
        shimmer.SetActive(true);
        MwsiveConnectionManager.instance.GetChallenges(Callback_GetChallenges, offsetResent, ITEM_LIMIT);
    }

    private void Callback_GetChallenges(object[] _value)
    {
        MwsiveChallengesRoot mwsiveChallengesRoot = (MwsiveChallengesRoot)_value[1];
        shimmer.SetActive(false);
        foreach (Challenges challenges in mwsiveChallengesRoot.challenges)
        { 
           GameObject challengeInstance = GameObject.Instantiate(challengePrefabR, challengeScrollContentR);
           ChallengeAppObject challengeAppObject = challengeInstance.GetComponent<ChallengeAppObject>();
           challengeAppObject.Initialize(challenges, false);   
        }
        offsetResent += ITEM_LIMIT;
        
    }

    public void OnClick_OnBackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
        NewScreenManager.instance.GetCurrentView().SetAndroidBackAction();
    }

    public void GetCompleteChallenges()
    {
        completados.SetActive(true);
        MwsiveConnectionManager.instance.GetCompleteChallenges(Callback_GetCompleteChallenges, offsetComplete, ITEM_LIMIT);
    }

    private void Callback_GetCompleteChallenges(object[] _value)
    {
        MwsiveChallengesRoot mwsiveChallengesRoot = (MwsiveChallengesRoot)_value[1];
        completados.SetActive(false);
        foreach (Challenges challenges in mwsiveChallengesRoot.challenges)
        {
            GameObject challengeInstance = GameObject.Instantiate(challengePrefabC, challengeScrollContentC);
            ChallengeAppObject challengeAppObject = challengeInstance.GetComponent<ChallengeAppObject>();
            challengeAppObject.Initialize(challenges, true);
        }
        offsetComplete += ITEM_LIMIT;
       
    }

    public void OnClick_Resents()
    {
        ClearChallenges(challengeScrollContentR);
        offsetResent = 0;
        GetChallenges();
        ScrollResents.SetActive(true);
        ScrollComplete.SetActive(false);
        resentText.color = Color.black;
        completeText.color = Color.gray;
    }

    public void OnClick_Complete()
    {
        ClearChallenges(challengeScrollContentC);
        offsetComplete = 0;
        GetCompleteChallenges();
        ScrollResents.SetActive(false);
        ScrollComplete.SetActive(true);
        resentText.color = Color.gray;
        completeText.color = Color.black;
    }

    private void ClearChallenges(Transform _content)
    {
        for(int i = ARRAY_WITH_NEWLOADINGSYSTEM; i < _content.childCount; i++)
        {
            Destroy(_content.GetChild(i).gameObject);
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
                    OnClick_OnBackButton();
                }
            });
        }
#endif
    }

}
