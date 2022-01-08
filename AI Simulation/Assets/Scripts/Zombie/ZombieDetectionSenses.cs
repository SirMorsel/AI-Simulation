using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDetectionSenses : MonoBehaviour
{
    private NoiseManager noiseManager;
    private ZombieBehaviour zombieBehaviour;
    private ZombieStats zombieStats;
    

    public GameObject player; // WIP

    private bool seesPlayer = false;
    private bool hearsSomething = false;
    private Transform noisePosition = null;
    
    // Start is called before the first frame update
    void Start()
    {
        noiseManager = NoiseManager.Instance;
        zombieBehaviour = this.GetComponent<ZombieBehaviour>();
        zombieStats = this.GetComponent<ZombieStats>();
    }

    // Update is called once per frame
    void Update()
    {
        VisualDetection(player.transform);
        if (!seesPlayer)
        {
            HearingDetection();
        }
    }

    private void OnDrawGizmos()
    {
        if (zombieStats != null)
        {
            // Visual
            float totalFOV = zombieStats.GetZombieEyeDetectionAngle();
            float rayRange = zombieStats.GetZombieEyeDetectionMaxDistance();
            float halfFOV = totalFOV / 2.0f;
            Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
            Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
            Vector3 leftRayDirection = leftRayRotation * transform.forward;
            Vector3 rightRayDirection = rightRayRotation * transform.forward;
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);
            Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);

            // Hearing
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, zombieStats.GetZombieHearDetectionRadius());
        }
    }

    private void VisualDetection(Transform target)
    {
        Vector3 targetDir = target.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        if (angle < zombieStats.GetZombieEyeDetectionAngle() * 0.5f && CheckIfIsInVisualView(target))
        {
            RaycastHit hit;
            if (Physics.Linecast(transform.position, target.transform.position, out hit))
            {
                // print(hit.transform.name);
                if (hit.transform.tag == "Player")
                {
                    print("I see you!");
                    seesPlayer = true;
                }
                else
                {
                    seesPlayer = false;
                }
            }
            else
            {
                seesPlayer = false;
            }
        }
        else
        {
            seesPlayer = false;
        }
    }

    private void HearingDetection()
    {
        // refactor calulation of target
        GameObject targetNoise = null;
        for (int i = 0; i < noiseManager.GetObjectsWithNoiseList().Count; i++)
        {
            GameObject objectWithNoise = noiseManager.GetObjectsWithNoiseList()[i];
            if (objectWithNoise.GetComponent<Noise>().CheckIfIsMakingNoise() && CheckIfIsInHearingRange(objectWithNoise.transform))
            {
                //print($"I hear you {objectWithNoise.name}!");
                if (targetNoise != null)
                {
                    if (CalcNoiseVolumeValue(objectWithNoise.transform) > CalcNoiseVolumeValue(targetNoise.transform))
                    {
                        targetNoise = objectWithNoise;
                    }
                }
                else
                {
                    targetNoise = objectWithNoise;
                }
            }
        }
        if (targetNoise != null)
        {
            print($"I hear you {targetNoise.name}!");
            // move to target
            hearsSomething = true;
            noisePosition = targetNoise.transform;
            //zombieBehaviour.ChangeStateOfZombie(EnumZombieBehaviour.SEARCH, targetNoise.transform);
        }
        else
        {
            hearsSomething = false;
        }
    }

    private bool CheckIfIsInVisualView(Transform target)
    {
        return (target.position - transform.position).sqrMagnitude < (zombieStats.GetZombieEyeDetectionMaxDistance() * zombieStats.GetZombieEyeDetectionMaxDistance());
    }

    private bool CheckIfIsInHearingRange(Transform target)
    {
        return (target.position - transform.position).sqrMagnitude < (zombieStats.GetZombieHearDetectionRadius() * zombieStats.GetZombieHearDetectionRadius());
    }

    private float CalcNoiseVolumeValue(Transform target)
    {
        // calc volume to set the target priority (noiseVolume and distance) // WIP
        return (target.position - transform.position).sqrMagnitude * target.GetComponent<Noise>().GetNoiseVolume();
    }

    public bool CheckIfSeesPlayer()
    {
        return seesPlayer;
    }
    public bool CheckIfHearsSomething()
    {
        return hearsSomething;
    }

    public Transform GetPlayerPosition()
    {
        return player.transform;
    }

    public Transform GetPositionOfNoise()
    {
        return noisePosition;
    }
    public void RemovePositionOfNoise()
    {
        noisePosition = null;
    }
}