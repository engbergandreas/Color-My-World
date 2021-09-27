using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Trigger : MonoBehaviour
{
    public UnityEvent onTriggered;

    public UnityEvent<Vector3> onTriggeredVector;

    private void OnTriggerEnter(Collider other)
    {
        onTriggered.Invoke();
        onTriggeredVector.Invoke(transform.position);
    }
}