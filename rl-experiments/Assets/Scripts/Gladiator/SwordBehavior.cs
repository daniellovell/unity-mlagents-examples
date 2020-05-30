using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    public bool isHitReady;

    private GladiatorAgent _parentAgent;

    // Start is called before the first frame update
    void Start()
    {
        isHitReady = true;
        _parentAgent = GetComponentInParent<GladiatorAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // This will only register a hit on hitting the actual gladiator agent.
        // Does not count if we hit the shield or the sword
        if(other.tag == "Hitbox")
        {
            GladiatorAgent agentWeHit = other.gameObject.GetComponentInParent<GladiatorAgent>();

            if (agentWeHit != _parentAgent)
            {
                if (isHitReady) {
                    _parentAgent.HitEnemy(agentWeHit);
                    isHitReady = false;
                    StartCoroutine(HitResetRoutine());
                }
                else
                {
                    _parentAgent.AddReward(-0.01f); // Add negative reward for hitting when not ready
                }
            }
        }
    }

    IEnumerator HitResetRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        isHitReady = true;
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
