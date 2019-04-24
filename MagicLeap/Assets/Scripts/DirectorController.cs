using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.MagicLeap;

public class DirectorController : MonoBehaviour
{
    public GameObject controllerObject;
    public GameObject menuCanvas;

    public DirectorManager DirectorManager;

    private MLInputController _MLInputController;

    private DirectorMenuButton _lastButtonHit;

    private GameObject[] prefabs;
    private Transform _worldOrigin;

    private GameObject grabinGameObject;
    private bool _isGrabbing = false;

    [Space, SerializeField, Tooltip("ControllerConnectionHandler reference.")]
    private ControllerConnectionHandler _controllerConnectionHandler;

    #region Unity Methods

    private void Awake()
    {
        prefabs = GameObject.Find("Network Manager").GetComponent<SpawnManager>().prefabs;
        _worldOrigin = GameObject.Find("World Origin").transform;
    }

    // Use this for initialization
    void Start()
    {
        //MLInput.Start();
        MLInput.OnControllerButtonDown += OnButtonDown;
        MLInput.OnTriggerDown += HandleTriggerDown;
        MLInput.OnTriggerUp += HandleTriggerUp;

        _MLInputController = MLInput.GetController(MLInput.Hand.Left);

        controllerObject.SetActive(true);
        menuCanvas.SetActive(false);
    }

    void OnDestory()
    {
        MLInput.OnControllerButtonDown -= OnButtonDown;
        MLInput.OnTriggerDown -= HandleTriggerDown;
        MLInput.OnTriggerUp -= HandleTriggerUp;
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
            MLInput.OnTriggerDown -= HandleTriggerDown;
            MLInput.OnTriggerUp -= HandleTriggerUp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Controller model transform
        if (_controllerConnectionHandler.IsControllerValid(_MLInputController.Id))
        {
            controllerObject.transform.position = _MLInputController.Position;
            controllerObject.transform.rotation = _MLInputController.Orientation;
        }

        // Controller pointer
        RaycastHit hit;

        if (Physics.Raycast(controllerObject.transform.position, controllerObject.transform.forward, out hit, 10))
        {
            DirectorMenuButton button = hit.transform.GetComponent<DirectorMenuButton>();

            if (button != null)
            {
                if (_lastButtonHit == null)
                {
                    if (button.OnRaycastEnter != null)
                    {
                        button.OnRaycastEnter(hit.point, button.transform);
                    }
                    _lastButtonHit = button;
                }
                else if (_lastButtonHit == button)
                {
                    if (_lastButtonHit.OnRaycastContinue != null)
                    {
                        _lastButtonHit.OnRaycastContinue(hit.point, _lastButtonHit.transform.GetChild(0));
                    }
                }
                else
                {
                    if (_lastButtonHit.OnRaycastExit != null)
                    {
                        _lastButtonHit.OnRaycastExit(hit.point, _lastButtonHit.transform);
                    }
                    _lastButtonHit = null;
                }
            }
            else
            {
                if (_lastButtonHit != null)
                {
                    if (_lastButtonHit.OnRaycastExit != null)
                    {
                        _lastButtonHit.OnRaycastExit(hit.point, _lastButtonHit.transform);
                    }
                    _lastButtonHit = null;
                }
            }
        }
    }

    #endregion

    #region Event Handlers
    /// <summary>
    /// Handles the event for trigger down.
    /// </summary>
    /// <param name="controllerId">The id of the controller.</param>
    /// <param name="triggerValue">The value of the trigger.</param>
    private void HandleTriggerDown(byte controllerId, float triggerValue)
    {
        if (MLImageTracker.IsStarted && _controllerConnectionHandler.IsControllerValid(controllerId))
        {
            RaycastHit hit;

            if (Physics.Raycast(controllerObject.transform.position,
                controllerObject.transform.forward, out hit, 10))
            {
                DirectorMenuButton button =
                    hit.transform.GetComponent<DirectorMenuButton>();

                if (button != null)
                {
                    if (button.OnControllerTriggerDown != null)
                    {
                        button.OnControllerTriggerDown(1.0f, button.PrefabID);
                    }

                    grabinGameObject = Instantiate(prefabs[button.PrefabID]);
                    grabinGameObject.transform.parent = controllerObject.transform;
                    grabinGameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.75f);
                    grabinGameObject.AddComponent<MoveableObject>();

                    DirectorManager.AddObject(grabinGameObject, button.PrefabID);
                }
                else if (hit.transform.GetComponent<MoveableObject>())
                {
                    grabinGameObject = hit.transform.gameObject;
                    grabinGameObject.transform.parent = controllerObject.transform;
                    grabinGameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.75f);
                }
            }

            _isGrabbing = true;
        }
    }

    private void HandleTriggerUp(byte controllerId, float triggerValue)
    {
        if (MLImageTracker.IsStarted && _controllerConnectionHandler.IsControllerValid(controllerId))
        {
            if (_isGrabbing)
            {
                grabinGameObject.transform.parent = _worldOrigin;
            }

            _isGrabbing = false;
        }
    }

    void OnButtonDown(byte controller_id, MLInputControllerButton button)
    {
        if (button == MLInputControllerButton.Bumper)
        {
            menuCanvas.SetActive(true);
            menuCanvas.transform.position = _MLInputController.Position;

            menuCanvas.transform.rotation = Quaternion.Euler(0,Camera.main.transform.rotation.eulerAngles.y,0);

            //menuCanvas.transform.Translate(menuCanvas.transform.forward * 0.5f);
            //menuCanvas.transform.Translate(menuCanvas.transform.right * 0.2f);
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
