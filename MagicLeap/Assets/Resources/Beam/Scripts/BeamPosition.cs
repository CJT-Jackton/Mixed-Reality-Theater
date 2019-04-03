using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamPosition : MonoBehaviour
{
    private GameObject endPos;
    private float startTime;
    private float length;

    public GameObject hitEffect;

    public Vector3 direction;

    public float maxLength;
    public float speed;

    void Start()
    {
        startTime = Time.timeSinceLevelLoad;
        endPos = transform.Find("End Point").gameObject;
        endPos.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount > 0)
        {
            LineRenderer lineRenderer = transform.GetChild(0).gameObject.GetComponent<LineRenderer>() as LineRenderer;
            lineRenderer.SetPosition(0, transform.position);

            length = maxLength;

            if(Time.time - startTime < maxLength / speed)
            {
                length = speed * (Time.time - startTime);
            }

            endPos.transform.localPosition = new Vector3(0, 0, 0);
            endPos.transform.Translate(direction.normalized * length, Space.World);

            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction.normalized, out hit, length))
            {
                lineRenderer.SetPosition(1, hit.point);

                hitEffect.SetActive(true);
                hitEffect.transform.position = hit.point;
                hitEffect.transform.rotation = Quaternion.LookRotation(hit.normal);
            }

            else
            {
                lineRenderer.SetPosition(1, endPos.transform.position);

                hitEffect.SetActive(false);
            }

            GameObject glow = transform.GetChild(1).gameObject;
            glow.transform.localRotation = Quaternion.LookRotation(direction);
        }
    }
}
