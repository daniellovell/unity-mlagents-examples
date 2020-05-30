using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RTSUnit : MonoBehaviour
{ 
    public int teamID;
    public int health;
    public RTSMasterAgent parentMasterAgent;

    [HideInInspector]
    public NavMeshAgent navAgent;

    private void OnDrawGizmos()
    {
        if(navAgent)
            Gizmos.DrawWireSphere(navAgent.destination, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        RTSUnit otherUnit = other.GetComponent<RTSUnit>();

        if (otherUnit && otherUnit.teamID != teamID)
        {
            parentMasterAgent.HitEnemy(otherUnit);
        }
    }

    public void SetDestination(Vector3 destination)
    {
        print(destination.x.ToString() + "   " + destination.z.ToString());
        navAgent.SetDestination(destination);
    }

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }
}
                             