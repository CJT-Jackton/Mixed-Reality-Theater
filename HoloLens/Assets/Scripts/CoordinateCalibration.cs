using UnityEngine;
using UnityEngine.XR.WSA;
using Vuforia;

public class CoordinateCalibration : DefaultTrackableEventHandler
{
    public Transform WorldOrigin;
    public Transform ImageTarget;

    #region PROTECTED_METHODS

    protected override void OnTrackingFound()
    {
        Debug.Log("Image found!");

        WorldOrigin.position = ImageTarget.position;
        WorldOrigin.rotation = ImageTarget.rotation;

        WorldOrigin.gameObject.AddComponent<WorldAnchor>();

        TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();
    }

    #endregion
}
