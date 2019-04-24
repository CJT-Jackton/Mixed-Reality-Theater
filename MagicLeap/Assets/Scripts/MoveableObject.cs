using UnityEngine;

[RequireComponent(typeof(Collider))]
[DisallowMultipleComponent]
public class MoveableObject : MonoBehaviour
{
    #region Public Events
    public System.Action<Vector3, Transform> OnRaycastEnter;
    public System.Action<Vector3, Transform> OnRaycastContinue;
    public System.Action<Vector3, Transform> OnRaycastExit;
    #endregion

    #region Private Variables

    private Renderer renderer;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        renderer = GetComponent<Renderer>();

        OnRaycastEnter += ShowFrame;
        OnRaycastExit += HideFrame;
    }

    /// <summary>
    /// Align the object with gravity.
    /// </summary>
    void Update()
    {
        //transform.localRotation = Quaternion.FromToRotation(transform.up, Vector3.up);
    }

    private void OnDestroy()
    {
        OnRaycastEnter -= ShowFrame;
        OnRaycastExit -= HideFrame;
    }

    #endregion

    #region Public Methods

    public void ShowFrame(Vector3 vector3, Transform transform)
    {
        renderer.enabled = true;
    }

    public void HideFrame(Vector3 vector3, Transform transform)
    {
        renderer.enabled = false;
    }

    #endregion
}
