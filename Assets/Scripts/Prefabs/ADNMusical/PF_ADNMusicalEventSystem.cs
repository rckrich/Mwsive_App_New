using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PF_ADNMusicalEventSystem : MonoBehaviour
{

    public string[] types = new string[] {"artist"};

    public GameObject PlaceHolder, DynamicScroll, EraseButton, BackButton, ClearSearch;
    private string SearchText, SpotifyId, PlaceHolderText;
    public  TMP_InputField searchbar;
    public TextMeshProUGUI Number;
    public int MaxNumerofPrefabsInstanciate;
   
    public int PositionInSearch = 0;



    [Header("Dynamic Scroll")]
    public ScrollRect ScrollBar;
    private float ScrollbarVerticalPos =-0.001f;
    public GameObject LastPosition, Container;
    private GameObject Instance, Prefab;  
    public List<GameObject> Instances = new List<GameObject>();
    public bool CheckForSpawnHasEnded = true;
    private int Type;
    private bool EraseButtonNever = false;
    public int PositionNum = 0;


    

    public void ChangeName(int num, int min ){
        PositionNum = num;
        Number.text = "#" +num;
        if( num <= min){
            EraseButton.SetActive(false);
            EraseButtonNever = true;
        }
    }
    
    public void SetPrefab(GameObject _Prefab, int _Type){
        Prefab = _Prefab;
        
        Type = _Type;
    }

    public void OnClick_ClearSearch()
    {
        searchbar.text = "";
        SpotifyId = null;
        PlaceHolder.GetComponent<TextMeshProUGUI>().text = "";
        FindObjectOfType<ADNDynamicScroll>().CheckClearList(PositionNum);
        ClearSearch.SetActive(false);

    }

    public void HideEraseButton()
    {
        EraseButton.SetActive(false);
    }

    public void Search(){
        
        SearchText = searchbar.text;
        
            if(SearchText.Length >= 1){
                ClearSearch.SetActive(true);
                PlaceHolder.SetActive(false);
                CheckForSpawnHasEnded = true;
                
                DynamicScroll.transform.DOScaleY(1, 0.5F);
                if (Instances.Count !=0){
                    KillPrefablist();       
                }

                SearchItem();
                
            }
        
    }
    private void SearchItem(){
        DynamicPrefabSpawner();
        switch (Type)
        {
            case 0:
                types = new string[] {"artist"};
                SpotifyConnectionManager.instance.SearchForItem(SearchText, types, Callback_OnCLick_SearchForItem, "ES", MaxNumerofPrefabsInstanciate);
                break;
            case 1:
                types = new string[] {"track"};
                SpotifyConnectionManager.instance.SearchForItem(SearchText, types, Callback_OnCLick_SearchForItem, "ES", MaxNumerofPrefabsInstanciate);
                break;
        }
        PositionInSearch = MaxNumerofPrefabsInstanciate;
        ScrollBar.verticalNormalizedPosition = 1;
    }

    public void OnSelectInputField(){
        ADNDynamicScroll.instance.HideShowHeader();
        EraseButton.SetActive(false);
        BackButton.SetActive(true);
            
            
        ADNDynamicScroll.instance.HideAllOtherInstances(gameObject.name);
    }


    


    public string GetPlaceHolder(){
        return PlaceHolder.GetComponent<TextMeshProUGUI>().text;
    }

    public string GetSpotifyID(){
        return SpotifyId;
    }

    public void SetSpotifyID(string _SpotifyID)
    {
        SpotifyId = _SpotifyID;
    }

    public void SetPlaceHolder(string _text){
        if(_text.Length > 27){
            string _text2 = "";
            for (int i = 0; i < 27; i++)
            {
                
                _text2 =_text2 + _text[i];
                
            }
            _text2 = _text2 + "...";
            PlaceHolder.GetComponent<TextMeshProUGUI>().text = _text2;
            PlaceHolderText = _text2;
        }
        else{
            PlaceHolder.GetComponent<TextMeshProUGUI>().text = _text;
            PlaceHolderText = _text;
        }



        PlaceHolder.GetComponent<TextMeshProUGUI>().text = _text;
    }

    IEnumerator UpdateLayoutGroup()
    {
        gameObject.GetComponent<VerticalLayoutGroup>().enabled= false;
    
        yield return new WaitForEndOfFrame();
        gameObject.GetComponent<VerticalLayoutGroup>().enabled= true;
    }

    public void End(string _text, string _spotifyid){
        BackButton.SetActive(false);
        if (PositionNum > FindObjectOfType<ADNDynamicScroll>().GetMin())
        {
            ClearSearch.SetActive(false);
        }
        else
        {
            ClearSearch.SetActive(true);
        }
        SpotifyId = _spotifyid;
        KillPrefablist();
        PlaceHolder.GetComponent<TextMeshProUGUI>().text = _text;
        
        searchbar.text = "";
        DynamicScroll.transform.DOScaleY(0, 0.5F);
        
        PlaceHolder.SetActive(true);
        if(!EraseButtonNever){
            EraseButton.SetActive(true);
        }
        
        
    }

    

    private void Callback_OnCLick_SearchForItem(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        SearchRoot searchRoot = (SearchRoot)_value[1];
        
        switch (Type)
        {
            case 0:
            if (searchRoot.artists != null && Instances.Count > 0){
                for (int i = 0; i < searchRoot.artists.items.Count; i++)
                {
                    try
                    {
                        if(searchRoot.artists.items[i].images != null){
                            
                            Instances[i].GetComponent<ADNMusicalPrefabInitializaer>().InitializeSingleWithBackgroundWithImage(searchRoot.artists.items[i].name, searchRoot.artists.items[i].images[0].url, searchRoot.artists.items[i].id);
                        }else{
                            
                            Instances[i].GetComponent<ADNMusicalPrefabInitializaer>().InitializeSingleWithBackgroundNoImage(searchRoot.artists.items[i].name, searchRoot.artists.items[i].id); 
                        }
                        
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        
                        Instances[i].GetComponent<ADNMusicalPrefabInitializaer>().InitializeSingleWithBackgroundNoImage(searchRoot.artists.items[i].name, searchRoot.artists.items[i].id); 
                        
                    }
                }
            }   
                
            break;

            case 1:
                if (searchRoot.tracks != null && Instances.Count > 0)
                {
                    for (int i = 0; i < searchRoot.tracks.items.Count; i++)
                        {
                            try
                            {
                                if (searchRoot.tracks.items[i].album.images != null){
                                    
                                    Instances[i].GetComponent<ADNMusicalPrefabInitializaer>().InitializeDoubleWithBackgroundWithImage(searchRoot.tracks.items[i].name, searchRoot.tracks.items[i].artists[0].name, searchRoot.tracks.items[i].album.images[0].url, searchRoot.tracks.items[i].id);
                                }else{
                                    
                                    Instances[i].GetComponent<ADNMusicalPrefabInitializaer>().InitializeDoubleWithBackgroundNoImage(searchRoot.tracks.items[i].name, searchRoot.tracks.items[i].artists[0].name, searchRoot.tracks.items[i].id);
                                }
                                
                            }
                            catch (System.ArgumentOutOfRangeException)
                            {
                                
                                Instances[i].GetComponent<ADNMusicalPrefabInitializaer>().InitializeDoubleWithBackgroundNoImage(searchRoot.tracks.items[i].name, "papa", searchRoot.tracks.items[i].id);
                            }
                            
                            
                            
                        
                        }
                }   
            break;
        }


        
    }

    private void Callback_OnCLick_CheckForSpawn(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        SearchRoot searchRoot = (SearchRoot)_value[1];
        switch (Type)
        {
            case 0:
                if (searchRoot.artists != null){
                    for (int i = 0; i < MaxNumerofPrefabsInstanciate; i++)
                    {
                        try
                        {
                            if (searchRoot.artists.items[i].images != null){
                                Instances[PositionInSearch+i].GetComponent<ADNMusicalPrefabInitializaer>().InitializeSingleWithBackgroundWithImage(searchRoot.artists.items[i].name, searchRoot.artists.items[i].images[0].url, searchRoot.artists.items[i].id);
                            }else{
                                Instances[PositionInSearch+i].GetComponent<ADNMusicalPrefabInitializaer>().InitializeDoubleWithBackgroundNoImage(searchRoot.artists.items[i].name, searchRoot.artists.items[i].artists[0].name, searchRoot.artists.items[i].id);
                            }
                        }

                        catch (System.ArgumentOutOfRangeException)
                        {
                            Instances[PositionInSearch+i].GetComponent<ADNMusicalPrefabInitializaer>().InitializeSingleWithBackgroundNoImage(searchRoot.artists.items[i].name, searchRoot.artists.items[i].id);
                            
                        }
                    
                    }
                }   
                
                break;
            case 1:
                if (searchRoot.tracks != null){
                    for (int i = 0; i < MaxNumerofPrefabsInstanciate; i++)
                    {
                        try
                        {
                            if (searchRoot.tracks.items[i].album.images != null){
                                Instances[PositionInSearch+i].GetComponent<ADNMusicalPrefabInitializaer>().InitializeDoubleWithBackgroundWithImage(searchRoot.tracks.items[i].name, searchRoot.tracks.items[i].artists[0].name, searchRoot.tracks.items[i].album.images[0].url, searchRoot.tracks.items[i].id);
                            }else{
                                Instances[PositionInSearch+i].GetComponent<ADNMusicalPrefabInitializaer>().InitializeSingleWithBackgroundNoImage(searchRoot.tracks.items[i].name, searchRoot.tracks.items[i].id);
                            }
                        }

                        catch (System.ArgumentOutOfRangeException)
                        {
                            Instances[PositionInSearch+i].GetComponent<ADNMusicalPrefabInitializaer>().InitializeDoubleWithBackgroundNoImage(searchRoot.tracks.items[i].name, searchRoot.tracks.items[i].artists[0].name, searchRoot.tracks.items[i].id);
                            
                        }
                    
                    }
                }   
                
                break;
        }
        CheckForSpawnHasEnded = true;
        PositionInSearch = PositionInSearch + MaxNumerofPrefabsInstanciate;
    }


    public void CheckForSpawn(){
        
        if(Instances.Count != 0 && CheckForSpawnHasEnded){
            if(ScrollBar.verticalNormalizedPosition  <= ScrollbarVerticalPos){
                CheckForSpawnHasEnded = false;
                DynamicPrefabSpawner();
                SpotifyConnectionManager.instance.SearchForItem(SearchText, types, Callback_OnCLick_CheckForSpawn, "ES", MaxNumerofPrefabsInstanciate, PositionInSearch);
                
                

            }
        }
        
    }

    public void OnClickErase(){
        ADNDynamicScroll.instance.DestroyInstance(gameObject);
    }

    public void OnClick_BackButton()
    {
        ADNDynamicScroll.instance.HideShowHeader();
        ADNDynamicScroll.instance.CheckConfirmButton();
        ADNDynamicScroll.instance.ShowAllInstances();

        BackButton.SetActive(false);
        if(PositionNum > FindObjectOfType<ADNDynamicScroll>().GetMin())
        {
            ClearSearch.SetActive(false);
        }
        else
        {
            ClearSearch.SetActive(true);
        }
        


        KillPrefablist();
        searchbar.text = "";
        PlaceHolder.GetComponent<TextMeshProUGUI>().text = PlaceHolderText;
        DynamicScroll.transform.DOScaleY(0, 0.5F);

        PlaceHolder.SetActive(true);
        if (!EraseButtonNever)
        {
            EraseButton.SetActive(true);
        }
        


    }


    public void DynamicPrefabSpawner(){
        for (int i = 0; i <= MaxNumerofPrefabsInstanciate-1; i++)
        {
            SpawnPrefab();
        }
        LastPosition.transform.SetAsLastSibling();

    }

    public void KillPrefablist(){
        foreach (GameObject Prefab in Instances)
        {
            Destroy(Prefab);
        }
        Instances.Clear();
    }

    public List<GameObject> GetInstances(){
        return Instances;
    }

    public void SpawnPrefab(){
        
        Instance = Instantiate(Prefab,LastPosition.transform.position, Quaternion.identity);
        Instance.SetActive(false);
        Instance.transform.SetParent(GameObject.Find(Container.name).transform);
        Instance.transform.localScale = new Vector3(1,1,1); 
        Instances.Add(Instance);
        Instance.name =  Prefab.name  + "-"+ Instances.Count;

    }






}
