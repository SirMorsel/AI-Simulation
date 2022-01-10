using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{
    private ZombieStats zombieStats;
    private NavMeshAgent aiAgent;

    private float patrolCountdown;
    private float patrolMaxTimer = 3f;

    private float patrolRadius = 8f;

    // Start is called before the first frame update
    void Start()
    {
        zombieStats = this.GetComponent<ZombieStats>();
        aiAgent = this.GetComponent<NavMeshAgent>();
        // zombieStats.PrintZombieData();
        aiAgent.speed = zombieStats.GetZombieSpeed();
        patrolCountdown = patrolMaxTimer;
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
        aiAgent.ResetPath();
        print($"AGENT->>>>> {aiAgent.isStopped}");
    }

    public void PatrolWithoutHorde()
    {
        aiAgent.isStopped = false;
        float dist = aiAgent.remainingDistance;
        // print(aiAgent.remainingDistance);
        if (dist != Mathf.Infinity && aiAgent.pathStatus == NavMeshPathStatus.PathComplete && aiAgent.remainingDistance <= 3)
        {
            patrolCountdown -= Time.deltaTime;
            if (patrolCountdown <= 0)
            {
                aiAgent.SetDestination(RandomNavmeshLocation(patrolRadius));
                patrolCountdown = patrolMaxTimer;
            }
            
        }
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
