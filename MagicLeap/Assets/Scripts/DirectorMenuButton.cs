using UnityEngine;
using UnityEngine.XR.MagicLeap;

[RequireComponent(typeof(Collider))]
[DisallowMultipleComponent]
public class DirectorMenuButton : MonoBehaviour
{
    #region Public Events
    public System.Action<Vector3, Transform> OnRaycastEnter;
    public System.Action<Vector3, Transform> OnRaycastContinue;
    public System.Action<Vector3, Transform> OnRaycastExit;
    public System.Action<MLInputControllerButton> OnControllerButtonDown;
    public System.Action<MLInputControllerButton> OnControllerButtonUp;
    public System.Action<float, int> OnControllerTriggerDown;
    public System.Action<float, int> OnControllerTriggerUp;
    public System.Action<MLInputController> OnControllerDrag;
    #endregion

    #region Public Variables

    [Tooltip("Placement object's Prefab ID")]
    public int PrefabID;

    #endregion

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Public Methods

    public void Enlarge()
    {
        transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
    }

    public void ResetScale()
    {
        transform.localScale = Vector3.one;
    }
    #endregion
}
