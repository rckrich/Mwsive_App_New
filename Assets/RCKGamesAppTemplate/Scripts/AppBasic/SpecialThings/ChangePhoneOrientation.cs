using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePhoneOrientation : MonoBehaviour
{

    public void Portrait_Orientation()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    public void Landscape_Orientation()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    public void LandscapeLeft_Orientation()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    public void LandscapeRight_Orientation()
    {
        Screen.orientation = ScreenOrientation.LandscapeRight;
    }

    public void PortraitUpsideDown_Orientation()
    {
        Screen.orientation = ScreenOrientation.PortraitUpsideDown;
    }

    public void AutoRotation_Orientation()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
    }



}
