using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Contact_SendEmail : MonoBehaviour
{
    //public InputField inputField;

    string email;
    string subject;
    string body;


    private void OnEnable()
    {
        //email = "gruporckgames@gmail.com";
        email = "campamentodvmx@gmail.com";
        subject = MyEscapeURL("Cursos de Videojuegos");
        body = MyEscapeURL("Hola,\r\n\r\nMe gustaría obtener más información de los cursos de desarrollo de videojuegos. \r\n\r\nPor favor contáctame. \r\nMi teléfono: \r\nMi email: \r\nMi nombre: \r\n\r\n¡Bonito día!");

        //inputField.text = messageToSend;
    }

    public void Btn_SendEmail()
    {
        //Audio_Manager.Instance.PlaySFX(0);

#if UNITY_ANDROID
        //string URL = string.Format("sms:{0}?body={1}",mobile_num,System.Uri.EscapeDataString(inputField.text));
        string URL = "mailto:" + email + "?subject=" + subject + "&body=" + body;
        Application.OpenURL(URL);
#endif

#if UNITY_IOS

        //string URL = "sms:" + mobile_num + "?&body=" + System.Uri.EscapeDataString(inputField.text); //Method4 - Works perfectly
        string URL = "mailto:" + email + "?subject=" + subject + "&body=" + body;
        Application.OpenURL(URL);
#endif

        //Execute Text Message
        //Application.OpenURL(URL);

    }

    string MyEscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }
}




