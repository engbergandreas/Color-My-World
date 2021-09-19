using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Subclass of colorable object? -> direct access to desired color instead of having its own
//TODO: visualize when its possible to walk through or not

public class ChannelCollision : MonoBehaviour
{
    public RGBChannel canWalkThroughInChannel;

    private BoxCollider col;

    private void Start()
    {
        col = GetComponent<BoxCollider>();
        ColorGun.Instance.rgbChannelEvent.AddListener(OnChannelChange);
    }

    private void OnChannelChange(RGBChannel newChannel)
    {
        if(newChannel == canWalkThroughInChannel)
        {
            col.isTrigger = true;
        }else
        {
            col.isTrigger = false;
        }
    }
}
