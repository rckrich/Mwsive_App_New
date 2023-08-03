using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsigniasViewModel : ViewModel
{
    // Start is called before the first frame update
    public ScrollRect scrollRect;
    public int offset = 1;

    [Header("Instance Referecnes")]
    public GameObject FollowersHolderPrefab;
    public Transform instanceParent;
    void Start()
    {
        MwsiveConnectionManager.instance.GetBadges(Callback_GetBadges);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick_SpawnPopUpButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("popUp");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }
    public void OnClick_BackButton()
    {
        NewScreenManager.instance.BackToPreviousView();
    }

    private void Callback_GetBadges(object[] _value)
    {
        MwsiveBadgesRoot badgesRoot = (MwsiveBadgesRoot)_value[1];

        foreach (Badge badge in badgesRoot.badges)
        {
            BadgeHolder instance = GameObject.Instantiate(FollowersHolderPrefab, instanceParent).GetComponent<BadgeHolder>();
            instance.Initialize();

        }

        offset += 50;
    }
}
    


