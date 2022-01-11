using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviour : MonoBehaviour
{
    private HordeManager hordeManager;

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

    [SerializeField] private bool isInHorde = false;

    // Start is called before the first frame update
    void Start()
    {
        hordeManager = HordeManager.Instance;
        zombieDetectionSenses = this.GetComponent<ZombieDetectionSenses>();
        zombieMovement = this.GetComponent<ZombieMovement>();
        zombieStats = this.GetComponent<ZombieStats>();

        searchCountdown = searchRateMaxTime;
        detectionCountdown = detectionRateMaxTime;

        if (isInHorde)
        {
            hordeManager.AddToHorde(this.gameObject);
        }
        
    }

    private void Update() //maybe fiexupdate
    {
        StateMachine();
    }

    private void StateMachine()
    {
        switch (currentBehaviour)
        {
            case EnumZombieBehaviour.IDLE:
                //print($"State: {currentBehaviour}");
                zombieMovement.PatrolWithoutHorde();
                hordeManager.HordePatrol();
                if (zombieDetectionSenses.CheckIfHearsSomething())
                {
                    currentBehaviour = EnumZombieBehaviour.SEARCH;
                }
                if (zombieDetectionSenses.CheckIfSeesPlayer())
                {
                    currentBehaviour = EnumZombieBehaviour.DETECT;
                }

                if (isInHorde)
                {
                    // (inHorde) send command to all in horde if in idle
                    if (hordeManager.CheckIfIsZombieLeader(this.gameObject))
                    {
                        // horde move with all zombies behavior == idle
                        
                    }
                    else
                    {
                        zombieMovement.PatrolWithHorde(hordeManager.GetFormationList()[hordeManager.GetIndexOfZombieInHordeList(this.gameObject)]);
                    }
                }
                else
                {
                    zombieMovement.PatrolWithoutHorde();
                }
                
                break;
            case EnumZombieBehaviour.SEARCH:
                //print($"State: {currentBehaviour}");
                zombieMovement.MoveToTarget(zombieDetectionSenses.GetPositionOfNoise(), zombieStats.GetZombieSpeed(), 3f);
                if (zombieDetectionSenses.GetPositionOfNoise() != null)
                {
                    if (CheckIfIsInRange(zombieDetectionSenses.GetPositionOfNoise(), 3f)) // check if target is in range
                    {
                        // check if is in horde
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
                //print($"State: {currentBehaviour}");
                if (zombieDetectionSenses.CheckIfSeesPlayer())
                {
                    zombieMovement.StopMoving();
                    zombieMovement.LookAtTarget(zombieDetectionSenses.GetPlayerPosition());

                    detectionCountdown -= Time.deltaTime;
                    if (detectionCountdown <= 0)
                    {
                        if (isInHorde)
                        {
                            hordeManager.SetPlayerSeesPlayerStatus(true);
                            // set alarm to horde (All members in horde to chase)
                            hordeManager.SendAlarmToHorde();
                        }
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
                //print($"State: {currentBehaviour}");
                zombieMovement.MoveToTarget(zombieDetectionSenses.GetPlayerPosition(), zombieStats.GetZombieSpeed(), 1f);

                if (CheckIfIsInRange(zombieDetectionSenses.GetPlayerPosition(), zombieStats.GetZombieAttackRange())) // check if is in attack range
                {
                    currentBehaviour = EnumZombieBehaviour.ATTACK;
                }

                if (isInHorde)
                {
                    // check if other zombie in horde sees player
                    hordeManager.CheckIfAHordeMemberIsInRange();
                    if (!hordeManager.CheckIfHordeSeesPlayer())
                    {
                        print("Horde out of range");
                        zombieMovement.StopMoving();
                        currentBehaviour = EnumZombieBehaviour.IDLE;
                    }
                }
                else
                {
                    if (!CheckIfIsInRange(zombieDetectionSenses.GetPlayerPosition(), maxSenseRange)) // check if player is out of max chase range
                    {
                        print("Out of range");
                        zombieMovement.StopMoving();
                        currentBehaviour = EnumZombieBehaviour.IDLE;
                    }
                }
                

                break;
            case EnumZombieBehaviour.ATTACK:
                //print($"State: {currentBehaviour}");
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

    public void OverrideStage(EnumZombieBehaviour behaviour)
    {
        currentBehaviour = behaviour;
    }
}
