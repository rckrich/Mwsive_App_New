using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScrollViewModel : ViewModel
{
    [Header("Instanced Object References")]
    public Transform instanceParentTransform;
    public GameObject instancePrefabReference;
    [Header("Object Reference")]
    public GameObject nothingOnListMessageReference;
    public GameObject instancesGroupReference;

    protected List<Instanceable> instanceables = new List<Instanceable>();
    protected bool isInitialized = false;

    public override void Initialize<TV, TP, TI>(TV _viewModel, object[] _list)
    {
        base.Initialize<TV, TP, TI>(_viewModel, _list);
        StartInstanceProcess();
    }

    public virtual void StartInstanceProcess()
    {
        CallPresenter();
    }

    public virtual void InstanceObjects<TInstanceable>(List<Instanceable> _instanceables) where TInstanceable : InstanceableAppObject
    {
        if (_instanceables.Count <= 0 || _instanceables == null)
        {
            SetActiveMessageNoProduct(true);
            return;
        }

        foreach(Instanceable instanceable in _instanceables)
        {
            GameObject instanceableAppObject = Instantiate(instancePrefabReference, instanceParentTransform);
            InstanceableAppObject instanceableAppObjectComponent = instanceableAppObject.GetComponent<TInstanceable>();
            instanceableAppObjectComponent.Initialize(instanceable);
        }

        CallWaitAFrame();

        SetActiveMessageNoProduct(instanceables.Count <= 0 || instanceables == null);

        isInitialized = true;
    }

    public virtual void DeleteProductObjects()
    {
        for (int i = 0; i < instanceParentTransform.childCount; i++)
        {
            if (!instanceParentTransform.GetChild(i).GetComponent<InstanceableAppObject>().KeepWhenDestroyed)
                Destroy(instanceParentTransform.GetChild(i).gameObject);
        }
    }

    public virtual void SetActiveMessageNoProduct(bool _value)
    {
        if(nothingOnListMessageReference != null)
            nothingOnListMessageReference.SetActive(_value);
        if(instancesGroupReference != null)
            instancesGroupReference.SetActive(!_value);
    }

    public virtual void ReloadActivitiesFeed()
    {
        if (!CheckDeviceNetworkReachability())
        {
            CallErrorPopUp();
            return;
        }

        if (isInitialized)
        {
            DeleteProductObjects();
            StartInstanceProcess();
        }
    }
}
