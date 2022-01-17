using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{
    private ZombieStats zombieStats;
    private NavMeshAgent aiAgent;

    private float patrolCountdown;

    private float patrolLookCountdown;
    private bool isMoving = false;

    private Quaternion randomLookAxis = Quaternion.AngleAxis(90, Vector3.up);


    // Start is called before the first frame update
    void Start()
    {
        zombieStats = this.GetComponent<ZombieStats>();
        aiAgent = this.GetComponent<NavMeshAgent>();
        aiAgent.speed = zombieStats.GetZombieSpeed();
        patrolCountdown = zombieStats.GetZombiePatrolMaxTimer();
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
    }

    public void Patrol()
    {
        aiAgent.isStopped = false;
        float dist = aiAgent.remainingDistance;
        patrolCountdown -= Time.deltaTime;
        if (dist != Mathf.Infinity && aiAgent.pathStatus == NavMeshPathStatus.PathComplete && aiAgent.remainingDistance <= 3)
        {
            patrolCountdown -= Time.deltaTime;
            isMoving = false;
            if (patrolCountdown <= 0)
            {
                aiAgent.SetDestination(RandomNavmeshLocation(zombieStats.GetZombiePatrolRadius()));
                isMoving = true;
                patrolCountdown = zombieStats.GetZombiePatrolMaxTimer();
            }
        }
        RandomLook();
    }

    public void PatrolWithHorde(Vector3 formationPosition)
    {
        aiAgent.isStopped = false;
        aiAgent.SetDestination(formationPosition);
        isMoving = true;
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
                patrolLookCountdown = zombieStats.GetZombiePatrolLookMaxTimer();
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, randomLookAxis, Time.deltaTime * 1);
        }
    }

    public void LookAtTarget(Transform target)
    {
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1);
    }
}
