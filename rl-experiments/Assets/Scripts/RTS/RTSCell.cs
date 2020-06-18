using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RTSCell : MonoBehaviour
{

    public RTSGridObserver[] rtsGridObservers;
    public int row, col;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<RTSUnit>() != null || other.tag == "Finish")
        {
            int teamID;
            if (other.tag == "Finish")
                teamID = 1;
            else
            {
                RTSUnit unit = other.GetComponent<RTSUnit>();
                teamID = unit.teamID;
            }

            foreach(RTSGridObserver ro in rtsGridObservers)
            {
                ro.RegisterUnitEnter(teamID, row, col);
            }
        }
        if (other.GetComponent<NavMeshObstacle>())
        {
            NavMeshObstacle Obstacle = other.GetComponent<NavMeshObstacle>();
            foreach (RTSGridObserver ro in rtsGridObservers)
            {
                ro.RegisterObstacleEnter(row, col);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<RTSUnit>() != null || other.tag == "Finish")
        {
            int teamID;
            if (other.tag == "Finish")
                teamID = 1;
            else
            {
                RTSUnit unit = other.GetComponent<RTSUnit>();
                teamID = unit.teamID;
            }

            foreach (RTSGridObserver ro in rtsGridObservers)
            {
                ro.RegisterUnitExit(teamID, row, col);
            }
        }
        if (other.GetComponent<NavMeshObstacle>())
        {
            NavMeshObstacle Obstacle = other.GetComponent<NavMeshObstacle>();
            foreach (RTSGridObserver ro in rtsGridObservers)
            {
                ro.RegisterObstacleExit(row, col);
            }
        }
    }
}
