using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawableObject : ColorableObject
{
    public Texture2D drawableTexture;
    
    /// <summary>
    /// How much of the surface has to be painted before it is accepted as correct
    /// </summary>
    [Range(0, 1)]
    public float threshold = 0.70f;


    private int size = 128;
    private MeshRenderer meshRenderer;


    protected override void Start()
    {
        base.Start();
        //Setup drawable texture properties
        float grayscale = 0.2f;
        meshRenderer = GetComponent<MeshRenderer>();
        drawableTexture = new Texture2D(size, size);
        drawableTexture.wrapMode = TextureWrapMode.Clamp;

        //Set all pixels to grayscale value
        for (int i = 0; i < drawableTexture.width; i++)
        {
            for (int j = 0; j < drawableTexture.height; j++)
            {
                drawableTexture.SetPixel(i, j, new Color(grayscale, grayscale, grayscale));
            }
        }
        drawableTexture.Apply();

        //Get all the materials on the obj
        var materials = _renderer.materials;
        foreach (var material in materials)
        {
            material.SetTexture("_PaintedTex", drawableTexture);
        }
    }

    /// <summary>
    /// Color target at hitpoint, with color: color
    /// </summary>
    /// <param name="hitPoint"></param>
    /// <param name="color"></param>
    /// <param name="_cam"></param>
    public void ColorTarget(Vector3 hitPoint, Color color, Camera _cam)
    {
        
        Vector3 planeMin = _cam.WorldToScreenPoint(meshRenderer.bounds.min);
        Vector3 planeMax = _cam.WorldToScreenPoint(meshRenderer.bounds.max);

        float xProportion = Mathf.InverseLerp(planeMin.x, planeMax.x, hitPoint.x);
        float yProportion = Mathf.InverseLerp(planeMin.y, planeMax.y, hitPoint.y);

        int xPoint = Mathf.RoundToInt(xProportion * drawableTexture.width);
        int yPoint = Mathf.RoundToInt(yProportion * drawableTexture.height);

        ColorArea(xPoint, yPoint, color);
        Debug.Log(CalculateColorFraction());
        if (canGivePoints && CalculateColorFraction() >= threshold)
        {
            PointSystem.Instance.AddPoints(pointsToGive);
            canGivePoints = false;
        }
    }

    /// <summary>
    /// Color an area around coordinate (x,y) with color: color
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="color"></param>
    private void ColorArea(int x, int y, Color color)
    {
        int r = 15;
        for (int i = x - r; i < x + r; i++)
        {
            //Values outside texture width are discarded
            if (i < 0 || i >= drawableTexture.width)
                continue;

            for (int j = y - r; j < y + r; j++)
            {
                //Values outside texture height are discarded
                if (j < 0 || j >= drawableTexture.height)
                    continue;

                Color currentColor = drawableTexture.GetPixel(i, j);
                drawableTexture.SetPixel(i, j, Color.Lerp(currentColor, color, 0.5f));
            }
        }
        drawableTexture.Apply();
    }
    /// <summary>
    /// Calculate how much of the texture is the correct color
    /// </summary>
    /// <returns></returns>
    private float CalculateColorFraction()
    {
        float tolerance = 0.3f; //Changes how tolerant the system is to the correct color eg 30% off true value;
        var pixels = drawableTexture.GetPixels32();
        int correctPixels = 0;

        for (int i = 0; i < pixels.Length; i++)
        {
            if (Vector4.Magnitude(pixels[i] - desiredColorasColor) <= tolerance)
            {
                correctPixels++;
            }
        }
        return (float)correctPixels / pixels.Length;
    }
}


