using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CuratorAppObject : AppObject
{
    public TextMeshProUGUI displayName;
    public Image profilePicture;

    private string mwsiveID;
    private string spotifyID;
    private MwsiveUser mwsiveUser;

    public override void Initialize(params object[] list) {
        mwsiveUser = (MwsiveUser)list[0];

        mwsiveID = mwsiveUser.id.ToString();
        spotifyID = mwsiveUser.platform_id.ToString();

        if (mwsiveUser.display_name.Length > 27)
        {
            string _text2 = "";
            for (int k = 0; k < 27; k++)
            {

                _text2 = _text2 + mwsiveUser.display_name[k];

            }
            _text2 = _text2 + "...";
            displayName.text = _text2;
        }
        else
        {
            displayName.text = mwsiveUser.display_name;
        }
        if (mwsiveUser.image_url != null)
            ImageManager.instance.GetImage(mwsiveUser.image_url, profilePicture, (RectTransform)this.transform);
    }

    public void OnClick_CuratorAppObject(){

        NewScreenManager.instance.ChangeToSpawnedView("profile");
        NewScreenManager.instance.GetCurrentView().Initialize(spotifyID, mwsiveID);
    }
}
