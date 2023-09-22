using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    
    private int maxAmmountOfPrefabs = 20;
    public static GameObject prefab;

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
            _prefab.SetActive(false);
            pooledObjects.Add(_prefab);
        }
    }

    // Update is called once per frame
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

}


public class MwsiveData
{
    string playlist_name;
    string song_name;
    string albu_mname;
    string artists;
    string album_image_url;
    string id;
    string uir;
    string preview_url;
    string external_url;

    bool challenge_trackpoints;
    bool challenge_songeded;
    bool challenge_AmILastPosition;

    bool isRecommended;
    bool isPicked;


}
