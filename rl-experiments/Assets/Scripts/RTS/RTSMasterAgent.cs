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
    public RTSUnit enemyUnit;
    public int actionSpaceScale;


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
        enemyUnit.transform.localPosition = new Vector3(Random.value * 14 - 7, 0.0f, Random.value * 14 - 7);
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
        AddReward(2.0f);
        // enemyAgent.ReceiveDamage(1);
    }
    

    public override void CollectObservations(VectorSensor sensor)
    {
        if (rtsGridObserver)
            AddMatrixObservation(sensor, rtsGridObserver.dangerMatrix);  // Currently 17x17 grid
    }
    

    private void AddMatrixObservation(VectorSensor sensor, List<List<float>> matrix)
    {
        foreach(List<float> lf in matrix)
        {
            foreach(float f in lf)
            {
                sensor.AddObservation(f);
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

        AddReward(-0.002f);
    }
}
