using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{
    private ZombieStats zombieStats;
    private NavMeshAgent aiAgent;

    private float patrolCountdown;
    private float patrolMaxTimer = 10f;

    private float patrolLookCountdown;
    private float patrolLookMaxTimer = 3f;

    private float patrolRadius = 15f;
    private bool isMoving = false;

    private Quaternion randomLookAxis = Quaternion.AngleAxis(90, Vector3.up);


    // Start is called before the first frame update
    void Start()
    {
        zombieStats = this.GetComponent<ZombieStats>();
        aiAgent = this.GetComponent<NavMeshAgent>();
        // zombieStats.PrintZombieData();
        aiAgent.speed = zombieStats.GetZombieSpeed();
        patrolCountdown = patrolMaxTimer;
    }

    private void Update()
    {
        // print(isMoving);
    }

    public void MoveToTarget(Transform target, float speed, float offset = 0)
    {
        aiAgent.isStopped = false;
        isMoving = true;
        aiAgent.speed = speed;
        aiAgent.stoppingDistance = offset;
        aiAgent.SetDestination(target.position);
    }

    public void StopMoving()
    {
        aiAgent.isStopped = true;
        isMoving = false;
        aiAgent.ResetPath();
        print($"AGENT->>>>> {aiAgent.isStopped}");
    }

    public void PatrolWithoutHorde()
    {
        aiAgent.isStopped = false;
        float dist = aiAgent.remainingDistance;
        // print(aiAgent.remainingDistance);
        patrolCountdown -= Time.deltaTime;
        if (dist != Mathf.Infinity && aiAgent.pathStatus == NavMeshPathStatus.PathComplete && aiAgent.remainingDistance <= 3)
        {
            patrolCountdown -= Time.deltaTime;
            isMoving = false;
            if (patrolCountdown <= 0)
            {
                aiAgent.SetDestination(RandomNavmeshLocation(patrolRadius));
                isMoving = true;
                patrolCountdown = patrolMaxTimer;
            }
            
        }
        RandomLook();
    }

    private Vector3 RandomNavmeshLocation(float radius)
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

    private void RandomLook()
    {
        if (!isMoving)
        {
            patrolLookCountdown -= Time.deltaTime;
            if (patrolLookCountdown <= 0)
            {
                randomLookAxis = Quaternion.AngleAxis(Random.Range(-30f, 30f), Vector3.up);
                patrolLookCountdown = patrolLookMaxTimer;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, randomLookAxis, Time.deltaTime * 1);
        }
    }
}
