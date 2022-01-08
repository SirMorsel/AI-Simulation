using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{
    private ZombieStats zombieStats;
    private NavMeshAgent aiAgent;

    // Start is called before the first frame update
    void Start()
    {
        zombieStats = this.GetComponent<ZombieStats>();
        aiAgent = this.GetComponent<NavMeshAgent>();
        // zombieStats.PrintZombieData();
        aiAgent.speed = zombieStats.GetZombieSpeed();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveToTarget(Transform target, float speed, float offset = 0)
    {
        aiAgent.isStopped = false;
        aiAgent.speed = speed;
        aiAgent.stoppingDistance = offset;
        aiAgent.SetDestination(target.position);
    }

    public void StopMoving()
    {
        aiAgent.isStopped = true;
        print($"AGENT->>>>> {aiAgent.isStopped}");
    }
}
