using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;


public class RaceAgentRaycast : Agent
{
    Rigidbody rb;

    public Transform finishTransform;
    public Transform obstacleTransform;


    // Update is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // If the Agent fell, zero its momentum
        this.rb.angularVelocity = Vector3.zero;
        this.rb.velocity = Vector3.zero;
        transform.localPosition = new Vector3(Random.value * 40 - 20, 1.0f, Random.value * 40 - 20);
        finishTransform.localPosition = new Vector3(Random.value * 40 - 20, 1.0f, Random.value * 40 - 20);
        obstacleTransform.localPosition = new Vector3(Random.value * 40 - 20, 1.0f, Random.value * 40 - 20);
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Finish")
        {
            SetReward(10.0f);
            EndEpisode();
        }
    }

    public float speed = 10;
    public override void OnActionReceived(float[] action)
    {
        // Actions, size = 2
        Vector3 forwardToGo = Vector3.zero;
        Vector3 rightToGo = Vector3.zero;
        int forwardAxis = (int)action[0]; // Force in x
        int rightAxis  = (int)action[1]; // Force in y

        switch (forwardAxis)
        {
            case 1:
                forwardToGo = transform.forward * speed;
                break;
            case 2:
                forwardToGo = transform.forward * -speed;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                rightToGo = transform.right * speed;
                break;
            case 2:
                rightToGo = transform.right * -speed;
                break;
        }

        // Apply the action to the env (i.e. move)
        transform.Translate(forwardToGo * Time.deltaTime);
        transform.Translate(rightToGo * Time.deltaTime);

        SetReward(-1.0f);
    }
}
