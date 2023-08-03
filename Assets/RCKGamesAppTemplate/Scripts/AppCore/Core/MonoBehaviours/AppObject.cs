using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppObject : MonoBehaviour
{
    public virtual void Initialize(params object[] list) { }

    public virtual void SetActive(bool value) { this.gameObject.SetActive(value); }

    protected void AddEventListener<T>(EventManager.EventDelegate<T> listener) where T : AppEvent
    {
        EventManager.instance.AddListener<T>(listener);
    }

    protected void InvokeEvent<T>(T e) where T : AppEvent
    {
        EventManager.instance.InvokeEvent<T>(e);
    }

    protected void RemoveEventListener<T>(EventManager.EventDelegate<T> listener) where T : AppEvent
    {
        EventManager.instance.RemoveListener<T>(listener);
    }

    protected virtual bool CheckDeviceNetworkReachability()
    {
        return (Application.internetReachability != NetworkReachability.NotReachable);
    }
}
