using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicScroll : MonoBehaviour
{
    public float MaxPrefabsInScreen = 0;
    public ScrollRect ScrollBar;
    private float ScrollbarVerticalPos =-0.001f;
    public GameObject SpawnArea, Prefabs, LastPosition, PrefabPosition, Container;
    private GameObject Instance;  
    public List<GameObject> Instances = new List<GameObject>(); 

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckForSpawn(){
        Debug.Log(ScrollBar.verticalNormalizedPosition + " " + ScrollbarVerticalPos);
        if(Instances.Count != 0){
            if(ScrollBar.verticalNormalizedPosition  <= ScrollbarVerticalPos){
                
                DynamicPrefabSpawner(MaxPrefabsInScreen);
            }
        }
        
    }
    private void CalculateMaxPrefabToCall(){
        if(MaxPrefabsInScreen ==0){
            if(Instances.Count != 0){
                MaxPrefabsInScreen = Mathf.Round((SpawnArea.GetComponent<RectTransform>().rect.height) / Instances[Instances.Count -1].GetComponent<RectTransform>().sizeDelta.y);
            }
            

        }
    }



    public void DynamicPrefabSpawner(float howmanyprefabs){
        Debug.Log(MaxPrefabsInScreen);
        if(MaxPrefabsInScreen ==0){
            
            CalculateMaxPrefabToCall();
        }
        if(howmanyprefabs != MaxPrefabsInScreen){
            for (int i = 0; i <= howmanyprefabs; i++)
            {
                
                SpawnPrefab();
            }
        }else{
            for (int i = 0; i <= MaxPrefabsInScreen; i++)
            {
                
                SpawnPrefab();
            }
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

    public List<GameObject>  GetInstances(){
        return Instances;
    }

    public void SpawnPrefab(){
        
        Instance = Instantiate(Prefabs,PrefabPosition.transform.position, Quaternion.identity);
        Instance.transform.SetParent(GameObject.Find(Container.name).transform);
        Instance.transform.localScale = new Vector3(1,1,1); 
        Instances.Add(Instance);
        Instance.name =  Prefabs.name  + "-"+ Instances.Count;

    }
    
}
