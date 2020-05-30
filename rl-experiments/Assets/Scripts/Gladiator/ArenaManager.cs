using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public GladiatorAgent blueAgent;
    public GladiatorAgent redAgent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnGladiatorDeath(GladiatorAgent deadAgent)
    {
        if(blueAgent == deadAgent)
        {
            blueAgent.AddReward(-10.0f);
            redAgent.AddReward(10.0f);
        }
        else
        {
            redAgent.AddReward(-10.0f);
            blueAgent.AddReward(10.0f);
        }

        // All agents need to reset
        blueAgent.EndEpisode();
        redAgent.EndEpisode();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
