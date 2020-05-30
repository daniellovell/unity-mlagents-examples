using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

// flow = true;

public class CubeAgent : Agent
{
    public Transform enemyBall;
    Rigidbody rBody;
    float lastDist;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        if (this.transform.localPosition.y < 0)
        {
            // If the Agent fell, zero its momentum
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        lastDist = enemyBall.localPosition.magnitude;
        // Start both in a random position
        transform.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
        enemyBall.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(enemyBall.localPosition); // 3 observations per Vector 3
        sensor.AddObservation(this.transform.localPosition); // 3 observations per Vector 3

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x); // 1 observation
        sensor.AddObservation(rBody.velocity.z); // 1 observation
        sensor.AddObservation(rBody.angularVelocity); // 3

        sensor.AddObservation(enemyBall.localPosition.magnitude); //1 

        sensor.AddObservation(enemyBall.GetComponent<Rigidbody>().velocity.x); // 1
        sensor.AddObservation(enemyBall.GetComponent<Rigidbody>().velocity.z); // 1

        // 14
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }

    public float speed = 10;
    public override void OnActionReceived(float[] action)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = action[0]; // Force in x
        controlSignal.z = action[1]; // Force in y

        // Apply the action to the env
        //transform.Translate(Vector3.forward * controlSignal.x * Time.deltaTime * speed);
        //transform.Translate(Vector3.right * controlSignal.z * Time.deltaTime * speed);

        rBody.AddForce(controlSignal * speed);

        float distanceToTarget = Vector3.Distance(this.transform.localPosition, enemyBall.localPosition);

        float dist = enemyBall.localPosition.magnitude;
        if(dist > lastDist)
        {
            SetReward(0.1f);
        }

        lastDist = dist;

        if (enemyBall.localPosition.y < 0.0f && transform.localPosition.y >= 0.0f)
        {
            SetReward(5.0f);
            EndEpisode();
        }
        else if(enemyBall.localPosition.y < 0.0f && transform.localPosition.y < 0.0f){
            SetReward(1.0f);
            EndEpisode();
        }


        SetReward(-0.1f);
    }



}
