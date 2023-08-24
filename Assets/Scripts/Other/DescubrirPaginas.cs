using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DescubrirPaginas : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    
    public List<GameObject> scenes = new List<GameObject>();
    
    public int numEnpantalla;
    
    public List<GameObject> images = new List<GameObject>();
    public List<GameObject> ResultsText = new List<GameObject>();
    
    public Sprite sprite;
    public Sprite sprite2;
    public Descubrir_ViewModel _Descubrir;
    public ScrollRect ScrollofImages;

    public void ChangeWindow()
    {

    }
    public void HideEscena(){

        scenes[numEnpantalla].SetActive(false);
        EleccionDeEscena(0);
        ScrollofImages.horizontalNormalizedPosition = 0;
    }

    public int GetCurrentEscena(){
        return numEnpantalla;
    }

    public void EleccionDeEscena(int numero)
    {
        if(numero != numEnpantalla)
        {
            _Descubrir.KillPrefablist(numEnpantalla);
            numEnpantalla = numero;
            for(int i = 0; i < 7; i++)
            {
                images[i].GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f);
                images[i].GetComponent<Image>().sprite = sprite;
                scenes[i].SetActive(false);
                ResultsText[i].SetActive(false);
            }
            switch (numero)
            {
                case 0:
                    scenes[0].SetActive(true);
                    images[0].GetComponent<Image>().color = Color.white;
                    images[0].GetComponent<Image>().sprite = sprite2;
                    break;
                case 1:
                    scenes[1].SetActive(true);
                    images[1].GetComponent<Image>().color = Color.white;
                    images[1].GetComponent<Image>().sprite = sprite2;
                    break;
                case 2:
                    scenes[2].SetActive(true);
                    images[2].GetComponent<Image>().color = Color.white;
                    images[2].GetComponent<Image>().sprite = sprite2;
                    break;
                case 3:
                    scenes[3].SetActive(true);
                    images[3].GetComponent<Image>().color = Color.white;
                    images[3].GetComponent<Image>().sprite = sprite2;
                    break;
                case 4:
                    scenes[4].SetActive(true);
                    images[4].GetComponent<Image>().color = Color.white;
                    images[4].GetComponent<Image>().sprite = sprite2;
                    break;
                case 5:
                    scenes[5].SetActive(true);
                    images[5].GetComponent<Image>().color = Color.white;
                    images[5].GetComponent<Image>().sprite = sprite2;
                    break;
                case 6:
                    scenes[6].SetActive(true);
                    images[6].GetComponent<Image>().color = Color.white;
                    images[6].GetComponent<Image>().sprite = sprite2;
                    break;
            }
        } 
        _Descubrir.Searchbar.text = "";     
    }

    public void HideShowText(bool val){
       
        ResultsText[numEnpantalla].SetActive(val);
    }

    public void HideAllText(){
        foreach (GameObject item in ResultsText)
        {
            item.SetActive(false);
        }
    }

    

}
