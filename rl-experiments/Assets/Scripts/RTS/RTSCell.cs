using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RTSCell : MonoBehaviour
{
    // Event for RTSGrids to subscribe to when a unit enters the cell
    public delegate void UnitEnterAction(int teamID, int row, int col);
    public event UnitEnterAction OnUnitEnter;

    // Event for RTSGrids to subscribe to when a unit exits the cell
    public delegate void UnitExitAction(int teamID, int row, int col);
    public event UnitExitAction OnUnitExit;

    // Event for RTSGrids to subscribe to when a Obstacle enters the cell
    public delegate void ObstacleEnterAction(int row, int col);
    public event ObstacleEnterAction OnObstacleEnter;

    // Event for RTSGrids to subscribe to when a Obstacle exits the cell
    public delegate void ObstacleExitAction(int row, int col);
    public event ObstacleExitAction OnObstacleExit;

    public int row, col;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<RTSUnit>() != null)
        {
            RTSUnit unit = other.GetComponent<RTSUnit>();
            OnUnitEnter?.Invoke(unit.teamID, row, col);
        }
        if (other.GetComponent<NavMeshObstacle>())
        {
            NavMeshObstacle Obstacle = other.GetComponent<NavMeshObstacle>();
            OnObstacleEnter?.Invoke(row, col);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<RTSUnit>() != null)
        {
            RTSUnit unit = other.GetComponent<RTSUnit>();
            OnUnitExit?.Invoke(unit.teamID, row, col);
        }
        if (other.GetComponent<NavMeshObstacle>())
        {
            NavMeshObstacle Obstacle = other.GetComponent<NavMeshObstacle>();
            OnObstacleExit?.Invoke(row, col);
        }
    }
}
