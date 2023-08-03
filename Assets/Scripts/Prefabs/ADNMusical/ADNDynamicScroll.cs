using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ADNDynamicScroll : MonoBehaviour
{
    
    public float MaxPrefabsInScreen = 0;
    public ScrollRect ScrollBar;
    public GameObject SpawnArea, Prefab, Añadir, ScrollView, GuardarTop, HeaderGrafico;
    private GameObject Instance;  
    public List<GameObject> Instances = new List<GameObject>(); 
    private static ADNDynamicScroll _instance;
    public List<string> DataPlaceHolders = new List<string>(); 
    public List<string> DataSpotifyID = new List<string>(); 

    
    public enum TypeOfADN {OnRepeat, Artistaquereviviria, GustoCulposo, DeAmor, UltimoDescubrimiento, GOAT, ProximaEstrella, SoundtrackDeMiVida};
    public TypeOfADN ADN;
    public TextMeshProUGUI Title, Subtitle; 
    public List<GameObject> Prefabs = new List<GameObject>(); 
    private string PlaceHolderText;
    private GameObject PrefabToSet;
    public int Type, Max, Min, CurrentPrefabs;
    private float ScrollPosition = -0.3f;
    private bool IsAllPlacesComplete = true;


    // Start is called before the first frame update

    // Update is called once per frame
    private void OnEnable() {
        switch(ADN){
            case TypeOfADN.OnRepeat:
                Title.text = "ON REPEAT";
                Subtitle.text = "Escribe tus cancion favorita";
                PlaceHolderText = "Buscar Canción";
                PrefabToSet = Prefabs[1];
                Max = 1;
                Min = 1;
                Type = 1;
                GuardarTop.GetComponent<Image>().color = new Color32 (255,255,255,255);
                Añadir.SetActive(false);
                break;
            case TypeOfADN.Artistaquereviviria:
                Title.text = "ARTISTA QUE REVIVIRIA";
                Subtitle.text = "Escribe tus artistas favoritos";
                PlaceHolderText = "Buscar Artista";
                PrefabToSet = Prefabs[0];
                Max = 5;
                Min = 1;
                Type = 0;
                GuardarTop.GetComponent<Image>().color = new Color32 (255,255,255,255);
                break;
            case TypeOfADN.GustoCulposo:
                Title.text = "GUSTO CULPOSO";
                Subtitle.text = "Escribe tus canciones favoritas";
                PlaceHolderText = "Buscar Canción";
                PrefabToSet = Prefabs[1];
                Max = 5;
                Min = 1;
                Type = 1;
                GuardarTop.GetComponent<Image>().color = new Color32 (255,255,255,255);
                break;
            case TypeOfADN.DeAmor:
                Title.text = "DE AMOR";
                Subtitle.text = "Escribe tus canciones favoritas";
                PlaceHolderText = "Buscar Canción";
                PrefabToSet = Prefabs[1];
                Max = 5;
                Min = 1;
                Type = 1;
                GuardarTop.GetComponent<Image>().color = new Color32 (255,255,255,255);
                break;
            case TypeOfADN.UltimoDescubrimiento:
                Title.text = "ULTIMO DESCUBRIMIENTO";
                Subtitle.text = "Escribe tus canciones favoritas";
                PlaceHolderText = "Buscar Canción";
                Max = 5;
                Min = 1;
                PrefabToSet = Prefabs[1];
                Type = 1;
                GuardarTop.GetComponent<Image>().color = new Color32 (255,255,255,255);
                break;
            case TypeOfADN.GOAT:
                Title.text = "GOAT";
                Subtitle.text = "Escribe tus artista favorito";
                PlaceHolderText = "Buscar Artista";
                PrefabToSet = Prefabs[0];
                Max = 1;
                Min = 1;
                Type = 0;
                Añadir.SetActive(false);
                GuardarTop.GetComponent<Image>().color = new Color32 (255,255,255,255);
                break;
            case TypeOfADN.ProximaEstrella:
                Title.text = "PROXIMA ESTRELLA";
                Subtitle.text = "Escribe tus artistas favoritos";
                PlaceHolderText = "Buscar Artista";
                PrefabToSet = Prefabs[0];
                Max = 5;
                Min = 1;
                Type = 0;
                GuardarTop.GetComponent<Image>().color = new Color32 (255,255,255,255);
                break;
            case TypeOfADN.SoundtrackDeMiVida:
                Title.text = "EL SOUNDTRACK DE MI VIDA";
                Subtitle.text = "Escribe tus canciones favoritas";
                PlaceHolderText = "Buscar Canción";
                PrefabToSet = Prefabs[1];
                Max = 18;
                Min = 4;
                Type = 1;
                
                GuardarTop.GetComponent<Button>().enabled = false;
                break;
        }

        
        if(DataPlaceHolders != null){
            if(DataPlaceHolders.Count >= Min){
                for (int i = 0; i < DataPlaceHolders.Count; i++)
                {
                    DynamicPrefabSpawner(0);
                    if(DataPlaceHolders[i] != null || DataPlaceHolders[i] != ""){
                        Instances[i].GetComponent<PF_ADNMusicalEventSystem>().SetPlaceHolder(DataPlaceHolders[i]);
                    }
                    
                }
            }else{
            DynamicPrefabSpawner(Min-1);
            }
            
        }
        CurrentPrefabs = Min;
        GuardarTop.GetComponent<Button>().enabled = false;
        GuardarTop.GetComponent<Image>().color = new Color32 (128,128,128,255);
    }

    public void HideShowHeader(bool value){
        HeaderGrafico.SetActive(value);
    }
    
    public void HideAllOtherInstances(string gameObjectname){
        Debug.Log("Hide");
        foreach (GameObject item in Instances)
        {
            if(item.name != gameObjectname){
                item.SetActive(false);
            }
            
        }
        Añadir.SetActive(false);
    }

    public static ADNDynamicScroll instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ADNDynamicScroll>();
            }
            return _instance;
        }
    }
    
    public void DestroyInstance(GameObject target){
        Instances.Remove(target);
        Destroy(target);
        CurrentPrefabs--;
        int i = 1;
        foreach (GameObject item in Instances)
        {
            item.GetComponent<PF_ADNMusicalEventSystem>().ChangeName(i, Min);
            item.name =  Prefab.name  + "-"+ i;
            i++;

            if(item.GetComponent<PF_ADNMusicalEventSystem>().GetPlaceHolder() != PlaceHolderText){
                GuardarTop.GetComponent<Image>().color = new Color32 (255,255,255,255);
                GuardarTop.GetComponent<Button>().enabled = true;
            }else{
                IsAllPlacesComplete = false;
            }
            
        }
        if(!IsAllPlacesComplete){
            GuardarTop.GetComponent<Button>().enabled = false;
            GuardarTop.GetComponent<Image>().color = new Color32 (128,128,128,255);
            IsAllPlacesComplete = true;
        }
        
        
        Añadir.SetActive(true);
    }

    public void ShowAllInstances(string _text, string SpotifyId){
        
        foreach (GameObject item in Instances)
        {
            
            if(item.activeSelf != true){
                Debug.Log("Show");
                item.SetActive(true);
            }else{
                item.GetComponent<PF_ADNMusicalEventSystem>().End(_text, SpotifyId);   
                
            }
            if(item.GetComponent<PF_ADNMusicalEventSystem>().GetPlaceHolder() != PlaceHolderText){
                GuardarTop.GetComponent<Image>().color = new Color32 (255,255,255,255);
                GuardarTop.GetComponent<Button>().enabled = true;
            }else{
                IsAllPlacesComplete = false;
            }



        }
        if(!IsAllPlacesComplete){
            GuardarTop.GetComponent<Button>().enabled = false;
            GuardarTop.GetComponent<Image>().color = new Color32 (128,128,128,255);
            IsAllPlacesComplete = true;
        }
        Añadir.SetActive(true);
    }

    private void CalculateMaxPrefabToCall(){
        if(MaxPrefabsInScreen ==0){
            if(Instances.Count != 0){
                MaxPrefabsInScreen = Mathf.Round((SpawnArea.GetComponent<RectTransform>().rect.height) / Instances[Instances.Count -1].GetComponent<RectTransform>().sizeDelta.y);
            }
            

        }
    }

    public void GetData(){
        DataPlaceHolders.Clear();
        DataSpotifyID.Clear();
        foreach (var item in Instances )
        {
            string _data = item.GetComponent<PF_ADNMusicalEventSystem>().GetPlaceHolder();
            string id = item.GetComponent<PF_ADNMusicalEventSystem>().GetSpotifyID();
            if(_data != PlaceHolderText){
                DataPlaceHolders.Add(_data);
                DataSpotifyID.Add(id);
            }
        }
    }
    public void ControlAñadirButton(){
        
        CurrentPrefabs++;
        
        if(CurrentPrefabs >= Max){
            Añadir.SetActive(false);
        }
        Debug.Log(ScrollPosition);
        ScrollBar.verticalNormalizedPosition = -0.001f;

        GuardarTop.GetComponent<Button>().enabled = false;
        GuardarTop.GetComponent<Image>().color = new Color32 (128,128,128,255);
        

    }

    public void DynamicPrefabSpawner(float howmanyprefabs){
        
        for (int i = 0; i <= howmanyprefabs; i++)
        {
            SpawnPrefab();
        }

        Añadir.transform.SetAsLastSibling();        
    }

    public void KillPrefablist(){
        foreach (GameObject Prefab in Instances)
        {
            Destroy(Prefab);
        }
        Instances.Clear();
    }

    public List<GameObject>  GetInstances(){
        return Instances;
    }

    public void SpawnPrefab(){
        
        Instance = Instantiate(Prefab,Añadir.transform.position, Quaternion.identity);
        Instance.transform.SetParent(GameObject.Find("PF_Container").transform);
        Instance.transform.localScale = new Vector3(1,1,1); 
        Instances.Add(Instance);
        Instance.name =  Prefab.name  + "-"+ Instances.Count;
        Instance.GetComponent<PF_ADNMusicalEventSystem>().ChangeName(Instances.Count, Min);
        Instance.GetComponent<PF_ADNMusicalEventSystem>().SetPrefab(PrefabToSet, Type);
        Instance.GetComponent<PF_ADNMusicalEventSystem>().SetPlaceHolder(PlaceHolderText);
        
        
    }
    
}
