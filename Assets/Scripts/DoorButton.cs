using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DoorButton : MonoBehaviour
{

    public float timeUntilClosed = 5;

    public GameObject door;

    public DrawableObject button;

    private bool open = false;
    private float timer = 0;
    private BoxCollider doorcollider;
    private void Start()
    {
        doorcollider = door.GetComponent<BoxCollider>();
        button._event.AddListener(OnFullyColoredButton);
    }
    public void OnFullyColoredButton()
    {
        WalkThrough(true);
        timer = timeUntilClosed;
    }

    private void Update()
    {
        if(open)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                WalkThrough(false);
                button.ResetToOriginal();
            }
        }
    }


    public void WalkThrough(bool status)
    {
        doorcollider.isTrigger = status;
        open = status;
    }


}
