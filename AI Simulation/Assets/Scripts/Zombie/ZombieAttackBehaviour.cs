using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackBehaviour : MonoBehaviour
{
    private PlayerController playerController;
    private ZombieStats zombieStats;

    private float attackCountdown;

    // Start is called before the first frame update
    void Start()
    {
        playerController = PlayerController.Instance;
        zombieStats = this.GetComponent<ZombieStats>();
        attackCountdown = zombieStats.GetZombieAttackInterval();
    }

    public void Attack()
    {
        attackCountdown -= Time.deltaTime;
        if (attackCountdown <= 0)
        {
            playerController.gameObject.GetComponent<PlayerStats>().SetHealth(-zombieStats.GetZombieAttackDamage());
            ResetAttackCountdown();
        }
    }

    public void ResetAttackCountdown()
    {
        attackCountdown = zombieStats.GetZombieAttackInterval();
    }
}
