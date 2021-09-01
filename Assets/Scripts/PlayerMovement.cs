using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 15.0f;

    //TODO: better movement controls
    void Update()
    {
        Vector3 movementdir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        movementdir *= movementSpeed * Time.deltaTime;

        transform.Translate(movementdir);
    }
}
