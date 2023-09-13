using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ADNDynamicScroll : MonoBehaviour
{
    
    public float MaxPrefabsInScreen = 0;
    public ScrollRect ScrollBar;
    public GameObject SpawnArea, Prefab, Add, ScrollView, GuardarTop, container;
    private GameObject Instance;  
    public List<GameObject> Instances = new List<GameObject>();
    private static ADNDynamicScroll _instance;
    public List<string> DataPlaceHolders = new List<string>(); 
    public List<string> DataSpotifyID = new List<string>(); 
    

    public TextMeshProUGUI Title, Subtitle; 
    public List<GameObject> Prefabs = new List<GameObject>(); 
    
    private string PlaceHolderText;
    private GameObject PrefabToSet;
    private int Type, Max, Min;
    private float ScrollPosition = -0.3f;
    private bool IsAllPlacesComplete = true;
    private string TypeString;
    private bool isAllDataNull = false;

    [HideInInspector]
    private MwsiveUserRoot mwsiveUserRoot;
    public int TypeOfADN;
    private bool Editable;

    
    public void Initialize(int _TypeOfADN, bool _Editable, string _profileId) {
        
        TypeOfADN = _TypeOfADN;
        Editable = _Editable;
        
        Debug.Log(Editable);
        
        
        switch(TypeOfADN){
            case 0:
                Title.text = "ON_REPEAT";
                TypeString = "ON_REPEAT";
                if(Editable){
                    Subtitle.text = "Escribe tus cancion favorita";
                    PlaceHolderText = "Buscar Canción";
                }else
                {
                    Subtitle.text = "";
                    PlaceHolderText = "";
                }
                PrefabToSet = Prefabs[1];
                Max = 1;
                Min = 1;
                Type = 1;
                
                Add.SetActive(false);
                break;
            case 1:
                Title.text = "ARTISTA QUE REVIVIRÍA";
                TypeString = "ON_REVIVAL";
                if(Editable){
                    Subtitle.text = "Escribe tus artistas favoritos";
                    PlaceHolderText = "Buscar Artista";
                }else{
                    Subtitle.text = "";
                    PlaceHolderText = "";
                }
                PrefabToSet = Prefabs[0];
                Max = 5;
                Min = 1;
                Type = 0;
                
                break;
            case 2:
                Title.text = "GUSTO CULPOSO";
                TypeString = "GUILTY_PLEASURE";
                if(Editable){
                    Subtitle.text = "Escribe tus canciones favoritas";
                    PlaceHolderText = "Buscar Canción";
                }else{
                    Subtitle.text = "";
                    PlaceHolderText = "";
                }
                PrefabToSet = Prefabs[1];
                Max = 5;
                Min = 1;
                Type = 1;
                
                break;
            case 3:
                Title.text = "DE AMOR";
                TypeString = "ON_LOVE";
                if(Editable){
                    Subtitle.text = "Escribe tus canciones favoritas";
                    PlaceHolderText = "Buscar Canción";
                }else{
                    Subtitle.text = "";
                    PlaceHolderText = "";
                }
                PrefabToSet = Prefabs[1];
                Max = 5;
                Min = 1;
                Type = 1;
                break;
            case 4:
                Title.text = "ULTIMO DESCUBRIMIENTO";
                TypeString = "LATEST_DISCOVERIES";
                if(Editable){
                    Subtitle.text = "Escribe tus canciones favoritas";
                    PlaceHolderText = "Buscar Canción";
                }else{
                    Subtitle.text = "";
                    PlaceHolderText = "";
                }
                Max = 5;
                Min = 1;
                PrefabToSet = Prefabs[1];
                Type = 1;
                
                break;
            case 5:
                Title.text = "PROXIMA ESTRELLA";
                TypeString = "NEXT_STARS";
                if(Editable){
                    Subtitle.text = "Escribe tus artistas favoritos";
                    PlaceHolderText = "Buscar Artista";
                }else{
                    Subtitle.text = "";
                    PlaceHolderText = "";
                }
                PrefabToSet = Prefabs[0];
                Max = 5;
                Min = 1;
                Type = 0;
                
                break;
            case 6:
                Title.text = "GOAT";
                TypeString = "GOATS";
                
                if(Editable){
                    Subtitle.text = "Escribe tus artista favorito";
                    PlaceHolderText = "Buscar Artista";
                }else{
                    Subtitle.text = "";
                    PlaceHolderText = "";
                }
                PrefabToSet = Prefabs[0];
                Max = 1;
                Min = 1;
                Type = 0;
                Add.SetActive(false);
                
                break;
            
            case 7:
                Title.text = "EL SOUNDTRACK DE MI VIDA";
                TypeString = "OST";
                if(Editable){
                    Subtitle.text = "Escribe tus canciones favoritas";
                    PlaceHolderText = "Buscar Canción";
                }else{
                    Subtitle.text = "";
                    PlaceHolderText = "";
                }
                PrefabToSet = Prefabs[1];
                Max = 18;
                Min = 4;
                Type = 1;
                
                
                break;
        }
        
        MwsiveConnectionManager.instance.GetMwsiveUser(_profileId, Callback_GetMwsiveUser);
        
        GuardarTop.GetComponent<Button>().enabled = false;
        GuardarTop.GetComponent<Image>().color = new Color32 (128,128,128,255);

        if(!Editable){
            
            Add.SetActive(false);
            GuardarTop.SetActive(false);

        }
        
    }
    public int GetMin()
    {
        return Min;
    }

    public void CheckConfirmButton()
    {
        if (GuardarTop.GetComponent<Button>().enabled && Instances.Count == 1)
        {
            GuardarTop.GetComponent<Button>().enabled = false;
            GuardarTop.GetComponent<Image>().color = new Color32(128, 128, 128, 255);
        }
    }

    public void Callback_GetMwsiveUser(object[] _value)
    {
        mwsiveUserRoot = (MwsiveUserRoot)_value[1];
        if (mwsiveUserRoot.user.user_lists != null)
        {
            bool flag = false;

            foreach (var item in mwsiveUserRoot.user.user_lists)
            {
                if (item.type == TypeString && item.items_list != null && item.items != "" && item.items_list[0] != null)
                {
                    if (Type == 0)
                    {
                        SpotifyConnectionManager.instance.GetSeveralArtists(item.items_list.ToArray(), OnCallback_SetPlaceHoldersArtists);
                        flag = true;
                        break;
                    }
                    else
                    {
                        SpotifyConnectionManager.instance.GetSeveralTracks(item.items_list.ToArray(), OnCallback_SetPlaceHoldersTracks);
                        flag = true;
                        break;
                    }
                }

            }

            if (!flag)
            {

                if (!Editable)
                {
                    UIMessage.instance.UIMessageInstanciate("La lista esta vacia");

                }
                else
                {
                    DynamicPrefabSpawner(Min - 1);

                }


            }
        }


    }   
    public void OnClick_CurrentUser()
    {
        Editable = true;
    }

    public void OnCallback_SetPlaceHoldersArtists(object [] _value){
        SeveralArtistRoot severalartists = (SeveralArtistRoot)_value[1];
        if(severalartists != null){
            if(severalartists.artists.Count >= Min){
            for (int i = 0; i < severalartists.artists.Count; i++)
            {
                DynamicPrefabSpawner(0);
                if(severalartists.artists[i].name != null || severalartists.artists[i].name  != ""){
                    Instances[i].GetComponent<PF_ADNMusicalEventSystem>().SetPlaceHolder(severalartists.artists[i].name);
                    Instances[i].GetComponent<PF_ADNMusicalEventSystem>().SetSpotifyID(severalartists.artists[i].id);
                }
                    if (i+1 <= Min)
                    {
                        Instances[i].GetComponent<PF_ADNMusicalEventSystem>().ClearSearch.SetActive(true);
                    }

                }
            }else{
                DynamicPrefabSpawner(Min-1);
            }
        }
        
        
        if (Instances.Count >= Max-1)
        {
            Add.SetActive(false);
            
            
        }

        if (!Editable)
        {
            foreach (GameObject item in Instances)
            {
                item.GetComponentInChildren<TMP_InputField>().interactable = false;
                item.GetComponent<PF_ADNMusicalEventSystem>().EraseButton.SetActive(false);
            }
        }


        

    }

    public void OnCallback_SetPlaceHoldersTracks(object [] _value){
        SeveralTrackRoot severaltracks = (SeveralTrackRoot)_value[1];
        if(severaltracks != null){
            if(severaltracks.tracks.Count >= Min){
                for (int i = 0; i < severaltracks.tracks.Count; i++)
                {
                    
                    DynamicPrefabSpawner(0);
                    
                if(severaltracks.tracks[i].name != null){
                    
                    Instances[i].GetComponent<PF_ADNMusicalEventSystem>().SetPlaceHolder(severaltracks.tracks[i].name);
                    Instances[i].GetComponent<PF_ADNMusicalEventSystem>().SetSpotifyID(severaltracks.tracks[i].id);

                    }
                    if(i+1 <= Min)
                    {
                        Instances[i].GetComponent<PF_ADNMusicalEventSystem>().ClearSearch.SetActive(true);
                    }
                }

            }else{
                DynamicPrefabSpawner(Min-1);
            }
        }

        if (Instances.Count >= Max)
            {
                Add.SetActive(false);


            }


        

        if (!Editable)
        {
            foreach (GameObject item in Instances)
            {
                item.GetComponentInChildren<TMP_InputField>().interactable = false;
                item.GetComponent<PF_ADNMusicalEventSystem>().EraseButton.SetActive(false);
            }
        }

    }

    public void HideShowHeader(){
        ScrollView.GetComponent<ScrollRect>().enabled = false;
    }
    
    
    public void HideAllOtherInstances(string gameObjectname){
        
        foreach (GameObject item in Instances)
        {
            if(item.name != gameObjectname){
                item.SetActive(false);
            }
            
        }
        Add.SetActive(false);

        container.transform.localPosition = new Vector3(0, 0, 0);
        
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

        Debug.Log(Max + " " + Instances.Count);
        if (Max > Instances.Count)
        {
            Add.SetActive(true);
        }
    }

    public void ShowAllInstances()
    {
        foreach (GameObject item in Instances)
        {

            if (item.activeSelf != true)
            {
                Debug.Log("Show");
                item.SetActive(true);

            }
        }
        GuardarTop.GetComponent<Button>().enabled = false;
        GuardarTop.GetComponent<Image>().color = new Color32(128, 128, 128, 255);
        IsAllPlacesComplete = true;

        if (Max > Instances.Count)
        {
            Add.SetActive(true);
        }

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

            /// Falta verificar la lista para que no deje guardar cuando hay cosas que hay vacias, si es que hay algo que tiene texto, o all esta vacio o all esta lleno.

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
        Debug.Log(Max + " " + Instances.Count);
        if(Max > Instances.Count)
        {
            Add.SetActive(true);
        }
        
    }

    private void CalculateMaxPrefabToCall(){
        if(MaxPrefabsInScreen ==0){
            if(Instances.Count != 0){
                MaxPrefabsInScreen = Mathf.Round((SpawnArea.GetComponent<RectTransform>().rect.height) / Instances[Instances.Count -1].GetComponent<RectTransform>().sizeDelta.y);
            }
            

        }
    }

    public void SetData(){
        List<string> data = new List<string>();
        foreach (GameObject item in Instances )
        {
            data.Add(item.GetComponent<PF_ADNMusicalEventSystem>().GetSpotifyID());
            if (item.GetComponent<PF_ADNMusicalEventSystem>().GetSpotifyID() == null)
            {
                isAllDataNull = true;
            }
            else
            {
                isAllDataNull = false;
            }
        }
        StopAllCoroutines();
        MwsiveConnectionManager.instance.PostMusicalDNA(TypeString, data.ToArray(), Callback_PostMusicalDNA );
    }

    public void Callback_PostMusicalDNA( object[] _value){
        
        UIMessage.instance.UIMessageInstanciate("Se ha actualizado tu lista");
        GuardarTop.GetComponent<Button>().enabled = false;
        GuardarTop.GetComponent<Image>().color = new Color32(128, 128, 128, 255);

    }


    public void ControlAddButton(){
        
        
        
        if(Instances.Count >= Max){
            Add.SetActive(false);
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

        Add.transform.SetAsLastSibling();        
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
        
        Instance = Instantiate(Prefab,Add.transform.position, Quaternion.identity);
        Instance.transform.SetParent(GameObject.Find("PF_Container").transform);
        Instance.transform.localScale = new Vector3(1,1,1); 
        Instances.Add(Instance);
        Instance.name =  Prefab.name  + "-"+ Instances.Count;
        Instance.GetComponent<PF_ADNMusicalEventSystem>().ChangeName(Instances.Count, Min);
        Instance.GetComponent<PF_ADNMusicalEventSystem>().SetPrefab(PrefabToSet, Type);
        Instance.GetComponent<PF_ADNMusicalEventSystem>().SetPlaceHolder(PlaceHolderText);
        
        
    }

    

    public void CheckClearList(int _positiion)
    {
        if(_positiion <= Min )
        {
            bool flag = false;
            foreach (GameObject item in Instances)
            {
                if (item.GetComponent<PF_ADNMusicalEventSystem>().GetPlaceHolder() == "" || item.GetComponent<PF_ADNMusicalEventSystem>().GetPlaceHolder() == null)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }



            if (flag && !isAllDataNull)
            {
                GuardarTop.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                GuardarTop.GetComponent<Button>().enabled = true;
            }
            else
            {
                GuardarTop.GetComponent<Button>().enabled = false;
                GuardarTop.GetComponent<Image>().color = new Color32(128, 128, 128, 255);
            }
        }
        
    }
    
}
