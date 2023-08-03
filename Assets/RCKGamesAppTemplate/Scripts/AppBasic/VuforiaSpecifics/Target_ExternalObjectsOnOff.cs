/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Target_ExternalObjectsOnOff : MonoBehaviour, ITrackableEventHandler
{
    public GameObject[] objectsToDeactivate_WhenScanned;
    public GameObject[] objectsToActivate_WhenScanned;


    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES


    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
        }
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED || newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            // Se encontro un target, desactivamos los objetos que queremos que desaparezcan con este target
            for(int i =0;i< objectsToDeactivate_WhenScanned.Length; i++)
            {
                objectsToDeactivate_WhenScanned[i].SetActive(false);
            }

            // Se encontro un target, activamos los objetos que queremos que aparezcan con este target
            for (int i = 0; i < objectsToActivate_WhenScanned.Length; i++)
            {
                objectsToActivate_WhenScanned[i].SetActive(true);
            }
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED && newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            // No hay target o se perdio el target, reactivamos los objetos que habiamos desactivado
            for (int i = 0; i < objectsToDeactivate_WhenScanned.Length; i++)
            {
                objectsToDeactivate_WhenScanned[i].SetActive(true);
            }

            // No hay target o se perdio el target, desactivamos los objetos que activamos anteriormente
            for (int i = 0; i < objectsToActivate_WhenScanned.Length; i++)
            {
                objectsToActivate_WhenScanned[i].SetActive(false);
            }
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            for (int i = 0; i < objectsToDeactivate_WhenScanned.Length; i++)
            {
                objectsToDeactivate_WhenScanned[i].SetActive(true);
            }

            for (int i = 0; i < objectsToActivate_WhenScanned.Length; i++)
            {
                objectsToActivate_WhenScanned[i].SetActive(false);
            }
        }
    }

    #endregion // PUBLIC_METHODS

}
*/