using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back_Btn_AndroidSpecific_Manager : MonoBehaviour // RCK_Singleton<Back_Btn_AndroidSpecific_Manager>
{
    /// <summary>
    /// 0 => Exit App ||
    /// 1 => Return to Scene 01_Inicio ||
    /// 2 => Close PopUp GoToAdress ||
    /// 3 => Close Screen About ||
    /// 4 => Close Screen DetailedInfo ||
    /// 5 => Close Section Actividades ||
    /// 6 => Close Section Patrocinadores ||
    /// 7 => Close All Screens PatrocinadoresInfo
    /// </summary>
    [HideInInspector]
    public int setValue = 0;


    private void Update()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                switch (setValue)
                {
                    case 0:
                        Application.Quit();
                        return;
                    case 1:
                        //Speakers_Manager.Instance.Btn_ChangeScene_Inicio();
                        return;
                    case 2:
                        //Speakers_Manager.Instance.Btn_OpenClosePopUpGoToAdress();
                        return;
                    case 3:
                        //Speakers_Manager.Instance.Btn_OpenCloseScreen_About();
                        return;
                    case 4:
                        //Speakers_Manager.Instance.Btn_OpenCloseScreen_DetailedInfo();
                        return;
                    case 5:
                        //Menu_Manager.Instance.Btn_OpenSpeakerCanvas();
                        return;
                    case 6:
                        //Menu_Manager.Instance.Btn_OpenSpeakerCanvas();
                        return;
                    case 7:
                        //Patrocinadores_Manager.Instance.Btn_OpenMainScreenPatrocinadores();
                        return;
                    default:
                        //Speakers_Manager.Instance.Btn_ChangeScene_Inicio();
                        return;
                }
            }
        }
    }


}
