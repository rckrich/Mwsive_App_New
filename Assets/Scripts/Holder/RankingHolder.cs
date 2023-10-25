using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingHolder : AppObject
{
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI rankNumber;
    public Image profilePicture;

    private string mwsiveID;
    private string spotifyID;
    private MwsiveUser mwsiveUser;
    private int mwsiveRank;

    public override void Initialize(params object[] list)
    {

        if(list[0] != null)
        {
            mwsiveUser = (MwsiveUser)list[0];
            mwsiveRank = (int)list[1];

            rankNumber.text = mwsiveRank.ToString();
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
                ImageManager.instance.GetImage(mwsiveUser.image_url, profilePicture, (RectTransform)this.transform, "PROFILEIMAGE");
        }
        else
        {
            mwsiveRank = (int)list[1];
            rankNumber.text = mwsiveRank.ToString();
            displayName.text = "Usuario no disponible";
            mwsiveID = "";
        }
    }

    public void OnClick_CuratorAppObject()
    {
        if(!mwsiveID.Equals(""))
        {
            NewScreenManager.instance.ChangeToSpawnedView("profile");
            NewScreenManager.instance.GetCurrentView().Initialize(spotifyID, mwsiveID);
        }
        else
        {
            UIMessage.instance.UIMessageInstanciate("Este perfil no está disponible");
        }
        
    }
}
