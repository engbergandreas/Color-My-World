using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeTest : MonoBehaviour
{
    public int cols = 20;
    public int x1 = 5;
    public int y1 = 5;

    private GameObject _plane;
    public Camera _cam;
    Texture2D texture;
    public Color drawColor;
    void Start()
    {
        _plane = gameObject ;
       texture = new Texture2D(cols, cols);
        GetComponent<Renderer>().material.mainTexture = texture;



    }

    // Update is called once per frame
    void Update()
    {
        float scale = 256.0f / cols;

        //for (int y = 0; y < texture.height; y++)
        //{
        //    for (int x = 0; x < texture.width; x++)
        //    {
        //        //Color color = ((x & y) != 0 ? Color.white : Color.gray);
        //        //texture.SetPixel(x, y, color);
        //        Color color = new Color(0, 0, 0);
        //        texture.SetPixel(x, y, color);
        //    }
        //}

        texture.SetPixel(1, 1, Color.white);
        texture.SetPixel(1, 2, Color.white);

        texture.SetPixel(x1, y1, Color.white);


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

        Debug.Log(startPosition);
        int r = 15;
        for (int x = -r; x < r; x++)
        {
            for (int y = -r; y < r; y++)
            {
                int xcord = texture.width - Mathf.RoundToInt(xPoint) + x;
                int ycord = texture.height - Mathf.RoundToInt(yPoint) + y;
                Color currentColor = texture.GetPixel(xcord, ycord);
                texture.SetPixel(xcord, ycord, Color.Lerp(currentColor, drawColor, 0.5f));
            }
        }

        texture.Apply();
    }
}
