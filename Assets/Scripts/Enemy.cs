using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : DrawableObject
{
    public Transform[] patrolPoints;
    public float SearchRadius = 5.0f;

    


    private NavMeshAgent agent;
    private int destPoint;
    private bool isHunting = false;

    protected override void Start()
    {
        base.Start();
        agent = GetComponentInParent<NavMeshAgent>();

        GoToNextPatrolPoint();
    }

    protected override void FullyColored()
    {
        base.FullyColored();
        Destroy(gameObject); //TODO: enemy death animation or other thing

    }

    
    private void GoToNextPatrolPoint()
    {
        // Set the agent to go to the currently selected destination.
        agent.destination = patrolPoints[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % patrolPoints.Length;

    }

    private void Update()
    {
        //TODO draw gizmo / visualize search radius
        //TODO kill, hurt player when close
        float playerDistance = FindPlayerDistanceSqr();
        if (playerDistance <= SearchRadius)
        {
            agent.destination = Player.Instance.transform.position;
            isHunting = true;
        }
        else
        {
            if(isHunting)
            {
                GoToNextPatrolPoint();
                isHunting = false;
            }
        }

        
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GoToNextPatrolPoint();

        //rotates plane towarads camera when using NavMeshAgent
        transform.eulerAngles = new Vector3(90, 180, -agent.transform.rotation.y);
    }

    private float FindPlayerDistanceSqr()
    {
        return Vector3.Magnitude(Player.Instance.transform.position - agent.transform.position);
    }
}
