using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviour : MonoBehaviour
{
    private HordeManager hordeManager;

    private ZombieAttackBehaviour zombieAttackBehaviour;
    private ZombieDetectionSenses zombieDetectionSenses;
    private ZombieMovement zombieMovement;
    private ZombieStats zombieStats;

    private EnumZombieBehaviour currentBehaviour = EnumZombieBehaviour.IDLE;

    private float searchCountdown;
    private float detectionCountdown;

    // This timer is used to prevent any freezing in certain state conditions by automatically returning the agent to the idle state after the timer has expired.
    private float failsafeMaxTime = 20.0F;
    private float failSafeCountdown;

    [SerializeField] private bool isInHorde = false;

    // Start is called before the first frame update
    void Start()
    {
        hordeManager = HordeManager.Instance;
        zombieAttackBehaviour = this.GetComponent<ZombieAttackBehaviour>();
        zombieDetectionSenses = this.GetComponent<ZombieDetectionSenses>();
        zombieMovement = this.GetComponent<ZombieMovement>();
        zombieStats = this.GetComponent<ZombieStats>();

        searchCountdown = zombieStats.GetZombieSearchRateMaxTime();
        detectionCountdown = zombieStats.GetZombieDetectionRateMaxTime();
        failSafeCountdown = failsafeMaxTime;

        if (isInHorde)
        {
            hordeManager.AddToHorde(this.gameObject);
        }
        
    }

    private void Update() //maybe fiexupdate
    {
        StateMachine();
    }

    private void OnDestroy()
    {
        hordeManager.RemoveFromHorde(this.gameObject);
    }

    private void StateMachine()
    {
        switch (currentBehaviour)
        {
            case EnumZombieBehaviour.IDLE:
                //print($"State: {currentBehaviour}");
                if (isInHorde)
                {
                    zombieMovement.PatrolWithoutHorde();
                    hordeManager.HordePatrolFormation();
                    if (!hordeManager.CheckIfIsZombieLeader(this.gameObject))
                    {
                        if (currentBehaviour == EnumZombieBehaviour.IDLE)
                        {
                            zombieMovement.PatrolWithHorde(hordeManager.GetFormationList()[hordeManager.GetIndexOfZombieInHordeList(this.gameObject)]);
                        }
                    }
                }
                else
                {
                    zombieMovement.PatrolWithoutHorde();
                }
                if (zombieDetectionSenses.CheckIfHearsSomething())
                {
                    currentBehaviour = EnumZombieBehaviour.SEARCH;
                }
                if (zombieDetectionSenses.CheckIfSeesPlayer())
                {
                    currentBehaviour = EnumZombieBehaviour.DETECT;
                }

                break;
            case EnumZombieBehaviour.SEARCH:
                //print($"State: {currentBehaviour}");
                zombieMovement.MoveToTarget(zombieDetectionSenses.GetPositionOfNoise(), zombieStats.GetZombieSpeed(), 3f);
                if (zombieDetectionSenses.GetPositionOfNoise() != null)
                {
                    
                    if (CheckIfIsInRange(zombieDetectionSenses.GetPositionOfNoise(), 5f)) // check if target is in range
                    {
                       // print("Is looking");
                        searchCountdown -= Time.deltaTime;
                        if (zombieDetectionSenses.CheckIfSeesPlayer())
                        {
                            currentBehaviour = EnumZombieBehaviour.DETECT;
                            searchCountdown = zombieStats.GetZombieSearchRateMaxTime();
                            print("found soemthing");
                        }
                        else if (searchCountdown <= 0 && !zombieDetectionSenses.CheckIfSeesPlayer())
                        {
                            zombieMovement.StopMoving();
                            currentBehaviour = EnumZombieBehaviour.IDLE;
                            searchCountdown = zombieStats.GetZombieSearchRateMaxTime();
                            zombieDetectionSenses.RemovePositionOfNoise();
                            print("nothing found");
                        }
                        failSafeCountdown = failsafeMaxTime;
                    }
                    else
                    {
                        failSafeCountdown -= Time.deltaTime;
                        if (failSafeCountdown <= 0)
                        {
                            currentBehaviour = EnumZombieBehaviour.IDLE;
                            failSafeCountdown = failsafeMaxTime;
                        }
                    }
                }
                else
                {
                    currentBehaviour = EnumZombieBehaviour.IDLE;
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
                            hordeManager.SetSeesPlayerStatus(true);
                            hordeManager.SendAlarmToHorde();
                        }
                        currentBehaviour = EnumZombieBehaviour.CHASE;
                        detectionCountdown = zombieStats.GetZombieDetectionRateMaxTime();
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
                zombieMovement.MoveToTarget(zombieDetectionSenses.GetPlayerPosition(), zombieStats.GetZombieSpeed(), 1.5f);

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
                        //print("Horde out of range");
                        zombieMovement.StopMoving();
                        currentBehaviour = EnumZombieBehaviour.IDLE;
                    }
                }
                else
                {
                    if (!CheckIfIsInRange(zombieDetectionSenses.GetPlayerPosition(), zombieStats.GetZombieMaxSenseRange())) // check if player is out of max chase range
                    {
                        //print("Out of range");
                        zombieMovement.StopMoving();
                        currentBehaviour = EnumZombieBehaviour.IDLE;
                    }
                }
                

                break;
            case EnumZombieBehaviour.ATTACK:
                //print($"State: {currentBehaviour}");
                if (CheckIfIsInRange(zombieDetectionSenses.GetPlayerPosition(), zombieStats.GetZombieAttackRange())) // check if player is in attack range
                {
                    zombieAttackBehaviour.Attack();
                }
                else
                {
                    zombieAttackBehaviour.ResetAttackCountdown();
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
        if ((target.position - transform.position).sqrMagnitude < ((maxRange * maxRange)))
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
