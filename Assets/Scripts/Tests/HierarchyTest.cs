using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HierarchyTest : MonoBehaviour
{
    public GameObject prefab, container, target;
    // Start is called before the first frame update
    void Start()
    {
        GameObject Instance;
        Instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        Instance.transform.SetParent(container.transform);
        Debug.Log(target.transform.GetSiblingIndex());
        Instance.transform.SetSiblingIndex(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
