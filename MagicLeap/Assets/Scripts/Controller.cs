using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class Controller : MonoBehaviour
{
    public GameObject controllerObject;
    public GameObject hitObject;

    private MLInputController _MLInputController;

    // Use this for initialization
    void Start()
    {
        MLInput.Start();
        MLInput.OnControllerButtonDown += OnButtonDown;

        _MLInputController = MLInput.GetController(MLInput.Hand.Left);

        controllerObject.SetActive(true);
        hitObject.SetActive(true);
    }

    void OnDestory()
    {
        MLInput.OnControllerButtonDown -= OnButtonDown;
        MLInput.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        controllerObject.transform.position = _MLInputController.Position;
        controllerObject.transform.rotation = _MLInputController.Orientation;

        if (_MLInputController.TriggerValue > 0.5f)
        {
            controllerObject.transform.Translate(0, 0, -0.01f);

            RaycastHit hit;

            if (Physics.Raycast(controllerObject.transform.position, controllerObject.transform.forward, out hit, 200))
            {
                hitObject.transform.position = hit.point;

                if (hit.rigidbody)
                {
                    hit.rigidbody.constraints = RigidbodyConstraints.None;
                    hit.rigidbody.AddForceAtPosition(controllerObject.transform.forward * 5, hit.point);
                    Destroy(hit.rigidbody.gameObject, 3);
                }
            }
        }
    }

    void OnButtonDown(byte controller_id, MLInputControllerButton button)
    {
        if (button == MLInputControllerButton.Bumper)
        {
            RaycastHit hit;

            if (Physics.Raycast(controllerObject.transform.position, controllerObject.transform.forward, out hit, 200))
            {
                GetComponent<ClientManager>().UploadAnchor(hit.point);
            }
        }
    }

    void OnButtonUp(byte controller_id, MLInputControllerButton button)
    {
        if (button == MLInputControllerButton.HomeTap)
        {
            Application.Quit();
        }
    }
}
