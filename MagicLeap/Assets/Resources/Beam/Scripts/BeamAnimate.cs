using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAnimate : MonoBehaviour
{

    public float speed;

    private new LineRenderer renderer;
    private Material material;

    // Use this for initialization
    void Start()
    {
        renderer = gameObject.GetComponent<LineRenderer>() as LineRenderer;
        material = renderer.material;
        material.mainTexture.wrapMode = TextureWrapMode.Repeat;
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset += new Vector2(-Time.deltaTime * speed, 0);
    }
}
