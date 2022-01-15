using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;
    public static PlayerController Instance { get { return _instance; } }

    private PlayerStats playerStats;
    private UIManager uiManager;

    private float attackIntervalCountdown;
    private bool isUsingMeleeAttack = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        playerStats = this.GetComponent<PlayerStats>();
        uiManager = UIManager.Instance;
        attackIntervalCountdown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (isUsingMeleeAttack)
        {
            attackIntervalCountdown -= Time.deltaTime;
            if (attackIntervalCountdown <= 0)
            {
                isUsingMeleeAttack = false;
            }
        }
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        if (movement != Vector3.zero)
        {
            float movementSpeed = playerStats.GetWalkSpeed();
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = playerStats.GetRunSpeed();
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * playerStats.GetTurnSpeed());
            transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
        }
    }

    private void MeleeAttack(GameObject target)
    {
        if (Input.GetKey(KeyCode.Space) && !isUsingMeleeAttack)
        {
            isUsingMeleeAttack = true;
            if (target.GetComponent<ZombieDetectionSenses>().CheckIfSeesPlayer())
            {
                print("Normal damage");
                target.GetComponent<ZombieStats>().SetZombieHealth(playerStats.GetMeleeDamage());
            }
            else
            {
                print("Crit damage");
                target.GetComponent<ZombieStats>().SetZombieHealth(playerStats.GetMeleeDamage() * playerStats.GetCritMultiplier());
            }
            attackIntervalCountdown = playerStats.GetAttackInterval();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Interactable")
        {
            uiManager.ChangeInteractableUITextVisibility(true);
            if (Input.GetKey(KeyCode.E))
            {
                other.gameObject.GetComponent<Noise>().ActivateNoise();
            }
        }
        if (other.tag == "Enemy")
        {
            MeleeAttack(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interactable")
        {
            uiManager.ChangeInteractableUITextVisibility(false);
        }
        if (other.tag == "Enemy")
        {
            MeleeAttack(other.gameObject);
        }
    }
}
