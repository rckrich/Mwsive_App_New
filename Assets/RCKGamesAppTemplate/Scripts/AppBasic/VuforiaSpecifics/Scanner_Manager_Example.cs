/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using AlmostEngine.Screenshot;

public class Scanner_Manager_Example : Singleton<Scanner_Manager_Example>
{
    [Header("** Camera **")]
    public VuforiaBehaviour cameraAR;
    public GameObject spriteScanner;
    [Header("** Disbale Camera & Scanner **")]
    public GameObject screenScannerDisabled;
    [Header("** Actividad Sabias Que **")]
    public GameObject screen_PopUp_SabiasQue;
    public GameObject screen_Bravo_SabiasQue;
    [Header("** Actividad Foto **")]
    public GameObject screen_Actividad_Foto;
    public GameObject menuUp;
    public GameObject menuDown;
    public GameObject screen_SelectSticker;
    bool isTorchLightOn = false;
    [Header("** Cuadro Send Message **")]
    public GameObject screen_Cuadro_Contacto;




    private void Start()
    {
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_NORMAL);
    }

    private void OnApplicationPause(bool pause)
    {
        //Debug.Log("OnApplicationPAUSE  estatus: " + pause);
        if (pause)
        {
            Btn_ClosePhotoActivity();
            Btn_Disable_VuforiaBehaviour();
            screen_PopUp_SabiasQue.SetActive(false);
            screen_Actividad_Foto.SetActive(false);
            screen_Cuadro_Contacto.SetActive(false);
        }
    }

    public void Btn_GoToMainScene()
    {
        Scene_Manager.Instance.Btn_SceneToLoad_IntValue(1);
        Audio_Manager.Instance.PlaySFX(0);
    }

    public void Btn_Disable_VuforiaBehaviour()
    {
       // Audio_Manager.Instance.PlaySFX(0);
        cameraAR.enabled = false;
        screenScannerDisabled.SetActive(true);
    }

    public void Btn_Enable_VuforiaBehaviour()
    {
        Audio_Manager.Instance.PlaySFX(0);
        cameraAR.enabled = true;
    }

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

    public void Btn_CameraFocus()
    {
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO);
        spriteScanner.GetComponent<Animator>().SetTrigger("Activate");
    }


    //==========================================
    //========== ACTIVIDAD SABIAS QUE ==========
    //==========================================
    public void Btn_OpenPopUp_SabiasQue()
    {
        screen_PopUp_SabiasQue.SetActive(true);
    }

    public void Btn_ClosePopUp_SabiasQue()
    {
        Audio_Manager.Instance.PlaySFX(0);
        screen_PopUp_SabiasQue.SetActive(false);
    }

    public void Btn_CloseScreenBravo_SabiasQue()
    {
        Audio_Manager.Instance.PlaySFX(0);
        screen_Bravo_SabiasQue.SetActive(false);
    }

    //==========================================
    //============= ACTIVIDAD FOTO =============
    //==========================================

    public void Btn_OpenPhotoActivity()
    {
        Audio_Manager.Instance.PlaySFX(0);
        screen_Actividad_Foto.SetActive(true);
        menuUp.SetActive(false);
        menuDown.SetActive(false);
    }

    public void Btn_ClosePhotoActivity()
    {
        Audio_Manager.Instance.PlaySFX(0);
        screen_Actividad_Foto.SetActive(false);
        menuUp.SetActive(true);
        menuDown.SetActive(true);
    }

    public void Btn_OpenSelectSticker()
    {
        Audio_Manager.Instance.PlaySFX(0);
        screen_SelectSticker.SetActive(true);
    }

    public void Btn_CloseSelectSticker()
    {
        Audio_Manager.Instance.PlaySFX(0);
        screen_SelectSticker.SetActive(false);
    }

    public void Btn_CameraShot_SFX()
    {
        Audio_Manager.Instance.PlaySFX(1);
    }

    public void Btn_SFXSoundOnly()
    {
        Audio_Manager.Instance.PlaySFX(0);
    }

    //==========================================
    //====== ACTIVIDAD CUADRO CONTACTO =========
    //==========================================

    public void Btn_Open_CuadroContacto()
    {
        Audio_Manager.Instance.PlaySFX(0);
        screen_Cuadro_Contacto.SetActive(true);
    }

    public void Btn_Close_CuadroContacto()
    {
        Audio_Manager.Instance.PlaySFX(0);
        screen_Cuadro_Contacto.SetActive(false);
    }





}
*/
