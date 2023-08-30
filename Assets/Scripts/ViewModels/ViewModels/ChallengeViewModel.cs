using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeViewModel : ViewModel
{
    public GameObject challengePrefabR;
    public Transform challengeScrollContentR;
    public GameObject challengePrefabC;
    public Transform challengeScrollContentC;

    private int offsetResent;
    private int offsetComplete;

    private void Start()
    {
        GetChallenges();
    }

    public void GetChallenges()
    {
        MwsiveConnectionManager.instance.GetChallenges(Callback_GetChallenges, offsetResent, 20);
    }

    private void Callback_GetChallenges(object[] _value)
    {
        MwsiveChallengesRoot mwsiveChallengesRoot = (MwsiveChallengesRoot)_value[1];

        foreach (Challenges challenges in mwsiveChallengesRoot.challenges)
        { 
           GameObject challengeInstance = GameObject.Instantiate(challengePrefabR, challengeScrollContentR);
           ChallengeAppObject challengeAppObject = challengeInstance.GetComponent<ChallengeAppObject>();
           challengeAppObject.Initialize(challenges);   
        }
        offsetResent += 20;
    }

    public void OnClick_OnBackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
    }

    public void GetCompleteChallenges()
    {
        MwsiveConnectionManager.instance.GetCompleteChallenges(Callback_GetCompleteChallenges, offsetComplete, 20);
    }

    private void Callback_GetCompleteChallenges(object[] _value)
    {
        MwsiveChallengesRoot mwsiveChallengesRoot = (MwsiveChallengesRoot)_value[1];

        foreach (Challenges challenges in mwsiveChallengesRoot.challenges)
        {
            GameObject challengeInstance = GameObject.Instantiate(challengePrefabC, challengeScrollContentC);
            ChallengeAppObject challengeAppObject = challengeInstance.GetComponent<ChallengeAppObject>();
            challengeAppObject.Initialize(challenges);
        }
        offsetComplete += 20;
    }
}
