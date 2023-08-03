/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Scanner_Manager_Example_2 : SingletonRCK<Scanner_Manager_Example_2>
{
    [Header("** Camera **")]
    public VuforiaBehaviour cameraAR;
    public GameObject imageScanner;
    [Header("** Pause (Disbale Camera & Scanner) **")]
    public GameObject screenScannerDisabled;
    [Header("** Lampara **")]
    bool isTorchLightOn = false;


    private void Start()
    {
        SetFocusMode_ToNormal();
    }

    //===============================================================================
    // GENERALES
    //===============================================================================
    public void Btn_GoToMainScene()
    {
        Audio_Manager.Instance.PlaySFX(0);
        Scene_Manager.Instance.Btn_SceneToLoad_IntValue(1);
    }

    public void Btn_GoTo_Game1()
    {
        Audio_Manager.Instance.PlaySFX(0);
        Scene_Manager.Instance.Btn_SceneToLoad_IntValue(3);

        Progress_Manager.Instance.progress.game[0].isUnlocked = true;
        Progress_Manager.Instance.save();
    }

    //===============================================================================
    // PAUSA (Disbale Camera & Scanner)
    //===============================================================================
    private void OnApplicationPause(bool pause)
    {
        //Debug.Log("OnApplicationPAUSE  estatus: " + pause);
        if (pause)
        {
            Btn_Disable_VuforiaBehaviour();
        }
    }

    public void Btn_Disable_VuforiaBehaviour()
    {
        cameraAR.enabled = false;
        screenScannerDisabled.SetActive(true);
    }

    public void Btn_Enable_VuforiaBehaviour()
    {
        Audio_Manager.Instance.PlaySFX(0);
        cameraAR.enabled = true;
        screenScannerDisabled.SetActive(false);
    }

    //===============================================================================
    // LAMPARA
    //===============================================================================
    public void Btn_OnOffPhoneLight()
    {
        Audio_Manager.Instance.PlaySFX(0);

        if (!isTorchLightOn)
        {
            CameraDevice.Instance.SetFlashTorchMode(true);
            isTorchLightOn = true;
        }
        else
        {
            CameraDevice.Instance.SetFlashTorchMode(false);
            isTorchLightOn = false;
        }
    }

    //===============================================================================
    // FOCUS
    //===============================================================================
    public void Btn_CameraFocus()
    {
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO);
        imageScanner.GetComponent<Animator>().SetTrigger("Activate");
    }

    void SetFocusMode_ToNormal()
    {
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_NORMAL);
    }


}

*/

