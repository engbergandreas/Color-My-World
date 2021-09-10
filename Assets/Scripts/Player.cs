using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Different channels player & objects can be in, found in player.cs
/// </summary>
public enum RGBChannel
{
    Red, Green, Blue
};

public class Player : MonoBehaviour
{
    public static Player Instance;
    public RGBChannel playerChannel;
    
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
