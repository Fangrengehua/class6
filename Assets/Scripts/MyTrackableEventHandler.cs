/*==============================================================================
Copyright (c) 2019 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
///
/// Changes made to this file could be overwritten when upgrading the Vuforia version.
/// When implementing custom event handler behavior, consider inheriting from this class instead.
/// </summary>
public class MyTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    public TrackedObject trackedObject;  //目标识别后叠加的虚拟物体

    private Vector3 startPosition; //记录虚拟物体识别前的初始位置
    private Quaternion startRotation; //记录虚拟物体识别前的初始朝向

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
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;
        
        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + 
                  " " + mTrackableBehaviour.CurrentStatus +
                  " -- " + mTrackableBehaviour.CurrentStatusInfo);

        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS


    
    protected virtual void OnTrackingFound()
    {
        if (mTrackableBehaviour)
        {
            print("进入OnTrackingFound方法");
            //首次识别到目标时，将虚拟物体打开
            if (!trackedObject.gameObject.activeSelf)
            {
                trackedObject.gameObject.SetActive(true);
                startPosition = trackedObject.transform.localPosition;
                startRotation = trackedObject.transform.localRotation;
            }
            else
            {
                trackedObject.transform.parent = transform;
                //恢复初始位置
                trackedObject.transform.localPosition = startPosition;
                //恢复初始朝向
                trackedObject.transform.localRotation = startRotation;

                trackedObject.ResetMove();
            }
                
        }
    }


    protected virtual void OnTrackingLost()
    {
        if (mTrackableBehaviour)
        {
            print("进入OnTrackingLost方法");
            //让虚拟物体脱离识别目标
            if (trackedObject.gameObject.activeSelf)
            {
                trackedObject.transform.parent = transform.parent;
                trackedObject.MoveToCenter();
            }
                
        }
    }

    #endregion // PROTECTED_METHODS
}
