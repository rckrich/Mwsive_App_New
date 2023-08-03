using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class FollowSelector : MonoBehaviour
{
    [SerializeField]
    public List<TMP_Text> opcion = new List<TMP_Text>();
    public List<GameObject> under = new List<GameObject>();
    public int numEnpantalla;
    public TMP_FontAsset font1;
    public TMP_FontAsset font2;
    public void SelectorFollow(int numero)
    {
        if (numero != numEnpantalla)
        {
            numEnpantalla = numero;

            switch (numero)
            {
               case 0:
                    opcion[0].font = font1;
                    opcion[1].font = font2;
                    under[0].SetActive(false);
                    under[1].SetActive(true);
                    break;
                case 1:
                    opcion[1].font = font1;
                    opcion[0].font = font2;
                    under[1].SetActive(false);
                    under[0].SetActive(true);
                    break;
            }
        }
    }
}
