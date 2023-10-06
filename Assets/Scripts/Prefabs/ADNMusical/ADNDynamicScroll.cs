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
    public GameObject SpawnArea, Prefab, Add, ScrollView, GuardarTop, container, TitleObject, PrefabParent, PrefabParentSelecPosition, TitleText, ScrollbarRestPosition, ScrollbarTypePosition;
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
    
    private string TypeString;
    
    private List<string> DB = new List<string>();
    private Vector3 PrefabParentPosition;

    [HideInInspector]
    private MwsiveUserRoot mwsiveUserRoot;
    public int TypeOfADN;
    public bool Editable;

    
    public void Initialize(int _TypeOfADN, bool _Editable, string _profileId) {
        TypeOfADN = _TypeOfADN;
        Editable = _Editable;
        
        
        
        
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

        SaveTopONOFF(false);

        if (!Editable){
            
            Add.SetActive(false);
            GuardarTop.SetActive(false);

        }
        PrefabParentPosition = PrefabParent.transform.position;
        
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
                    
                    DB.Add(severalartists.artists[i].id);
                }
                    if (i+1 <= Min && Editable)
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
                if(severaltracks.tracks[i] != null)
                    {
                        if (severaltracks.tracks[i].name != null)
                        {

                            Instances[i].GetComponent<PF_ADNMusicalEventSystem>().SetPlaceHolder(severaltracks.tracks[i].name);
                            Instances[i].GetComponent<PF_ADNMusicalEventSystem>().SetSpotifyID(severaltracks.tracks[i].id);
                            DB.Add(severaltracks.tracks[i].id);

                        }
                        if (i + 1 <= Min && Editable)
                        {
                            Instances[i].GetComponent<PF_ADNMusicalEventSystem>().ClearSearch.SetActive(true);
                        }
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
        PrefabParent.transform.localPosition = PrefabParentSelecPosition.transform.localPosition;
        TitleObject.SetActive(false);
        TitleText.SetActive(false);
        ScrollView.GetComponent<RectTransform>().offsetMin = ScrollbarTypePosition.GetComponent<RectTransform>().offsetMin;
        

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

        bool all_Null = false;
        bool flagNull = true;

        bool all_Full = false;
        bool flagFull = true;

        int i = 1;
        foreach (GameObject item in Instances)
        {
            string item_Place_Holder = item.GetComponent<PF_ADNMusicalEventSystem>().GetPlaceHolder();
            item.GetComponent<PF_ADNMusicalEventSystem>().ChangeName(i, Min);
            item.name =  Prefab.name  + "-"+ i;
            i++;

            if (flagNull)
            {
                if (item_Place_Holder == "" && item_Place_Holder == null)
                {
                    all_Null = true;

                }
                else
                {
                    flagNull = false;
                    all_Null = false;
                }
            }

            if (flagFull)
            {
                if (item_Place_Holder != PlaceHolderText && item_Place_Holder != null)
                {
                    all_Full = true;

                }
                else
                {
                    flagFull = false;
                    all_Full = false;
                }
            }

        }
        if (all_Full || all_Null)
        {
            SaveTopONOFF(CheckListsAreTheSame());

        }
        else
        {
            SaveTopONOFF(false);
        }

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
                item.SetActive(true);

            }
        }
        SaveTopONOFF(false);
        

        if (Max > Instances.Count)
        {
            Add.SetActive(true);
        }

        PrefabParent.transform.position = PrefabParentPosition;
        TitleObject.SetActive(true);
        TitleText.SetActive(true);

        ScrollView.GetComponent<RectTransform>().offsetMin = ScrollbarRestPosition.GetComponent<RectTransform>().offsetMin;
        ScrollBar.enabled = true;
    }
    public void ShowAllInstances(string _text, string SpotifyId){
        bool all_Null = false;
        bool flagNull = true;

        bool all_Full = false;
        bool flagFull = true;

        foreach (GameObject item in Instances)
        {
            string item_Place_Holder = item.GetComponent<PF_ADNMusicalEventSystem>().GetPlaceHolder();
            if (item.activeSelf != true){
                item.SetActive(true);
            }else{
                item.GetComponent<PF_ADNMusicalEventSystem>().End(_text, SpotifyId);
                item_Place_Holder = _text;
            }

            /// Falta verificar la lista para que no deje guardar cuando hay cosas que hay vacias, si es que hay algo que tiene texto, o all esta vacio o all esta lleno.
            if (flagNull)
            {
                if (item_Place_Holder == "" && item_Place_Holder == null)
                {
                    all_Null = true;

                }
                else
                {
                    flagNull = false;
                    all_Null = false;
                }
            }
  
            if (flagFull)
            {
                if (item_Place_Holder != PlaceHolderText && item_Place_Holder != null && item_Place_Holder != "")
                {
                    all_Full = true;

                }
                else
                {
                    flagFull = false;
                    all_Full = false;
                }
            }
            PrefabParent.transform.position = PrefabParentPosition;
            TitleObject.SetActive(true);
            TitleText.SetActive(true);
            ScrollBar.enabled = true;
            ScrollView.GetComponent<RectTransform>().offsetMin = ScrollbarRestPosition.GetComponent<RectTransform>().offsetMin;
        }
        
        if (all_Full || all_Null)
        {
            SaveTopONOFF(CheckListsAreTheSame());
            
        }
        else
        {
            SaveTopONOFF(false);
        }
        if(Max > Instances.Count)
        {
            Add.SetActive(true);
        }
        
    }

    private void SaveTopONOFF(bool _value)
    {
        if (_value)
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
    private bool CheckListsAreTheSame()
    {
        if(DB.Count == Instances.Count)
        {
            for(int I = 0; I < Instances.Count; I++)
            {
                if (Instances[I].GetComponent<PF_ADNMusicalEventSystem>().GetSpotifyID() != DB[I])
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            return true;
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

            
        }
        StopAllCoroutines();
        MwsiveConnectionManager.instance.PostMusicalDNA(TypeString, data.ToArray(), Callback_PostMusicalDNA );
        DB = data;
    }

    public void Callback_PostMusicalDNA( object[] _value){
        
        UIMessage.instance.UIMessageInstanciate("Se ha actualizado tu lista");
        SaveTopONOFF(false);

    }


    public void ControlAddButton(){
        
        
        
        if(Instances.Count >= Max){
            Add.SetActive(false);
        }
        ScrollBar.verticalNormalizedPosition = -0.001f;

        SaveTopONOFF(false);


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
        Instance.transform.SetParent(container.transform);
        Instance.transform.localScale = new Vector3(1,1,1); 
        Instances.Add(Instance);
        Instance.name =  Prefab.name  + "-"+ Instances.Count;
        Instance.GetComponent<PF_ADNMusicalEventSystem>().ChangeName(Instances.Count, Min);
        Instance.GetComponent<PF_ADNMusicalEventSystem>().SetPrefab(PrefabToSet, Type);
        Instance.GetComponent<PF_ADNMusicalEventSystem>().SetPlaceHolder(PlaceHolderText);

        if (!Editable)
        {
            Instance.GetComponentInChildren<TMP_InputField>().interactable = false;
            Instance.GetComponent<PF_ADNMusicalEventSystem>().EraseButton.SetActive(false);
        }
        
        
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
                    break;
                }
            }

            if (flag)
            {
                SaveTopONOFF(CheckListsAreTheSame());
            }
        }
        
    }

}
