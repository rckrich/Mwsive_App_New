using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class DescubrirPaginas : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    
    public List<GameObject> scenes = new List<GameObject>();
    
    public int numEnpantalla;
    
    public List<GameObject> images = new List<GameObject>();
    public List<GameObject> ResultsText = new List<GameObject>();
    public List<RectTransform> RestPositions = new List<RectTransform>();
    public Sprite sprite;
    public Sprite sprite2;
    public Descubrir_ViewModel _Descubrir;
    public ScrollRect ScrollofImages;
    public RectTransform Content;

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
                    ChangeShowText(true);
                    SnapTo(RestPositions[0]);
                    break;
                case 1:
                    scenes[1].SetActive(true);
                    images[1].GetComponent<Image>().color = Color.white;
                    images[1].GetComponent<Image>().sprite = sprite2;
                    ChangeShowText(true);
                    SnapTo(RestPositions[1]);
                    break;
                case 2:
                    scenes[2].SetActive(true);
                    images[2].GetComponent<Image>().color = Color.white;
                    images[2].GetComponent<Image>().sprite = sprite2;
                    ChangeShowText(true);
                    SnapTo(RestPositions[2]);
                    break;
                case 3:
                    scenes[3].SetActive(true);
                    images[3].GetComponent<Image>().color = Color.white;
                    images[3].GetComponent<Image>().sprite = sprite2;
                    ChangeShowText(true);
                    SnapTo(RestPositions[3]);
                    break;
                case 4:
                    scenes[4].SetActive(true);
                    images[4].GetComponent<Image>().color = Color.white;
                    images[4].GetComponent<Image>().sprite = sprite2;
                    ChangeShowText(true);
                    SnapTo(RestPositions[4]);
                    break;
                case 5:
                    scenes[5].SetActive(true);
                    images[5].GetComponent<Image>().color = Color.white;
                    images[5].GetComponent<Image>().sprite = sprite2;
                    ChangeShowText(true);
                    SnapTo(RestPositions[5]);
                    break;
                case 6:
                    scenes[6].SetActive(true);
                    images[6].GetComponent<Image>().color = Color.white;
                    images[6].GetComponent<Image>().sprite = sprite2;
                    ChangeShowText(true);
                   
                    break;
            }
        } 
        _Descubrir.Searchbar.text = "";     
    }

    public void ChangeShowText(bool _val){
        ResultsText[numEnpantalla].SetActive(true);
        if (_val)
        {
            switch (numEnpantalla)
            {
                case 0:
                    break;
                case 1:
                    
                    ResultsText[numEnpantalla].GetComponentInChildren<TextMeshProUGUI>().text = "Encuentra tus curadores favoritos";
                    break;
                case 2:
                    ResultsText[numEnpantalla].GetComponentInChildren<TextMeshProUGUI>().text = "Encuentra a tus canciones favoritas";
                    break;
                case 3:
                    ResultsText[numEnpantalla].GetComponentInChildren<TextMeshProUGUI>().text = "Encuentra a tus artistas favoritos";
                    break;
                case 4:
                    ResultsText[numEnpantalla].GetComponentInChildren<TextMeshProUGUI>().text = "Encuentra a tus ?lbumes favoritos";
                    break;
                case 5:
                    ResultsText[numEnpantalla].GetComponentInChildren<TextMeshProUGUI>().text = "Encuentra a tus playlists favoritas";
                    break;
                case 6:
                    ResultsText[numEnpantalla].GetComponentInChildren<TextMeshProUGUI>().text = "Encuentra a tus g?neros favoritos";
                    break;
            }
        }
        else
        {
            switch (numEnpantalla)
            {
                case 0:
                    ResultsText[numEnpantalla].GetComponentInChildren<TextMeshProUGUI>().text = "Resultado de busqueda";
                    break;
                case 1:
                    ResultsText[numEnpantalla].GetComponentInChildren<TextMeshProUGUI>().text = "Resultado de busqueda curadores";
                    break;
                case 2:
                    ResultsText[numEnpantalla].GetComponentInChildren<TextMeshProUGUI>().text = "Resultado de busqueda canciones";
                    break;
                case 3:
                    ResultsText[numEnpantalla].GetComponentInChildren<TextMeshProUGUI>().text = "Resultado de busqueda artistas";
                    break;
                case 4:
                    ResultsText[numEnpantalla].GetComponentInChildren<TextMeshProUGUI>().text = "Resultado de busqueda ?lbumes";
                    break;
                case 5:
                    ResultsText[numEnpantalla].GetComponentInChildren<TextMeshProUGUI>().text = "Resultado de busqueda playlists";
                    break;
                case 6:
                    ResultsText[numEnpantalla].GetComponentInChildren<TextMeshProUGUI>().text = "Resultado de busqueda g?neros";
                    break;
            }
        }
        
    }

    public void HideAllText(){
        foreach (GameObject item in ResultsText)
        {
            item.SetActive(false);
        }
    }


    /* public void OnClick(int num){

       Content.transform.DOMoveX(-RestPositions[num].transform.position.x, .5f);
    }*/

    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        Vector2 vector2 = (Vector2)ScrollofImages.transform.InverseTransformPoint(Content.position)
                 - (Vector2)ScrollofImages.transform.InverseTransformPoint(target.position);
        vector2.y = 0;
        Content.anchoredPosition = vector2;
                 
        

        /*var pos = 1 - ((Content.rect.height / 2 - target.localPosition.y) / Content.rect.height);
        ScrollofImages.normalizedPosition = new Vector2(0, pos);*/
    }
}
