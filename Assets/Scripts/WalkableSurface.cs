using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkableSurface : DrawableObject
{
    private MeshCollider col;

    protected override void Start()
    {
        base.Start();
        col = GetComponent<MeshCollider>();
        col.convex = true;
        col.isTrigger = true;
    }

    protected override void FullyColored()
    {
        col.isTrigger = false;
        staticColor = true;
        ShowTrueColor();

    }

    protected override void OnPartiallyColored()
    {
        col.isTrigger = true;
        staticColor = false;
        ShowRGBColor();
    }



}
