using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ViewModelType
{
    public string viewModelType;
    public GameObject viewModelPrefab;
}

[CreateAssetMenu(fileName = "ViewModelType", menuName = "ScriptableObjects/ViewModelTypeScriptableObject", order = 1)]
public class SpawnableViewModelTypesScriptableObject : ScriptableObject
{
    public List<ViewModelType> viewTypes = new List<ViewModelType>();
}
