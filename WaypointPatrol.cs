using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    int m_CurrentWaypointIndex;
    public float speed;

    void Start()
    {
        navMeshAgent.SetDestination(waypoints[0].position);
        InvokeRepeating("speedRandomizer", 0f, 6f);
    }

    void Update()
    {
        navMeshAgent.speed = speed;


        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination (waypoints[m_CurrentWaypointIndex].position);
        }
    }

    void speedRandomizer()
    {
        speed = Random.Range(0.8f, 3);
        UnityEngine.Debug.Log(speed);
    }
}

//Waypoints are assigned index numbers and ghosts will follow waypoints in order of index. They'll  have a path from 1 to 2 to 3, etc..

//Baking the NavMesh doesn't work on my computer, so ghosts won't move for me. If ghosts give you issues try re-baking the NavMesh

//all is well now... navmesh saved...