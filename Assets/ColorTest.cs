using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColorTest : MonoBehaviour
{
    public Camera cam;

    Vector3[] vertices;
    Mesh mesh;
    Color[] colors;
    public float radius = 0.2f;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        colors = new Color[vertices.Length];
    }
    // Update is called once per frame
    void Update()
    {
        vertices = mesh.vertices;

        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit rayMouseToWorld))
            return;

        Vector3 hitPoint = rayMouseToWorld.point;
        Matrix4x4 localToWorld = transform.localToWorldMatrix;

        for (int i = 0; i < vertices.Length; ++i)
        {
            Vector3 vertexWorldPos = localToWorld.MultiplyPoint3x4(vertices[i]);
            
            if(Vector3.Magnitude(vertexWorldPos - hitPoint) < radius )
            {
                colors[i] = new Color(1, 0, 0);
            }
        }

        mesh.vertices = vertices;
        mesh.colors = colors;
    }
}
