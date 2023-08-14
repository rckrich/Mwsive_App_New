using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CuratorAppObject : AppObject
{
    public TextMeshProUGUI displayName;
    public Image profilePicture;

    private MwsiveUser mwsiveUser;

    public override void Initialize(params object[] list) {
        mwsiveUser = (MwsiveUser)list[0];
        displayName.text = mwsiveUser.display_name;
        if(mwsiveUser.image != null)
            ImageManager.instance.GetImage(mwsiveUser.image, profilePicture, (RectTransform)this.transform);
    }

    public void OnClick_CuratorAppObject(){
        //NewScreenManager.instance.ChangeToSpawnedView("curators");
    }
}
