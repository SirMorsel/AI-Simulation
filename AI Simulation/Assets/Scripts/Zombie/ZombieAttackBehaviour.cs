using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackBehaviour : MonoBehaviour
{
    private PlayerController playerController;
    private ZombieStats zombieStats;

    private float attackInterval = 2f; //script obj
    private float attackCountdown;
    // Start is called before the first frame update
    void Start()
    {
        playerController = PlayerController.Instance;
        zombieStats = this.GetComponent<ZombieStats>();
        attackCountdown = attackInterval;
    }

    public void Attack()
    {
        attackCountdown -= Time.deltaTime;
        if (attackCountdown <= 0)
        {
            //send Damage to player
            playerController.gameObject.GetComponent<PlayerStats>().SetHealth(-zombieStats.GetZombieAttackDamage());
            ResetAttackCountdown();
        }
    }

    public void ResetAttackCountdown()
    {
        attackCountdown = attackInterval;
    }
}
