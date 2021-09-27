using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharger : MonoBehaviour
{
    public float chargeUpTime = 2.5f;
    public float force = 2;

    private Rigidbody rb;    

    public float timer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        timer = chargeUpTime;
    }

    private void Update()
    {
        if (timer <= 0)
        {
            rb.AddForce(transform.right * force, ForceMode.Impulse);
            timer = chargeUpTime;
            transform.Rotate(Vector3.up, 180, Space.Self);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
