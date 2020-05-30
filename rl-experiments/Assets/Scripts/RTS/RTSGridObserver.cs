using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSGridObserver : MonoBehaviour
{
    public int teamID;
    public int numGridsPerSide;

    [SerializeField]
    public List<List<float>> dangerMatrix;

    // Start is called before the first frame update
    void Start()
    {
        dangerMatrix = new List<List<float>>(numGridsPerSide);
        for(int r = 0; r < numGridsPerSide; r++)
        {
            dangerMatrix.Add(new List<float>(numGridsPerSide));
            for (int c = 0; c < numGridsPerSide; c++)
                dangerMatrix[r].Add(0f);
        }

    }

    public void RegisterUnitEnter(int teamID, int row, int col)
    {
        print("RegisterUnitEnter");
        if(teamID == this.teamID)
            dangerMatrix[row][col] += 1;
        else
            dangerMatrix[row][col] -= 1;
        
    }

    public void RegisterUnitExit(int teamID, int row, int col)
    {
        print("RegisterUnitExit");
        if (teamID == this.teamID)
            dangerMatrix[row][col] -= 1;
        else
            dangerMatrix[row][col] += 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
