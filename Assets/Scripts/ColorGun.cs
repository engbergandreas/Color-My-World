using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ColorGun : MonoBehaviour
{
    public static ColorGun Instance;
    public Color color;
    public float fireRate = 10.0f;
    public bool continiousFire;
    public Image crosshair;
    public Transform fireGunPosition;
    public List<Texture2D> maskList;
    public UnityEvent<RGBChannel> rgbChannelEvent;
    public UnityEvent<Color> colorChannelEvent;

    private Camera _cam;
    private float fireTimer;

    //private LineRenderer lr;

    private bool hasRed = false, hasGreen = false, hasBlue = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _cam = Camera.main;
        fireTimer = 0;
        Cursor.visible = false; //make cursor pointer invisible
        //lr = GetComponent<LineRenderer>();
        //lr.startWidth = 0.3f;
        //lr.endWidth = 0.3f;
    }
    // Update is called once per frame
    void Update()
    {
        if (hasRed && Input.GetKeyDown(KeyCode.Alpha1))
            ChangeColorGun(RGBChannel.Red);
        if (hasGreen && Input.GetKeyDown(KeyCode.Alpha2))
            ChangeColorGun(RGBChannel.Green);
        if (hasBlue && Input.GetKeyDown(KeyCode.Alpha3))
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

        RectTransform crosshairRec = crosshair.GetComponent<RectTransform>();
        Vector3 screenMousePos = Input.mousePosition;
        crosshairRec.position = screenMousePos;

        CheckIntersectingObjectsBetweenPlayerAndMouse();
    }
    /// <summary>
    /// Raycast from the player towards the mouse to se if we intercept any drawable objects
    /// Crosshair alpha value set to 1 if we are hovering a drawable object, otherwise lower value.
    /// </summary>
    private void CheckIntersectingObjectsBetweenPlayerAndMouse()
    {
        crosshair.color = new Color(0, 0, 0, 0.2f);
        crosshair.rectTransform.eulerAngles = new Vector3(0, 0, 0);


        //lr.SetPosition(0, Vector3.zero);
        //lr.SetPosition(1, Vector3.zero);
        //lr.startColor = Color.black;
        //lr.endColor = Color.black;

        if (!Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit rayMouseToWorld))
            return;
        Vector3 hitPoint = rayMouseToWorld.point;
        Vector3 dir = hitPoint - fireGunPosition.position;

        //lr.SetPosition(0, fireGunPosition.position);
        //lr.SetPosition(1, hitPoint);

        if (!Physics.Raycast(fireGunPosition.position, dir.normalized, out RaycastHit rayPlayerToMouse))
            return;
        
        var interceptedObj = rayPlayerToMouse.transform.GetComponent<DrawableObject>();
        if(interceptedObj)
        {
            crosshair.color = new Color(0, 0, 0, 1);
            float deg = 60.0f;
            float speed = 6.0f;
            crosshair.rectTransform.eulerAngles = new Vector3(0, 0, Mathf.Sin(Time.time * speed) * deg );
            //lr.startColor = Color.blue;
            //lr.endColor = Color.blue;
        }
    }

    /// <summary>
    /// Fires gun from player to mouse location 
    /// </summary>
    private void FireGun()
    {
        fireTimer = 1 / fireRate;

        Vector3 mousePosition = Input.mousePosition;
        Ray screenPosition = _cam.ScreenPointToRay(mousePosition);
        if(!Physics.Raycast(screenPosition, out RaycastHit hitinfo, 50))
        {
            return;
        }
        //Check we hit drawable target
        GameObject hitTarget = hitinfo.transform.gameObject;
        DrawableObject objectHit = hitTarget.GetComponent<DrawableObject>();
        if (objectHit)
        {
            //Fire a ray from the player towards the mouse in world coordinates
            //check if the ray intersects any other objects on the path
            Vector3 worldHitPoint = hitinfo.point;
            Vector3 dir = worldHitPoint - fireGunPosition.position;
            if (!Physics.Raycast(fireGunPosition.position, dir.normalized, out RaycastHit info))
                return;

            DrawableObject interception = info.transform.gameObject.GetComponent<DrawableObject>();
            if (interception && interception == objectHit) //Is it the same drawable object that we clicked on then fire the gun
            {
                Vector3 hitPoint = _cam.WorldToScreenPoint(hitinfo.point);
                objectHit.ColorTarget(hitPoint, color, _cam, GetRandomSplatterMask());
            }
        }
    }

    Texture2D GetRandomSplatterMask()
    {
        
        return maskList[Random.Range(0, maskList.Count)];
    }

    //TODO: broadcast onchanged to other objects in a better way?
    //UnityEvent<RGBChannel> <Color>
    private void ChangeColorGun(RGBChannel channel)
    {
        color = RGBChannelToColor(channel);
        //var colorableObjects = FindObjectsOfType<ColorableObject>();
        //foreach(var obj in colorableObjects)
        //{
        //    obj.OnChangedColorGun(color);
        //}

        //var channelCollisions= FindObjectsOfType<ChannelCollision>();
        //foreach(var obj in channelCollisions)
        //{
        //    obj.OnChannelChange(channel);
        //}

        rgbChannelEvent.Invoke(channel);
        colorChannelEvent.Invoke(color);
    }

    public Color RGBChannelToColor(RGBChannel channel)
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
  
    public void PickUpColor(RGBChannel color)
    {
        switch (color)
        {
            case RGBChannel.Red:
                hasRed = true;
                break;
            case RGBChannel.Green:
                hasGreen = true;
                break;
            case RGBChannel.Blue:
                hasBlue = true;
                break;
            default:
                break;
        }
    }
}
