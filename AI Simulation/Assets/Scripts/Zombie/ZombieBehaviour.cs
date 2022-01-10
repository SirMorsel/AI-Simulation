using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviour : MonoBehaviour
{
    private ZombieDetectionSenses zombieDetectionSenses;
    private ZombieMovement zombieMovement;
    private ZombieStats zombieStats;

    private EnumZombieBehaviour currentBehaviour = EnumZombieBehaviour.IDLE;

    private float searchCountdown;
    private float detectionCountdown;

    // move to scriptable object
    private float searchRateMaxTime = 10f;
    private float detectionRateMaxTime = 5f;
    private float maxSenseRange = 10f;

    // Start is called before the first frame update
    void Start()
    {
        zombieDetectionSenses = this.GetComponent<ZombieDetectionSenses>();
        zombieMovement = this.GetComponent<ZombieMovement>();
        zombieStats = this.GetComponent<ZombieStats>();

        searchCountdown = searchRateMaxTime;
        detectionCountdown = detectionRateMaxTime;
        
    }

    private void Update()
    {
        StateMachine();
    }

    // refactor state function (like CaB)
    public void ChangeStateOfZombie(EnumZombieBehaviour state, Transform target)
    {
        switch (state)
        {
            case EnumZombieBehaviour.IDLE:
                break;
            case EnumZombieBehaviour.SEARCH:
                zombieMovement.MoveToTarget(target, zombieStats.GetZombieSpeed(), 3f);
                break;
            case EnumZombieBehaviour.DETECT:
                print("Player is visible!"); // Start countdown for chasing (co-routine)
                
                break;
            case EnumZombieBehaviour.CHASE:
                break;
            case EnumZombieBehaviour.ATTACK:
                break;
            default:
                break;
        }
    }

    private void StateMachine()
    {
        switch (currentBehaviour)
        {
            case EnumZombieBehaviour.IDLE:
                print($"State: {currentBehaviour}");
                zombieMovement.PatrolWithoutHorde();
                // move random
                if (zombieDetectionSenses.CheckIfHearsSomething())
                {
                    // change to state search
                    currentBehaviour = EnumZombieBehaviour.SEARCH;
                }
                if (zombieDetectionSenses.CheckIfSeesPlayer())
                {
                    // change to state detect
                    currentBehaviour = EnumZombieBehaviour.DETECT;
                }
                break;
            case EnumZombieBehaviour.SEARCH:
                // investigate noise (move to noise)
                print($"State: {currentBehaviour}");
                zombieMovement.MoveToTarget(zombieDetectionSenses.GetPositionOfNoise(), zombieStats.GetZombieSpeed(), 3f);
                if (zombieDetectionSenses.GetPositionOfNoise() != null)
                {
                    if (CheckIfIsInRange(zombieDetectionSenses.GetPositionOfNoise(), 3f)) // check if target is in range
                    {
                        // look around function
                       // print($"TIMER-------> {searchCountdown}");
                       // print("Is looking");
                        searchCountdown -= Time.deltaTime;
                        if (zombieDetectionSenses.CheckIfSeesPlayer()) // sees player
                        {
                            currentBehaviour = EnumZombieBehaviour.DETECT;
                            searchCountdown = searchRateMaxTime;
                            print("found soemthing");
                        }
                        else if (searchCountdown <= 0 && !zombieDetectionSenses.CheckIfSeesPlayer())
                        {
                            //TODO: Maybe stay until noise is off
                            zombieMovement.StopMoving();
                            currentBehaviour = EnumZombieBehaviour.IDLE;
                            searchCountdown = searchRateMaxTime;
                            zombieDetectionSenses.RemovePositionOfNoise();
                            print("nothing found");
                        }
                    }
                }
                break;
            case EnumZombieBehaviour.DETECT:
                print($"State: {currentBehaviour}");
                if (zombieDetectionSenses.CheckIfSeesPlayer())
                {
                    // rotate to player and stop patroling
                    detectionCountdown -= Time.deltaTime;
                    if (detectionCountdown <= 0)
                    {
                        currentBehaviour = EnumZombieBehaviour.CHASE;
                        detectionCountdown = detectionRateMaxTime;
                    }
                }
                else
                {
                    zombieMovement.StopMoving();
                    currentBehaviour = EnumZombieBehaviour.IDLE;
                }
                break;
            case EnumZombieBehaviour.CHASE:
                print($"State: {currentBehaviour}");
                zombieMovement.MoveToTarget(zombieDetectionSenses.GetPlayerPosition(), zombieStats.GetZombieSpeed(), 1f);
                if (CheckIfIsInRange(zombieDetectionSenses.GetPlayerPosition(), zombieStats.GetZombieAttackRange())) // check if is in attack range
                {
                    currentBehaviour = EnumZombieBehaviour.ATTACK;
                }
                if (!CheckIfIsInRange(zombieDetectionSenses.GetPlayerPosition(), maxSenseRange)) // check if player is out of max chase range
                {
                    print("Out of range");
                    zombieMovement.StopMoving();
                    currentBehaviour = EnumZombieBehaviour.IDLE;
                }
                break;
            case EnumZombieBehaviour.ATTACK:
                print($"State: {currentBehaviour}");
                if (CheckIfIsInRange(zombieDetectionSenses.GetPlayerPosition(), zombieStats.GetZombieAttackRange())) // check if player is in attack range
                {
                    // attck funtion
                    print("Hit player with attack");
                }
                else
                {
                    currentBehaviour = EnumZombieBehaviour.CHASE;
                }
                break;
            default:
                break;
        }
    }

    private bool CheckIfIsInRange(Transform target, float maxRange)
    {
        //print($"Range: {(target.position - transform.position).sqrMagnitude} | {(maxRange * maxRange)}");
        if ((target.position - transform.position).sqrMagnitude < (maxRange * maxRange))
        {
            return true;
        }
        return false;
    }
}
