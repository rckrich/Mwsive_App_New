using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DynamicSearchPrefabEventSystem : MonoBehaviour
{
    public DescubrirPaginas Descubrir;
    public GameObject Searchbar;
    public List<GameObject> Prefabs = new List<GameObject>();    
    public List<GameObject> LastPosition = new List<GameObject>();   
    public GameObject PrefabsPosition;
    public GameObject SpawnArea;
    public List<ScrollRect> Scrollbar = new List<ScrollRect>(); 
    public List<List<GameObject>> ListOfLists = new List<List<GameObject>>();
    private string SearchText;
    private GameObject Instance;
    public int numEnpantalla;
    private bool EnableSerach;
    public float MaxPrefabsinScreen = 0;
    private float ScrollbarVerticalPos =-0.001f;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject item in Prefabs)
        {
            ListOfLists.Add( new List<GameObject>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return)){
            EnableSerach = true;
            Search();
        }else{
            EnableSerach = false;
        }
    }

    

    public void Search(){
        int lastnum= numEnpantalla;
        numEnpantalla = Descubrir.GetCurrentEscena();
        SearchText = Searchbar.GetComponent<TMP_InputField>().text;
        
        if(SearchText.Length >= 3 || EnableSerach){
            if(numEnpantalla != lastnum){
                KillPrefablist(lastnum);
            }
            if(ListOfLists[numEnpantalla].Count ==0){
                CalculateMaxPrefabToCall();
                DynamicPrefabSpawner(MaxPrefabsinScreen);
            }
        } 
    }
    
    public void CheckForSpawn(){
        if(ListOfLists[numEnpantalla].Count != 0){
            if(Scrollbar[numEnpantalla].verticalNormalizedPosition  <= ScrollbarVerticalPos){
                DynamicPrefabSpawner(10);
            }
        }
        
    }
    
    public void DynamicPrefabSpawner(float prefabs){
        numEnpantalla = Descubrir.GetCurrentEscena();
        if(ListOfLists[numEnpantalla].Count == 0){
            CalculateMaxPrefabToCall();
        }
            for (int i = 0; i <= prefabs; i++)
            {
                SpawnPrefab(true);
            }
        LastPosition[numEnpantalla].transform.SetAsLastSibling();

    }

    private void CalculateMaxPrefabToCall(){
        if(MaxPrefabsinScreen ==0){
                  
            MaxPrefabsinScreen = Mathf.Round((SpawnArea.GetComponent<RectTransform>().rect.height) / Prefabs[numEnpantalla].GetComponent<RectTransform>().sizeDelta.y);
            Debug.Log(SpawnArea.GetComponent<RectTransform>().rect.height);
            Debug.Log(Prefabs[numEnpantalla].GetComponent<RectTransform>().sizeDelta.y);

        }
    }

    public void KillAllPrefabLists(){
        Searchbar.GetComponent<TMP_InputField>().text = null;
        foreach (List<GameObject> ListPrefab in ListOfLists)
        {
            foreach(GameObject Prefab in ListPrefab){
                Destroy(Prefab);
            }
            ListPrefab.Clear(); 
        }
    }

    public void KillPrefablist(int scene){
        foreach (GameObject Prefab in ListOfLists[scene])
        {
            Destroy(Prefab);
        }
        ListOfLists[scene].Clear();
    }
    private void SpawnMejoresResultados(int numoftimes){
        for (int i = 0; i <= numoftimes; i++)
        {
            for (int j = 1; j <= Prefabs.Count-1; j++)
            {
                Debug.Log(j);
                Instance = Instantiate(Prefabs[j],PrefabsPosition.transform.position, Quaternion.identity);
                Instance.transform.SetParent(GameObject.Find("PF_ResultadosdeBusqueda_Container").transform);
                Instance.transform.localScale = new Vector3(1,1,1);  
                ListOfLists[0].Add(Instance);
                ListOfLists[numEnpantalla].Add(Instance);
            }   
            
        }
        
        
    }

    private void SpawnPrefab(bool IsVisible){
        switch (numEnpantalla){
            case 0:
                SpawnMejoresResultados(1);
                LastPosition[numEnpantalla].transform.SetAsLastSibling();
                break;
            case 1:
                Instance = Instantiate(Prefabs[numEnpantalla],PrefabsPosition.transform.position, Quaternion.identity);
                Instance.transform.SetParent(GameObject.Find("PF_Curadores_Container").transform);
                Instance.transform.localScale = new Vector3(1,1,1);  
                Instance.SetActive(IsVisible);
                ListOfLists[numEnpantalla].Add(Instance);
                break;
            case 2:
                Instance = Instantiate(Prefabs[numEnpantalla],PrefabsPosition.transform.position, Quaternion.identity);
                Instance.transform.SetParent(GameObject.Find("PF_Songs_Container").transform);
                Instance.transform.localScale = new Vector3(1,1,1);
                Instance.SetActive(IsVisible);
                ListOfLists[numEnpantalla].Add(Instance);   
                break;
            case 3: 
                Instance = Instantiate(Prefabs[numEnpantalla],PrefabsPosition.transform.position, Quaternion.identity);
                Instance.transform.SetParent(GameObject.Find("PF_Artists_Container").transform);
                Instance.transform.localScale = new Vector3(1,1,1);  
                Instance.SetActive(IsVisible); 
                ListOfLists[numEnpantalla].Add(Instance);
                break;
            case 4: 
                Instance = Instantiate(Prefabs[numEnpantalla],PrefabsPosition.transform.position, Quaternion.identity);
                Instance.transform.SetParent(GameObject.Find("PF_Albums_Container").transform);
                Instance.transform.localScale = new Vector3(1,1,1);
                Instance.SetActive(IsVisible);
                ListOfLists[numEnpantalla].Add(Instance);  
                break;
            case 5: 
                Instance = Instantiate(Prefabs[numEnpantalla],PrefabsPosition.transform.position, Quaternion.identity);
                Instance.transform.SetParent(GameObject.Find("PF_Playlists_Container").transform);
                Instance.transform.localScale = new Vector3(1,1,1);
                Instance.SetActive(IsVisible);
                ListOfLists[numEnpantalla].Add(Instance);   
                break;
            case 6: 
                Instance = Instantiate(Prefabs[numEnpantalla],PrefabsPosition.transform.position, Quaternion.identity);
                Instance.transform.SetParent(GameObject.Find("PF_Genders_Container").transform);
                Instance.transform.localScale = new Vector3(1,1,1);
                Instance.SetActive(IsVisible);
                ListOfLists[numEnpantalla].Add(Instance);
                break;
        }

    }
}
