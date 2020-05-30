using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCell : MonoBehaviour
{
    // Event for RTSGrids to subscribe to when a unit enters the cell
    public delegate void UnitEnterAction(int teamID, int row, int col);
    public event UnitEnterAction OnUnitEnter;

    // Event for RTSGrids to subscribe to when a unit exits the cell
    public delegate void UnitExitAction(int teamID, int row, int col);
    public event UnitExitAction OnUnitExit;

    public int row, col;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<RTSUnit>() != null)
        {
            RTSUnit unit = other.GetComponent<RTSUnit>();
            OnUnitEnter?.Invoke(unit.teamID, row, col);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<RTSUnit>() != null)
        {
            RTSUnit unit = other.GetComponent<RTSUnit>();
            OnUnitExit?.Invoke(unit.teamID, row, col);
        }
    }

}
