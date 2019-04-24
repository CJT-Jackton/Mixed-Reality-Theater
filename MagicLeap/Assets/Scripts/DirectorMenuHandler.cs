using UnityEngine;
using UnityEngine.UI;

public class DirectorMenuHandler : MonoBehaviour
{
    public GameObject Canvas;

    public DirectorMenuButton CloseButton;

    public DirectorMenuButton[] SpawnButton;

    private GameObject newInstantiate;

    // Use this for initialization
    private void Awake()
    {
        CloseButton.OnControllerTriggerDown += CloseMenu;

        CloseButton.OnRaycastEnter += OnHover;
        CloseButton.OnRaycastExit += OnLostFocus;

        foreach (DirectorMenuButton button in SpawnButton)
        {
            button.OnRaycastEnter += OnHover;
            button.OnRaycastContinue += OnFocus;
            button.OnRaycastExit += OnLostFocus;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        CloseButton.OnControllerTriggerDown -= CloseMenu;

        CloseButton.OnRaycastEnter -= OnHover;
        CloseButton.OnRaycastExit -= OnLostFocus;

        foreach (DirectorMenuButton button in SpawnButton)
        {
            button.OnRaycastEnter -= OnHover;
            button.OnRaycastContinue -= OnFocus;
            button.OnRaycastExit -= OnLostFocus;
        }
    }

    private void OnHover(Vector3 point, Transform bTransform)
    {
        bTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        bTransform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
    }

    private void OnLostFocus(Vector3 point, Transform bTransform)
    {
        bTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        bTransform.localScale = Vector3.one;
    }

    private void OnFocus(Vector3 point, Transform bTransform)
    {
        bTransform.RotateAround(bTransform.position, bTransform.forward, 1.0f);
    }

    private void CloseMenu(float triggerReading, int prefabID)
    {
        Canvas.SetActive(false);
    }
}
