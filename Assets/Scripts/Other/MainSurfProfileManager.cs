using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSurfProfileManager : MonoBehaviour
{
    private static MainSurfProfileManager _instance;
    public static MainSurfProfileManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MainSurfProfileManager>();
            }

            return _instance;
        }
    }

    [SerializeField]
    private Image profileImage;

    public Image GetProfileImage()
    {
        return profileImage;
    }

}
