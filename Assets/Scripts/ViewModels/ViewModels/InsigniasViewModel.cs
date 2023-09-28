using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsigniasViewModel : ViewModel
{
    private const int LIMIT = 20;
    // Start is called before the first frame update
    public ScrollRect scrollRect;
    public int offset = 0;

    [Header("Instance Referecnes")]
    public GameObject BadgeHolderPrefab;
    public Transform instanceParent;

    public override void Initialize(params object[] list)
    {
        StartSearch();
        MwsiveConnectionManager.instance.GetBadges(Callback_GetBadges, offset, LIMIT);
    }

    public void OnClick_SpawnPopUpButton()
    {
        //CallPopUP(PopUpViewModelTypes.MessageOnly, )
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
            BadgeHolder instance = GameObject.Instantiate(BadgeHolderPrefab, instanceParent).GetComponent<BadgeHolder>();
            instance.Initialize(badge);
            offset++;
        }
        EndSearch();  
    }
}
    


