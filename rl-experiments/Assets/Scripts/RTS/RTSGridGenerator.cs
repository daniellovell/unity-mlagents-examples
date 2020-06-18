using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RTSGridGenerator : MonoBehaviour
{
    public int numTeams = 2;
    public int length = 15;
    public int numGridsPerSide = 32;

    private List<List<BoxCollider>> colliderGrid;
    private GameObject _parentObject;
    public List<GameObject> grid1D;
    public List<RTSMasterAgent> masterAgents;

    public void GenerateGrid()
    {
        if(grid1D.Count > 0)
        {
            DeleteGrid();
        }

        _parentObject = new GameObject("ColliderGrid");
        _parentObject.transform.parent = transform;
        _parentObject.transform.localPosition = Vector3.zero;


        // The parent object of all the cells will contain the same number 
        // of RTSGridObserver components as there are teams 
        // We save a list of them so that we can subscribe their functions to the cell's events later
        List<RTSGridObserver> rtsGridObs = new List<RTSGridObserver>();
        for (int i = 0; i < numTeams; i++)
        {
            RTSGridObserver rG = _parentObject.AddComponent<RTSGridObserver>();
            rG.teamID = i;
            rG.numGridsPerSide = numGridsPerSide;
            rtsGridObs.Add(rG);

            if (rG.teamID == masterAgents[i].teamID)
                masterAgents[i].rtsGridObserver = rG;
            else
                throw new System.Exception("MasterAgent and GridObserver order mismatch! " +
                    "Add MasterAgent in correct order to GridGenerator!");
            
        }

        colliderGrid = new List<List<BoxCollider>>(numGridsPerSide);

        float dGrid = length / (float)numGridsPerSide;

        for (int r = 0; r < numGridsPerSide; r++)
        {
            colliderGrid.Add(new List<BoxCollider>(numGridsPerSide));
            for (int c = 0; c < numGridsPerSide; c++)
            {
                GameObject go = new GameObject("Cell [" + r.ToString() + "," + c.ToString() + "]");
                go.transform.parent = _parentObject.transform;
                go.transform.position = new Vector3(-(length / 2f) + (dGrid / 2f) + (dGrid * r), 0f, -(length / 2f) + (dGrid / 2f) + (dGrid * c));
                go.AddComponent<BoxCollider>().size = new Vector3(dGrid, 30f, dGrid);
                go.GetComponent<BoxCollider>().isTrigger = true;

                RTSCell goCell = go.AddComponent<RTSCell>();
                goCell.row = r;
                goCell.col = c;

                colliderGrid[r].Add(go.GetComponent<BoxCollider>());

                goCell.rtsGridObservers = rtsGridObs.ToArray();

                grid1D.Add(go);
            }
        }
    }

    public void DeleteGrid()
    {
        if (_parentObject == null)
        {
            return;
        }

        grid1D.Clear();
        grid1D = new List<GameObject>();

        colliderGrid.Clear();
        colliderGrid = new List<List<BoxCollider>>();

        DestroyImmediate(_parentObject);
    }

}
