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

    public RTSGridObserver rtsGridObserver;
    public RTSGridGenerator rtsGridGenerator;

    float lastDist;

    // Update is called before the first frame update
    void Start()
    {
        if (!rtsGridObserver)
        {
            throw new System.NullReferenceException("No RTSGridObserver with matching team ID to " + teamID.ToString() + " ur fukt");
        } 
    }

    public override void OnEpisodeBegin()
    {
        health = healthMax;
        lastDist = 1000f;

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
        AddReward(rewardMax);
        
        EndEpisode();
        // enemyAgent.ReceiveDamage(1);
    }
    

    public override void CollectObservations(VectorSensor sensor)
    {
        if (rtsGridObserver)
            AddMatrixObservation(sensor, rtsGridObserver.megaMatrix);  // Currently 17x17 grid
    }


    private void AddMatrixObservation(VectorSensor sensor, List<List<List<float>>> matrix)
    {
        for(int r = 0; r < rtsGridObserver.numGridsPerSide; r++)
        {
            for (int c = 0; c < rtsGridObserver.numGridsPerSide; c++)
            {
                sensor.AddObservation(matrix[(int) ObservationType.Danger][r][c]);
            }
        }
    }

    public override void OnActionReceived(float[] action)
    {

        int actionType = (int) action[0];
        int destCellIdx = (int) action[1];


        switch (actionType)
        {
            case 0:
                float d = (enemyUnit.position - transform.position).sqrMagnitude;
                if(d < lastDist)
                {
                    AddReward(0.1f);
                }
                lastDist = d;
                break;
            case 1:
                Vector3 dest = rtsGridGenerator.grid1D[destCellIdx].transform.position;
                unit.SetDestination(dest);
                //AddReward(-1f);
                break;
        }

        //int unitSelectIndex = Mathf.Min(Mathf.Max((int)action[2], 0), numUnits - 1);
        //units[unitSelectIndex].GetComponent<NavMeshAgent>.SetDestination();

        AddReward(-rewardMax / MaxStep );
    }
}
