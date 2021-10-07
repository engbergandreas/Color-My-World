using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 15.0f;
    public float jumpStrength = 50.0f;

    private CapsuleCollider col;

    private Rigidbody rb;
    private bool onLadder = false;

    private int layerMask;

    private Actions playerActions;
    private bool isRunning;


    Vector3 movementdir;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        playerActions = GetComponent<Actions>();

        if (!rb)
            rb = GetComponentInChildren<Rigidbody>();
        if (!col)
            col = GetComponentInChildren<CapsuleCollider>();
        layerMask = ~LayerMask.GetMask("Player");

    }

    public void OnEnterLadder()
    {
        onLadder = true;
        rb.useGravity = false;
    }
    public void OnExitLadder()
    {
        onLadder = false;
        rb.useGravity = true;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(movementdir.x * movementSpeed * Time.deltaTime, rb.velocity.y, rb.velocity.z);
        if(onLadder)
        {
            rb.velocity = new Vector3(rb.velocity.x, movementdir.y * movementSpeed * Time.deltaTime, rb.velocity.z);

        }
    }
    //TODO: better movement controls
    void Update()
    {
        movementdir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        Vector3 rot;
        if (!onLadder)
        {
            rot = new Vector3(0, 180 - movementdir.x * 90, 0);
        }else
        {
            rot = new Vector3(0, 0, 0);
        }
        transform.eulerAngles = rot;

        if (Mathf.Abs(movementdir.x) > 0)
            Run();
        else {
            playerActions.Stay();
            isRunning = false;  
        }

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            playerActions.Jump();
            isRunning = false;
        }
    }

    private void Run()
    {
        if (!isRunning)
        {
            isRunning = true;
            playerActions.Run();
        }
    }

    //TODO: bug, infinte jump
    private bool IsGrounded()
    {
        return Physics.CheckSphere(new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z), col.radius * 1.05f, layerMask);      
    }
}
