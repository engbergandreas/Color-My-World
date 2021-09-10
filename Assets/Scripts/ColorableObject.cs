using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorableObject : MonoBehaviour
{
    public RGBChannel desiredColor;
    public int pointsToGive = 10;
    public bool showTrueColorOnCorrectChannel = false;

    protected Color desiredColorasColor;  
    protected bool canGivePoints = true;
    protected Color ShaderColorMultiplier;
    protected Renderer _renderer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        switch (desiredColor)
        {
            case RGBChannel.Red:
                desiredColorasColor = Color.red;
                break;
            case RGBChannel.Green:
                desiredColorasColor = Color.green;

                break;
            case RGBChannel.Blue:
                desiredColorasColor = Color.blue;
                break;
            default:
                break;
        }

        //GetComponent<Renderer>().material.mainTexture = texture;
        _renderer = GetComponent<Renderer>();
        //Get all the materials on the obj
        var materials = _renderer.materials;
        foreach (var material in materials)
        {
            //Enable shader keywords so they can be changed later
           material.EnableKeyword("_PaintedTex");
           material.EnableKeyword("_Color");

           //material.SetTexture("_PaintedTex", drawableTexture);
        }

    }
    
    //TODO: find more optimal way to change all textures color in the shader
    public void OnChangedColorGun(Color color)
    {
        ShaderColorMultiplier = color;
        //Renderer _renderer = GetComponent<Renderer>();
        //_renderer.material.SetColor("_Color", ShaderColorMultiplier);
        
        var materials = _renderer.materials;
        foreach (var material in materials)
        {
            if (color == desiredColorasColor && showTrueColorOnCorrectChannel)
                material.SetColor("_Color", Color.white);
            else 
                material.SetColor("_Color", ShaderColorMultiplier);
        }

    }
}
