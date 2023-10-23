using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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
    public GameObject UIMessageCanvas;

    private List<GameObject> messageList = new List<GameObject>();
    public List<GameObject> messageLastPosition = new List<GameObject>();
    public List<string> messageBufferList = new List<string>();

    private void Start()
    {
        SpawnMessageList();
        SpawnMessageList();
        SpawnMessageList();

    }

    private void SpawnMessageList()
    {
        GameObject Instance = Instantiate(MessagePF, Vector3.zero, Quaternion.identity);
        Instance.SetActive(false);
        Instance.transform.SetParent(GameObject.Find("UIMessage_Manager").transform);
        Instance.transform.localScale = new Vector3(1, 1, 1);
        Instance.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        Instance.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

        messageList.Add(Instance);
        Instance.name = "Message" + messageList.Count;
    }

    private GameObject GetPooledObject()
    {
        for (int i = 0; i < messageList.Count; i++)
        {
            if (!messageList[i].activeInHierarchy)
            {
                return messageList[i];
            }
        }
        return null;
    }

    public Vector3 ReturnLastPosition(int i)
    {
        return messageLastPosition[i].transform.position;
    }

    public void CheckList()
    {
        if(messageBufferList.Count > 0)
        {
            UIMessageInstanciate(messageBufferList[0]);
            messageBufferList.RemoveAt(0);
        }    
    }

    public void UIMessageInstanciate(string _text) {

        GameObject Instance = GetPooledObject();

        if (Instance != null)
        {
            try
            {
                Instance.GetComponent<UIMessage_Ani>().textMeshPro.text = _text;
                Instance.GetComponent<UIMessage_Ani>().Play_MessageAnimation();
            }
            catch (System.NullReferenceException e)
            {
                Debug.Log(e);
                Instance.GetComponent<UIMessage_Ani>().textMeshPro.text = " ";
                Instance.GetComponent<UIMessage_Ani>().Play_MessageAnimation();
            }
        }   
        else
        {
            messageBufferList.Add(_text);
        }

    }
}
