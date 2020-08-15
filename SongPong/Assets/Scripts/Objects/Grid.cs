using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private MeshRenderer mesh_renderer;
    public Vector3 size;

    // Start is called before the first frame update
    void Awake()
    {
        mesh_renderer = GetComponentInChildren<MeshRenderer>();
        size = Vector3.Scale(mesh_renderer.bounds.size, transform.localScale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
