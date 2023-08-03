using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowersViewModel : ViewModel
{
    public ScrollRect scrollRect;
    public int offset = 1;

    [Header("Instance Referecnes")]
    public GameObject FollowersHolderPrefab;
    public Transform instanceParent;
    void Start()
    {
        MwsiveConnectionManager.instance.GetFollowers(Callback_GetFollowers, 0, 50);
    }
    private void Callback_GetFollowers(object[] _value)
    {
        MwsiveFollowersRoot mwsiveFollowers = (MwsiveFollowersRoot)_value[1];

        foreach(MwsiveUser user in mwsiveFollowers.followers)
        {
            MwisiveFollowerHolder instance = GameObject.Instantiate(FollowersHolderPrefab, instanceParent).GetComponent<MwisiveFollowerHolder>();
            instance.Initialize();
            
        }

        offset += 50;
    }

   public void GetFollwers()
    {
        MwsiveConnectionManager.instance.GetFollowers(Callback_GetFollowers, offset, 50);

    }
}
