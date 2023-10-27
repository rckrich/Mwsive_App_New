using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MwisiveFollowerHolder : ViewModel
{
    public TextMeshProUGUI nameTxT;
    public Image curatorPicture;
    public string profileID;
    public void Initialize(string _name, string _profileID)
    {
        if (_name != null)
        {
            if (_name.Length > 27)
            {
                string _text2 = "";
                for (int i = 0; i < 27; i++)
                {
                    _text2 = _text2 + _name[i];
                }
                _text2 = _text2 + "...";
                nameTxT.text = _text2;
            }
            else
            {
                nameTxT.text = _name;
            }
        }

        profileID = _profileID;
    }

    public void Initialize(string _name, string _profileID, string _pictureURL)
    {
        if (_name != null)
        {
            if (_name.Length > 27)
            {
                string _text2 = "";
                for (int i = 0; i < 27; i++)
                {
                    _text2 = _text2 + _name[i];
                }
                _text2 = _text2 + "...";
                nameTxT.text = _text2;
            }
            else
            {
                nameTxT.text = _name;
            }
        }

        profileID = _profileID;
        if (_pictureURL != null)
            ImageManager.instance.GetImage(_pictureURL, curatorPicture, (RectTransform)this.transform);
    }

    public void OnClick_profile()
    {
        NewScreenManager.instance.ChangeToSpawnedView("profile");
        NewScreenManager.instance.GetCurrentView().GetComponent<ProfileViewModel>().Initialize(profileID);
    }
}
