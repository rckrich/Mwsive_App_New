using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMPro.TMP_InputField))]
public class InputFieldCustomCharacterValidationUserNameProfile : MonoBehaviour
{
    public  char Validate(ref string text, ref int pos, char ch)
    {
        
        if(char.IsLetterOrDigit(ch) || ch.Equals(" "))
        {
            pos++;

            return ch;
        }
        else
        {
            return '\0';
        }
        throw new NotImplementedException();
    }
}
