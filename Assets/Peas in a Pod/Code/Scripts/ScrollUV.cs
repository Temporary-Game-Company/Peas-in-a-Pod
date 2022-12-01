using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUV : MonoBehaviour
{
    private MeshRenderer _mr;

    private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        _mr = GetComponent<MeshRenderer>();
        rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Material mat = _mr.materials[0];

        Vector2 Offset = mat.mainTextureOffset;

        Offset.x += Time.deltaTime * 0.01f;

        mat.mainTextureOffset = Offset;

        transform.rotation = rotation;
    }
}
