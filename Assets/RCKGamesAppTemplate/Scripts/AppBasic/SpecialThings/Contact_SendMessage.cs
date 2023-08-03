using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Contact_SendMessage : MonoBehaviour
{
    public InputField inputField;

    string mobile_num;
    string messageToSend;

    private void OnEnable()
    {
        //mobile_num = "";
        mobile_num = "+5219991255231";
        messageToSend = "Hola Adriana, \r\n\r\nMe gustaría hacerte una oferta sobre uno de los cuadros presentados en la Galería Aumentada del evento TEDx Mérida.\r\n\r\nPor favor contáctame.\r\n";

        inputField.text = messageToSend;
    }

    public void Btn_SenMessage()
    {
        Audio_Manager.Instance.PlaySFX(0);

#if UNITY_ANDROID
            //Android SMS URL - doesn't require encoding for sms call to work
            string URL = string.Format("sms:{0}?body={1}",mobile_num,System.Uri.EscapeDataString(inputField.text));
            Application.OpenURL(URL);

#endif

#if UNITY_IOS
        //ios SMS URL - ios requires encoding for sms call to work
        //string URL = string.Format("sms:{0}?&body={1}",mobile_num,WWW.EscapeURL(message)); //Method1 - Works but puts "+" for spaces
        //string URL ="sms:"+mobile_num+"?&body="+WWW.EscapeURL(message); //Method2 - Works but puts "+" for spaces
        //string URL = string.Format("sms:{0}?&body={1}",mobile_num,System.Uri.EscapeDataString(message)); //Method3 - Works perfect
        string URL = "sms:" + mobile_num + "?&body=" + System.Uri.EscapeDataString(inputField.text); //Method4 - Works perfectly
        Application.OpenURL(URL);

#endif

        //Execute Text Message
        //Application.OpenURL(URL);
    }


}


