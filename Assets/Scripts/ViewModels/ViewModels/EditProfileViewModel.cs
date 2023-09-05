using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditProfileViewModel : ViewModel
{
 
    void Start()
    {
        
    }

    public void OnClick_BackButton()
    {
        NewScreenManager.instance.ChangeToMainView(ViewID.ProfileViewModel);
    }

    public void OnClick_EditPhoto()
    {
        Debug.Log("ppppppppppp");
        OpenGallery.instance.GetImageFromGallery();
    }
}
