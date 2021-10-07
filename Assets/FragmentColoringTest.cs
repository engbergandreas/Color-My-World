using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentColoringTest : MonoBehaviour
{
    Renderer _renderer;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        var materials = _renderer.materials;
        foreach (var material in materials)
        {
            //Enable shader keywords so they can be changed later
            material.EnableKeyword("_WorldMousePosition");
        }
    }

    private void Update()
    {

        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit rayMouseToWorld))
            return;

        Vector3 hitPoint = rayMouseToWorld.point;

        var material = _renderer.material;
        material.SetVector("_WorldMousePosition", new Vector4(hitPoint.x, hitPoint.y, hitPoint.z));
        //material.SetColor("_Color", ShaderColorMultiplier);
    }

}
