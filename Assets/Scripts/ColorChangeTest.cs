using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeTest : MonoBehaviour
{
    public int pixels;
    public int correctPixels;
    public int cols = 100;
    public int r = 10;
    public bool continuous = false;
    public float rate = 5.0f;

    private float rateoffire;

    private GameObject _plane;
    public Camera _cam;
    Texture2D texture;
    public Color drawColor;

    void Start()
    {
        _plane = gameObject ;
        texture = new Texture2D(cols, cols);
        GetComponent<Renderer>().material.mainTexture = texture;
        rateoffire = 1 / rate;
        pixels = cols * cols;
    }
    public void calcCorrectPixels()
    {
        var pxls = texture.GetPixels32();
        Debug.Log(pxls.Length);
        Debug.Log(pxls[0]);
        correctPixels = 0;
        for(int i = 0; i < pxls.Length; i++)
        {
            if (pxls[i].r > 250)
                correctPixels++;
        }
        //for(int i =0; i < texture.width; i++)
        //{
        //    for(int j = 0; j <texture.height; j++)
        //    {
        //        if(texture.GetPixel(i,j).r > 0.9f)
        //        {
        //            correctPixels++;
        //        }
        //    }
        //}
    }
    public void FireGun()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray screenPosition = _cam.ScreenPointToRay(mousePosition);
        if (!_plane.GetComponent<Collider>().Raycast(screenPosition, out RaycastHit hit, 1000))
        {
            return;
        }

        Vector3 hitPoint = _cam.WorldToScreenPoint(hit.point);
        Vector3 planeMin = _cam.WorldToScreenPoint(_plane.GetComponent<MeshRenderer>().bounds.min);
        Vector3 planeMax = _cam.WorldToScreenPoint(_plane.GetComponent<MeshRenderer>().bounds.max);
        float xProportion = Mathf.InverseLerp(planeMin.x, planeMax.x, hitPoint.x);
        float yProportion = Mathf.InverseLerp(planeMin.y, planeMax.y, hitPoint.y);
        float xPoint = xProportion * texture.width;
        float yPoint = yProportion * texture.height;
        Vector2 startPosition = new Vector2(xPoint, yPoint);
        
        for (int x = -r; x < r; x++)
        {
            for (int y = -r/2; y < r/2; y++)
            {
                int xcord = texture.width - Mathf.RoundToInt(xPoint) + x;
                int ycord = texture.height - Mathf.RoundToInt(yPoint) + y;
                Color currentColor = texture.GetPixel(xcord, ycord);
                texture.SetPixel(xcord, ycord, Color.Lerp(currentColor, drawColor, 0.5f));
            }
        }

        texture.Apply();
        calcCorrectPixels();
    }

    // Update is called once per frame
    void Update()
    {
        if (continuous)
        {
            if (rateoffire <= 0 && Input.GetMouseButton(0))
            {
                FireGun();
                rateoffire = 1 / rate;
            }
            else
                rateoffire -= Time.deltaTime;
        }else
        {
            if (Input.GetMouseButtonDown(0))
                FireGun();
        }
    }
}
