using System;
using System.Collections.Generic;
using UnityEngine;

public class RTSGridObserver : MonoBehaviour
{
    public int teamID;
    public int numGridsPerSide;

    [SerializeField]

    public List<List<List<float>>> megaMatrix;



    // Start is called before the first frame update
    void Start()
    {
        megaMatrix = new List<List<List<float>>>(2); //bigbrain
        int obsTypeCount = Enum.GetNames(typeof(ObservationType)).Length;

        for (int i = 0; i < obsTypeCount; i++)
        {
            megaMatrix.Add(new List<List<float>>(numGridsPerSide));
            for (int r = 0; r < numGridsPerSide; r++)
            {
                megaMatrix[i].Add(new List<float>(numGridsPerSide));
                for (int c = 0; c < numGridsPerSide; c++)
                    megaMatrix[i][r].Add(0f);
            }
        }

    }

    public void RegisterUnitEnter(int teamID, int row, int col)
    {
        print("RegisterUnitEnter");
        if (teamID == this.teamID)
            megaMatrix[(int)ObservationType.Danger][row][col] += 1;
        else
            megaMatrix[(int)ObservationType.Danger][row][col] -= 1;
        
    }

    public void RegisterUnitExit(int teamID, int row, int col)
    {
        print("RegisterUnitExit");
        if (teamID == this.teamID)
            megaMatrix[(int)ObservationType.Danger][row][col] -= 1;
        else
            megaMatrix[(int)ObservationType.Danger][row][col] += 1;
    }

    public void RegisterObstacleEnter(int row, int col)
    {
        print("RegisterObstacleEnter");
        megaMatrix[(int)ObservationType.Obstacle][row][col] = 1;
    }

    public void RegisterObstacleExit(int row, int col)
    {
        print("RegisterObstacleExit");
        megaMatrix[(int)ObservationType.Obstacle][row][col] = 0;
    }
}
