using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorableObject : MonoBehaviour
{
    private int size = 128;
    private MeshRenderer meshRenderer;
    public Texture2D texture;

    private Color ShaderColorMultiplier;

    public void ColorTarget(Vector3 hitPoint, Color color, Camera _cam)
    {
        Vector3 planeMin = _cam.WorldToScreenPoint(meshRenderer.bounds.min);
        Vector3 planeMax = _cam.WorldToScreenPoint(meshRenderer.bounds.max);

        float xProportion = Mathf.InverseLerp(planeMin.x, planeMax.x, hitPoint.x);
        float yProportion = Mathf.InverseLerp(planeMin.y, planeMax.y, hitPoint.y);

        int xPoint = Mathf.RoundToInt(xProportion * texture.width);
        int yPoint = Mathf.RoundToInt(yProportion * texture.height);

        ColorArea(xPoint, yPoint, color);
    }
    
    // Color an area around coordinate (x,y) with color 
    private void ColorArea(int x, int y, Color color)
    {
        int r = 15;
        for (int i = x - r; i < x + r; i++)
        {
            //values outside texture width 
            if (i < 0 || i >= texture.width)
                continue;

            for (int j = y - r; j < y + r; j++)
            {
                if (j < 0 || j >= texture.height)
                    continue;

                Color currentColor = texture.GetPixel(i,j);
                texture.SetPixel(i, j, Color.Lerp(currentColor, color, 0.5f));
            }
        }
        texture.Apply();
    }
    // Start is called before the first frame update
    void Start()
    {
        float grayscale = 0.2f;
        meshRenderer = GetComponent<MeshRenderer>();
        texture = new Texture2D(size, size);
        texture.wrapMode = TextureWrapMode.Clamp;
        for(int i =0; i < texture.width; i++)
        {
            for(int j = 0; j < texture.height; j++)
            {
                texture.SetPixel(i, j, new Color(grayscale, grayscale, grayscale));
            }
        }
        texture.Apply();
        
        //GetComponent<Renderer>().material.mainTexture = texture;
        Renderer _renderer = GetComponent<Renderer>();
        _renderer.material.EnableKeyword("_PaintedTex");
        _renderer.material.SetTexture("_PaintedTex", texture);

    }


    //TODO: find more optimal way to change all textures color in the shader
    public void OnChangedColorGun(Color color)
    {
        ShaderColorMultiplier = color;
        Renderer _renderer = GetComponent<Renderer>();
        _renderer.material.EnableKeyword("_Color");
        _renderer.material.SetColor("_Color", ShaderColorMultiplier);

}
