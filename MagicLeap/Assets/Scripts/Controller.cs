using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class Controller : MonoBehaviour
{
    public GameObject controllerObject;
    public GameObject hitObject;

    private MLInputController _MLInputController;

    [Space, SerializeField, Tooltip("ControllerConnectionHandler reference.")]
    private ControllerConnectionHandler _controllerConnectionHandler;

    #region Unity Methods

    // Use this for initialization
    void Start()
    {
        //MLInput.Start();
        MLInput.OnControllerButtonDown += OnButtonDown;
        MLInput.OnTriggerDown += HandleOnTriggerDown;

        _MLInputController = MLInput.GetController(MLInput.Hand.Left);

        controllerObject.SetActive(true);
        hitObject.SetActive(true);
    }

    void OnDestory()
    {
        MLInput.OnControllerButtonDown -= OnButtonDown;
        MLInput.OnTriggerDown -= HandleOnTriggerDown;
        //MLInput.Stop();
    }

    /// <summary>
    /// Cannot make the assumption that a privilege is still granted after
    /// returning from pause. Return the application to the state where it
    /// requests privileges needed and clear out the list of already granted
    /// privileges. Also, unregister callbacks.
    /// </summary>
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            MLInput.OnControllerButtonDown -= OnButtonDown;
            MLInput.OnTriggerDown -= HandleOnTriggerDown;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_controllerConnectionHandler.IsControllerValid(_MLInputController.Id))
        {
            controllerObject.transform.position = _MLInputController.Position;
            controllerObject.transform.rotation = _MLInputController.Orientation;
        }
    }

    #endregion

    #region Event Handlers
    /// <summary>
    /// Handles the event for trigger down.
    /// </summary>
    /// <param name="controllerId">The id of the controller.</param>
    /// <param name="triggerValue">The value of the trigger.</param>
    private void HandleOnTriggerDown(byte controllerId, float triggerValue)
    {
        if (MLImageTracker.IsStarted && _controllerConnectionHandler.IsControllerValid(controllerId))
        {
            controllerObject.transform.Translate(0, 0, -0.01f);

            RaycastHit hit;

            if (Physics.Raycast(controllerObject.transform.position, controllerObject.transform.forward, out hit, 200))
            {
                hitObject.transform.position = hit.point;

                if (hit.rigidbody)
                {
                    hit.rigidbody.constraints = RigidbodyConstraints.None;
                    hit.rigidbody.AddForceAtPosition(controllerObject.transform.forward * 100, hit.point);
                    
                    hit.rigidbody.gameObject.GetComponent<Destroyable>().Hit();
                }
            }
        }
    }

    void OnButtonDown(byte controller_id, MLInputControllerButton button)
    {
        if (button == MLInputControllerButton.Bumper)
        {
            //RaycastHit hit;

            //if (Physics.Raycast(controllerObject.transform.position, controllerObject.transform.forward, out hit, 200))
            //{
            //    GetComponent<ClientManager>().UploadAnchor(hit.point);
            //}
        }
    }

    void OnButtonUp(byte controller_id, MLInputControllerButton button)
    {
        if (button == MLInputControllerButton.HomeTap)
        {
            //Application.Quit();
        }
    }
    #endregion
}
