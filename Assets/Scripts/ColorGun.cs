using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGun : MonoBehaviour
{
    public Color color;
    public float fireRate = 10.0f;
    public bool continiousFire;

    private Camera _cam;
    private float fireTimer;

    private void Start()
    {
        _cam = Camera.main;
        fireTimer = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeColorGun(RGBChannel.Red);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeColorGun(RGBChannel.Green);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeColorGun(RGBChannel.Blue);


        if (fireTimer <= 0)
        {
            if (continiousFire && Input.GetMouseButton(0)) //can hold down mouse
                FireGun();
            else if (Input.GetMouseButtonDown(0)) //single taps only
                FireGun();
        }
        else
        {
            fireTimer -= Time.deltaTime;
        }
    }

    public void FireGun()
    {
        fireTimer = 1 / fireRate;

        Vector3 mousePosition = Input.mousePosition;
        Ray screenPosition = _cam.ScreenPointToRay(mousePosition);
        if(!Physics.Raycast(screenPosition, out RaycastHit hitinfo, 50))
        {
            return;
        }
        GameObject hitTarget = hitinfo.transform.gameObject;
        Vector3 hitPoint = _cam.WorldToScreenPoint(hitinfo.point);
        //TODO: check correct obj
        hitTarget.GetComponent<DrawableObject>().ColorTarget(hitPoint, color, _cam);

    }

    //TODO: broadcast onchanged to other objects in a better way?
    private void ChangeColorGun(RGBChannel channel)
    {
        color = RGBChannelToColor(channel);
        var colorableObjects = FindObjectsOfType<ColorableObject>();
        foreach(var obj in colorableObjects)
        {
            obj.OnChangedColorGun(color);
        }

        var channelCollisions= FindObjectsOfType<ChannelCollision>();
        foreach(var obj in channelCollisions)
        {
            obj.OnChannelChange(channel);
        }
    }

    private Color RGBChannelToColor(RGBChannel channel)
    {
        switch (channel)
        {
            case RGBChannel.Red:
                return Color.red;
            case RGBChannel.Green:
                return Color.green;
            case RGBChannel.Blue:
                return Color.blue;
            default:
                return Color.black;
        }
    }
}
