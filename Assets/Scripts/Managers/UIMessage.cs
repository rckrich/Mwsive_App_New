using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMessage : MonoBehaviour
{
    private static UIMessage _instance;
        public static UIMessage instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UIMessage>();
            }
            return _instance;
        }
    }

    public GameObject MessagePF;

    private void Start()
    {
    }



    public void UIMessageInstanciate(string _text){
        GameObject Instance  = Instantiate(MessagePF, Vector3.zero, Quaternion.identity);
        Instance.SetActive(false);
        Instance.transform.SetParent(GameObject.Find("SpawnableCanvas").transform);
        Instance.transform.localScale = new Vector3(1,1,1);
        Instance.GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
        Instance.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);
        Instance.GetComponentInChildren<TextMeshProUGUI>().text = _text;


        UIAniManager.instance.UIMessage(Instance);
    }
    

}
