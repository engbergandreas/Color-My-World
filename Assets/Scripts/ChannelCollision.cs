using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Subclass of colorable object? -> direct access to desired color 
public class ChannelCollision : MonoBehaviour
{
    public RGBChannel canWalkThroughInChannel;

    private BoxCollider col;

    private void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    public void OnChannelChange(RGBChannel newChannel)
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
