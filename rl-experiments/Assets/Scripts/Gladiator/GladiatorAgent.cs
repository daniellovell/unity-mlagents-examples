using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;


public class GladiatorAgent : Agent
{
    public ArenaManager arenaManager;
    public SwordBehavior swordBehavior;
    Rigidbody rb;

    public int health;
    public int healthMax = 10;

    public float m_LateralSpeed;
    public float m_ForwardSpeed;

    public float m_BurstSpeed;
    public float m_AngularVelocity;
    public float m_SpeedMultiplier;
    public bool burstAvailable;

    // Update is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        swordBehavior = GetComponentInChildren<SwordBehavior>();
        burstAvailable = true;
    }

    public override void OnEpisodeBegin()
    {
        health = healthMax;

        // If the Agent fell, zero its momentum
        this.rb.angularVelocity = Vector3.zero;
        this.rb.velocity = Vector3.zero;
        transform.localPosition = new Vector3(Random.value * 10 - 5, 0.0f, Random.value * 10 - 5);
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }

    public void ReceiveDamage(int damage)
    {
        health -= damage;
        AddReward(-2.0f);
        if(health <= 0)
        {
            print("I DIED");
            arenaManager.OnGladiatorDeath(this);
        }
    }

    public void HitEnemy(GladiatorAgent enemyAgent)
    {
        print("I HIT HIM");
        AddReward(2.0f);
        enemyAgent.ReceiveDamage(1);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(burstAvailable);
        sensor.AddObservation(health);
        sensor.AddObservation(swordBehavior.isHitReady);
    }

    public override void OnActionReceived(float[] action)
    {
        // Actions, size = 2
        Vector3 forwardToGo = Vector3.zero;
        Vector3 rightToGo = Vector3.zero;
        Vector3 rotateDir = Vector3.zero;
        Vector3 burstToGo = Vector3.zero;

        int forwardAxis = (int)action[0]; // Force in x
        int rightAxis = (int)action[1]; // Force in y
        var rotateAxis = (int)action[2];
        int burst = (int)action[3];

        switch (forwardAxis)
        {
            case 1:
                forwardToGo = transform.forward * m_ForwardSpeed;
                break;
            case 2:
                forwardToGo = transform.forward * -m_ForwardSpeed;
                break;
        }
        switch (rightAxis)
        {
            case 1:
                rightToGo = transform.right * m_LateralSpeed;
                break;
            case 2:
                rightToGo = transform.right * -m_LateralSpeed;
                break;
        }
        switch (rotateAxis)
        {
            case 1:
                rotateDir = transform.up * -1f;
                break;
            case 2:
                rotateDir = transform.up * 1f;
                break;
        }
        switch (burst)
        {
            case 1:
                burstToGo = transform.right * m_BurstSpeed;
                break;
            case 2:
                burstToGo = transform.right * -m_BurstSpeed;
                break;
            case 3:
                burstToGo = transform.forward * m_BurstSpeed;
                break;
            case 4:
                burstToGo = transform.forward * -m_BurstSpeed;
                break;
        }

        Vector3 dirToGo;
        if (burst > 0 && burstAvailable)
        {
            dirToGo = burstToGo;
            burstAvailable = false;
            StartCoroutine(BurstResetRoutine());
        }
        else
            dirToGo = forwardToGo + rightToGo;

        // Apply the action to the env (i.e. move)
        transform.Rotate(rotateDir, Time.deltaTime * m_AngularVelocity);
        rb.AddForce(dirToGo * m_SpeedMultiplier, ForceMode.VelocityChange);

        AddReward(-0.002f);
    }

    IEnumerator BurstResetRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        burstAvailable = true;
        yield break;
    }
}
