using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolManager : MonoBehaviour
{
    
    private int maxAmmountOfPrefabs = 10;
    public GameObject prefab;

    private List<GameObject> pooledObjects = new List<GameObject>();

    private static PoolManager _instance;

    public static PoolManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PoolManager>();
            }
            return _instance;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < maxAmmountOfPrefabs; i++)
        {
            GameObject _prefab = Instantiate(prefab);
            
            _prefab.name = _prefab.name + i;
            _prefab.transform.SetParent(gameObject.transform);
            pooledObjects.Add(_prefab);
            _prefab.SetActive(false);
        }
    }

    // Update is called once per frame
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy  || pooledObjects[i].GetComponent<SurfAni>().isAvailable)
            {
                return pooledObjects[i];
            }
        }
        Debug.LogWarning("No more Objects In Pool");
        return null;
    }


    public void RecoverPooledObject(GameObject _object)
    {
        //Antes de que se muera el surf manager hay que recuperar los items de las pools.
        foreach(GameObject item in pooledObjects)
        {
            if(item.transform.root.name != gameObject.name)
            {
                item.GetComponent<ButtonSurfPlaylist>().ClearData();
                item.transform.SetParent(gameObject.transform);
                item.SetActive(false);
            }
        }
    }
}

[System.Serializable]
public class MwsiveData
{
    public string playlist_name = null;
    public string song_name = null;
    public string album_name = null;
    public string artists = null;
    public string album_image_url = null;
    public string id = null;
    public string uri = null;
    public string preview_url = null;
    public string external_url = null;

    public bool challenge_trackpoints = false;
    public bool challenge_songeded = false;
    public bool challenge_AmILastPosition = false;

    public int total_piks { get; set; }
    public int total_recommendations { get; set; }
    public int? total_piks_followed { get; set; }
    public List<TopCurator> top_curators { get; set; }
    
    public bool isRecommended = false;
    public bool isPicked = false;


}
