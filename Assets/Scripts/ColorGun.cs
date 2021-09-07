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
            ChangeColorGun(Color.red);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeColorGun(Color.green);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeColorGun(Color.blue);


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
        hitTarget.GetComponent<ColorableObject>().ColorTarget(hitPoint, color, _cam);

    }

    private void ChangeColorGun(Color newColor)
    {
        color = newColor;
        //TODO: broadcast onchanged to colorableobjects
        var objs = FindObjectsOfType<ColorableObject>();
        foreach(var obj in objs)
        {
            obj.OnChangedColorGun(newColor);
        }
    }
}
