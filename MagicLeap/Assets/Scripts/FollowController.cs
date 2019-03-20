using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class FollowController : MonoBehaviour
{
    public GameObject hitObject;
    private MLInputController _MLInputController;

    // Use this for initialization
    void Start()
    {
        MLInput.Start();
        MLInput.OnControllerButtonDown += OnButtonDown;
        MLInput.OnControllerButtonUp += OnButtonUp;

        _MLInputController = MLInput.GetController(MLInput.Hand.Left);
        gameObject.SetActive(true);
    }

    void OnDestory()
    {
        MLInput.OnControllerButtonDown -= OnButtonDown;
        MLInput.OnControllerButtonUp -= OnButtonUp;
        MLInput.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _MLInputController.Position;
        transform.rotation = _MLInputController.Orientation;

        if (_MLInputController.TriggerValue == 1)
        {
            transform.Translate(0, 0, -0.01f);

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 200))
            {
                hitObject.transform.position = hit.point;

                if (hit.rigidbody)
                {
                    hit.rigidbody.constraints = RigidbodyConstraints.None;
                    hit.rigidbody.AddForceAtPosition(transform.forward * 10, hit.point);
                }
            }
        }
    }

    void OnButtonDown(byte controller_id, MLInputControllerButton button)
    {
        if (button == MLInputControllerButton.Bumper)
        {
            hitObject.transform.position = _MLInputController.Position;
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
