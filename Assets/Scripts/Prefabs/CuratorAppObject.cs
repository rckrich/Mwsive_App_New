using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CuratorAppObject : AppObject
{
    public TextMeshProUGUI displayName;
    public Image profilePicture;

    public override void Initialize(params object[] list) { 
        SpotifyConnectionManager.instance.GetUserProfile((string)list[0], Callback_GetUserProfile);
    }

    private void Callback_GetUserProfile(object[] _value){
         if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        ProfileRoot profileRoot = (ProfileRoot)_value[1];
        displayName.text = profileRoot.display_name;
        ImageManager.instance.GetImage(profileRoot.images[0].url, profilePicture, (RectTransform)this.transform);

    }

    public void OnClick_CuratorAppObject(){
        //NewScreenManager.instance.ChangeToSpawnedView("curators");
    }
}
