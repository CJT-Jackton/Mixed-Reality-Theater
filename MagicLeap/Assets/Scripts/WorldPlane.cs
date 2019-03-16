using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class WorldPlane : MonoBehaviour
{

    public Transform BBoxTransform;
    public Vector3 BBoxExtents;
    public GameObject PlanePrefab;

    [BitMask(typeof(MLWorldPlanesQueryFlags))]
    public MLWorldPlanesQueryFlags QueryFlags;

    private float timeout = 5f;
    private float timeSinceLastRequest = 0f;

    private MLWorldPlanesQueryParams _queryParams = new MLWorldPlanesQueryParams();
    private List<GameObject> _planeCache = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        MLWorldPlanes.Start();
        PlanePrefab.SetActive(false);
    }

    private void OnDestroy()
    {
        MLWorldPlanes.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastRequest += Time.deltaTime;
        if (timeSinceLastRequest > timeout)
        {
            timeSinceLastRequest = 0f;
            RequestPlanes();
        }
    }

    void RequestPlanes()
    {
        _queryParams.Flags = QueryFlags;
        _queryParams.MaxResults = 100;
        _queryParams.BoundsCenter = BBoxTransform.position;
        _queryParams.BoundsRotation = BBoxTransform.rotation;
        _queryParams.BoundsExtents = BBoxExtents;

        MLWorldPlanes.GetPlanes(_queryParams, OnReceivedPlanes);
    }

    private void OnReceivedPlanes(MLResult result, MLWorldPlane[] planes, MLWorldPlaneBoundaries[] boundaries)
    {
        for (int i = _planeCache.Count - 1; i >= 0; --i)
        {
            Destroy(_planeCache[i]);
            _planeCache.Remove(_planeCache[i]);
        }

        GameObject newPlane;
        for (int i = 0; i < planes.Length; ++i)
        {
            newPlane = Instantiate(PlanePrefab);
            newPlane.SetActive(true);
            newPlane.transform.position = planes[i].Center;
            newPlane.transform.rotation = planes[i].Rotation;
            newPlane.transform.localScale = new Vector3(planes[i].Width, planes[i].Height, 1f);
            _planeCache.Add(newPlane);
        }
    }
}
