using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowersViewModel : ViewModel
{
    
    public ScrollRect scrollRect;
    int onlyone = 0;
    public FollowSelector followSelector;
    public GameObject noFollowersText, noFollowText, noFollowContent;

    [Header("Instance Referecnes")]
    public GameObject FollowersHolderPrefab;
    public Transform instanceParent;
    public GameObject content;

    private float end = -0.01f;
    private const int FOLLOWERS_OPTION = 0;
    private const int FOLLOWED_OPTION = 1;
    private string profileID = "";
    private bool sceneActive;
    private int offset = 0;

    public override void Initialize(params object[] list)
    {
#if PLATFORM_ANDROID
        SetAndroidBackAction();
#endif
        noFollowContent.SetActive(false);
        noFollowersText.SetActive(false);
        noFollowText.SetActive(false);
    }

    private void Callback_GetFollowers(object[] _value)
    {
        if (profileID.Equals(""))
        {
            MwsiveFollowersRoot mwsiveFollowersRoot = (MwsiveFollowersRoot)_value[1];

            foreach(MwsiveUser user in mwsiveFollowersRoot.followers)
            {
                MwisiveFollowerHolder instance = GameObject.Instantiate(FollowersHolderPrefab, instanceParent).GetComponent<MwisiveFollowerHolder>();
                instance.Initialize(user.display_name, user.platform_id, user.image_url);
                offset++;
            }

            if(offset == 0)
            {
                noFollowContent.SetActive(true);
                noFollowersText.SetActive(true);
            }
        }
        else
        {
            MwsiveFollowersRoot mwsiveFollowers = (MwsiveFollowersRoot)_value[1];

            foreach (MwsiveUser user in mwsiveFollowers.followers)
            {
                MwisiveFollowerHolder instance = GameObject.Instantiate(FollowersHolderPrefab, instanceParent).GetComponent<MwisiveFollowerHolder>();
                instance.Initialize( user.display_name, user.platform_id, user.image_url);
                //instance.SetImage(user.image);
                offset++;
            }

            if (offset == 0)
            {
                noFollowContent.SetActive(true);
                noFollowersText.SetActive(true);
            }
        }
        

        
    }

   public void GetCurrentFollowers()
    {
        RemoveChild();
        followSelector.SelectorFollow(FOLLOWERS_OPTION);
        sceneActive = true;
        onlyone = 0;
        noFollowContent.SetActive(false);
        noFollowersText.SetActive(false);
        noFollowText.SetActive(false);
        if (profileID.Equals(""))
        {
            MwsiveConnectionManager.instance.GetFollowers(Callback_GetFollowers, 0, 50);
        }
        else
        {
            MwsiveConnectionManager.instance.GetUserFollowers(profileID, Callback_GetFollowers, 0, 50);
        }
        

    }
    public void GetFollowers(string _profileID)
    {
        RemoveChild();
        noFollowContent.SetActive(false);
        noFollowersText.SetActive(false);
        noFollowText.SetActive(false);
        followSelector.SelectorFollow(FOLLOWERS_OPTION);
        onlyone = 0;
        profileID = _profileID;
        MwsiveConnectionManager.instance.GetUserFollowers(_profileID, Callback_GetFollowers, 0, 50);
    }

    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
        NewScreenManager.instance.GetCurrentView().GetComponent<ProfileViewModel>().SetAndroidBackAction();
    }


    public void GetCurrentFollowed()
    {
        noFollowContent.SetActive(false);
        noFollowersText.SetActive(false);
        noFollowText.SetActive(false);
        RemoveChild();
        followSelector.SelectorFollow(FOLLOWED_OPTION);
        sceneActive = false;
        onlyone = 0;
        if (profileID.Equals(""))
        {
            MwsiveConnectionManager.instance.GetFollowed(Callback_GetFollowed, 0, 50);
        }
        else
        {
            MwsiveConnectionManager.instance.GetUserFollowed(profileID,Callback_GetFollowed, 0, 50);
        }
        
    }

    public void GetFollowed(string _profileID)
    {
        RemoveChild();
        noFollowContent.SetActive(false);
        noFollowersText.SetActive(false);
        noFollowText.SetActive(false);
        followSelector.SelectorFollow(FOLLOWED_OPTION);
        onlyone = 0;
        profileID = _profileID;
        MwsiveConnectionManager.instance.GetUserFollowed(_profileID, Callback_GetFollowed, 0, 50);

        
    }

    public void Callback_GetFollowed(object[] _value)
    {
        if (profileID.Equals(""))
        {
            MwsiveFollowedRoot mwsiveFollowedRoot = (MwsiveFollowedRoot)_value[1];

            foreach(MwsiveUser user in mwsiveFollowedRoot.followed)
            {
                MwisiveFollowerHolder instance = GameObject.Instantiate(FollowersHolderPrefab, instanceParent).GetComponent<MwisiveFollowerHolder>();
                instance.Initialize(user.display_name, user.platform_id, user.image_url);
                //instance.SetImage(user.image);
                offset++;
            }

            if (offset == 0)
            {
                noFollowContent.SetActive(true);
                noFollowText.SetActive(true);
            }
        }
        else
        {
            MwsiveFollowedRoot mwsiveFollowed = (MwsiveFollowedRoot)_value[1];

            foreach (MwsiveUser user in mwsiveFollowed.followed)
            {
                MwisiveFollowerHolder instance = GameObject.Instantiate(FollowersHolderPrefab, instanceParent).GetComponent<MwisiveFollowerHolder>();
                instance.Initialize(user.display_name, user.platform_id, user.image_url);
                //instance.SetImage(user.image);
                offset++;
            }

            if (offset == 0)
            {
                noFollowContent.SetActive(true);
                noFollowText.SetActive(true);
            }
        }
        
    }

    public void OnReachEnd()
    {
        if (sceneActive)
        {
            if (profileID.Equals(""))
            {
                if (onlyone == 0)
                {
                    if (scrollRect.verticalNormalizedPosition <= end)
                    {
                        MwsiveConnectionManager.instance.GetFollowers(Callback_GetFollowers, offset, 20);
                        onlyone = 1;
                    }
                }
            }
            else
            {
                if (onlyone == 0)
                {
                    if (scrollRect.verticalNormalizedPosition <= end)
                    {
                        MwsiveConnectionManager.instance.GetUserFollowers(profileID, Callback_GetFollowers, offset, 20);
                        onlyone = 1;
                    }
                }
            }
        }
        else
        {
            if (profileID.Equals(""))
            {
                if (onlyone == 0)
                {
                    if (scrollRect.verticalNormalizedPosition <= end)
                    {
                        MwsiveConnectionManager.instance.GetFollowed(Callback_GetFollowed, 20, offset);
                        onlyone = 1;
                    }
                }
            }
            else
            {
                if (onlyone == 0)
                {
                    if (scrollRect.verticalNormalizedPosition <= end)
                    {
                        MwsiveConnectionManager.instance.GetUserFollowed(profileID, Callback_GetFollowed, 20, offset);
                        onlyone = 1;
                    }
                }

            }
        }
        
        
    }

    public void RemoveChild()
    {
        if (content.transform.childCount != 1)
        {
            for(int i = 1; i < content.transform.childCount; i++)
            {
                Destroy(content.transform.GetChild(i).gameObject);
            }
        }
    }

    public void ProfileIDReset_GetFollowers()
    {
        profileID = "";
        GetCurrentFollowers();
        
    }

    public void ProfileIDReset_GetFollowed()
    {
        profileID = "";
        GetCurrentFollowed();
    }

    public void SetAndroidBAckAction()
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
#endif
    }
}
