using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.AI;

public class RTSMasterAgent : Agent
{    
    public int teamID;

    public int health;
    public int healthMax = 10;

    public RTSUnit unit;
    public Transform enemyUnit;
    public int actionSpaceScale;
    public Transform obstacle;

    public float rewardMax;

    [HideInInspector]
    public RTSGridObserver rtsGridObserver;

    // Update is called before the first frame update
    void Start()
    {
        RTSGridObserver[] gridObservers = FindObjectsOfType<RTSGridObserver>();
        foreach(RTSGridObserver rgo in gridObservers)
        {
            if(rgo.teamID == this.teamID)
            {
                rtsGridObserver = rgo;
                break;
            }
        }
        if (!rtsGridObserver)
        {
            throw new System.NullReferenceException("No RTSGridObserver with matching team ID to " + teamID.ToString() + " ur fukt");
        }
    }

    public override void OnEpisodeBegin()
    {
        health = healthMax;

        unit.transform.localPosition = new Vector3(Random.value * 14 - 7, 0.0f, Random.value * 14 - 7);
        enemyUnit.localPosition = new Vector3(Random.value * 14 - 7, 0.0f, Random.value * 14 - 7);
        obstacle.transform.localPosition = new Vector3(Random.value * 14 - 7, 0.0f, Random.value * 14 - 7);
        obstacle.transform.localRotation = Quaternion.Euler(0, Random.value * 360, 0);

    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }

    /*
    public void ReceiveDamage(int damage)
    {
        health -= damage;
        AddReward(-2.0f);
        if (health <= 0)
        {
            print("I DIED");
        }
    }
    */

    public void HitEnemy(RTSUnit enemyUnit)
    {
        print("I HIT HIM");
        AddReward(rewardMax);
        
        EndEpisode();
        // enemyAgent.ReceiveDamage(1);
    }
    

    public override void CollectObservations(VectorSensor sensor)
    {
        if (rtsGridObserver)
            AddMatrixObservation(sensor, rtsGridObserver.megaMatrix);  // Currently 17x17 grid
                //AddMatrixObservation(sensor, rtsGridObserver.obstacleMatrix);
    }


    private void AddMatrixObservation(VectorSensor sensor, List<List<List<float>>> matrix)
    {
        for(int r = 0; r < rtsGridObserver.numGridsPerSide; r++)
        {
            for (int c = 0; c < rtsGridObserver.numGridsPerSide; c++)
            {
                sensor.AddObservation(matrix[(int) ObservationType.Danger][r][c]);
                sensor.AddObservation(matrix[(int) ObservationType.Obstacle][r][c]);
            }
        }
    }

    public override void OnActionReceived(float[] action)
    {
        RTSGridGenerator rtsGridGen;

        int actionType = (int) action[0];
        int destCellIdx = (int) action[1];


        switch (actionType)
        {
            case 1:
                rtsGridGen = FindObjectOfType<RTSGridGenerator>();
                unit.SetDestination(rtsGridGen.grid1D[destCellIdx].transform.position);
                print("Destination Index: " + destCellIdx.ToString());
                break;
        }

        //int unitSelectIndex = Mathf.Min(Mathf.Max((int)action[2], 0), numUnits - 1);
        //units[unitSelectIndex].GetComponent<NavMeshAgent>.SetDestination();

        AddReward(-rewardMax / MaxStep );
    }
}
