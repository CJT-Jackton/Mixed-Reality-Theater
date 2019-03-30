using UnityEngine;
using UnityEngine.XR.MagicLeap;

[RequireComponent(typeof(PrivilegeRequester))]
public class CoordinateCalibration : MonoBehaviour
{
    #region Public Variables

    [Tooltip("The origin of the world")]
    public Transform WorldOrigin;

    #endregion

    #region Private Variables

    private PrivilegeRequester _privilegeRequester;
    private MLImageTrackerBehavior _imageTracker;
    private bool _calibrated = false;

    #endregion

    #region Unity Methods
    // Using Awake so that Privileges is set before PrivilegeRequester Start
    void Awake()
    {
        // If not listed here, the PrivilegeRequester assumes the request for
        // the privileges needed, CameraCapture in this case, are in the editor.
        _privilegeRequester = GetComponent<PrivilegeRequester>();

        // Before enabling the MLImageTrackerBehavior GameObjects, the scene must wait until the privilege has been granted.
        _privilegeRequester.OnPrivilegesDone += HandlePrivilegesDone;

        _imageTracker = GetComponent<MLImageTrackerBehavior>();
    }

    void OnDestroy()
    {
        _imageTracker.OnTargetFound -= OnTargetFound;
        
        if (_privilegeRequester != null)
        {
            _privilegeRequester.OnPrivilegesDone -= HandlePrivilegesDone;
        }
    }

    #endregion

    #region Event Handlers
    /// <summary>
    /// Responds to privilege requester result.
    /// </summary>
    /// <param name="result"/>
    void HandlePrivilegesDone(MLResult result)
    {
        if (!result.IsOk)
        {
            if (result.Code == MLResultCode.PrivilegeDenied)
            {
                Instantiate(Resources.Load("PrivilegeDeniedError"));
            }

            Debug.LogErrorFormat("Error: CoordinateCalibration failed to get requested privileges, disabling script. Reason: {0}", result);
            enabled = false;
            return;
        }

        Debug.Log("Succeeded in requesting all privileges");

        // Start image tracking
        _imageTracker.OnTargetFound += OnTargetFound;
    }

    /// <summary>
    /// Callback for when tracked image is found
    /// </summary>
    /// <param name="isReliable"> Contains if image found is reliable </param>
    private void OnTargetFound(bool isReliable)
    {
        if (!_calibrated && isReliable)
        {
            WorldOrigin.position = transform.position;
            WorldOrigin.rotation = transform.rotation;

            _calibrated = true;

            if (MLImageTracker.IsStarted)
            {
                MLImageTracker.Disable();
            }

            Debug.Log("Image target found, calibration completed.");
        }
    }

    #endregion
}
