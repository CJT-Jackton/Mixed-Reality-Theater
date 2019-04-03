using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamScript : MonoBehaviour
{
    public Transform endPos;
    public LineRenderer lineRenderer;

    public float maxLength;

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, transform.localPosition);
        lineRenderer.SetPosition(1, endPos.localPosition);
    }
}
