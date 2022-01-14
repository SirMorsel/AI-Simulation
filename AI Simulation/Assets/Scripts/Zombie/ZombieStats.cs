using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStats : MonoBehaviour
{
    [SerializeField] private ScriptableZombie zombieData;

    private string zombieDesignation;
    private string zombieDescription;

    private float zombieMaxHealth;
    private float currentZombieHealth;
    private float zombieSpeed;
    private float zombieAttackDamage;
    private float zombieMaxAttackRange;
    private float zombieAttackInterval; //

    private float zombieEyeDetectionAngle;
    private float zombieEyeDetectionMaxDistance;
    private float zombieHearDetectionRadius;

    private float zombieSearchRateMaxTime;
    private float zombieDetectionRateMaxTime;
    private float zombieMaxSenseRange;

    private float zombiePatrolMaxTimer;
    private float zombiePatrolLookMaxTimer;
    private float zombiePatrolRadius;

    private bool isInAgroMode = false;

    private void Awake()
    {
        InitializeZombieData();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeZombieData()
    {
        zombieDesignation = zombieData.Designation;
        zombieDescription = zombieData.Description;

        zombieMaxHealth = zombieData.MaxHealth;
        zombieSpeed = zombieData.Speed;
        zombieAttackDamage = zombieData.Damage;
        zombieMaxAttackRange = zombieData.MaxAttackRange;
        zombieAttackInterval = zombieData.AttackInterval;

        zombieEyeDetectionAngle = zombieData.EyeDetectionAngle;
        zombieEyeDetectionMaxDistance = zombieData.EyeDetectionMaxDistance;
        zombieHearDetectionRadius = zombieData.HearDetectionRadius;

        zombieSearchRateMaxTime = zombieData.SearchRateMaxTime;
        zombieDetectionRateMaxTime = zombieData.DetectionRateMaxTime;
        zombieMaxSenseRange = zombieData.MaxSenseRange;

        zombiePatrolMaxTimer = zombieData.PatrolMaxTimer;
        zombiePatrolLookMaxTimer = zombieData.PatrolLookMaxTimer;
        zombiePatrolRadius = zombieData.PatrolRadius;

        currentZombieHealth = zombieMaxHealth;
    }

    public void PrintZombieData()
    {
        print($"Designation: {zombieDesignation}, Description: {zombieDescription}, " +
            $"MaxHealth: {zombieMaxHealth}, Speed: {zombieSpeed}, Damage: {zombieAttackDamage}, Range: {zombieMaxAttackRange}" +
            $"EyeDetectionAngle: {zombieEyeDetectionAngle}, zombieEyeDetectionMaxDistance: {zombieEyeDetectionMaxDistance}, HearDetectionRadius: {zombieHearDetectionRadius}");
    }

    // Combat Getter
    public float GetCurrentZombieHealth() 
    {
        return currentZombieHealth;
    }

    public float GetZombieAttackDamage()
    {
        return zombieAttackDamage;
    }

    // Movement Getter
    public float GetZombieSpeed()
    {
        return zombieSpeed;
    }

    public float GetZombieEyeDetectionAngle()
    {
        return zombieEyeDetectionAngle;
    }

    public float GetZombieEyeDetectionMaxDistance()
    {
        return zombieEyeDetectionMaxDistance;
    }

    public float GetZombieHearDetectionRadius()
    {
        return zombieHearDetectionRadius;
    }

    public float GetZombieAttackRange()
    {
        return zombieMaxAttackRange;
    }

    public float GetZombieAttackInterval()
    {
        return zombieAttackInterval;
    }

    public float GetZombiePatrolMaxTimer()
    {
        return zombiePatrolMaxTimer;
    }

    public float GetZombiePatrolLookMaxTimer()
    {
        return zombiePatrolLookMaxTimer;
    }

    public float GetZombiePatrolRadius()
    {
        return zombiePatrolRadius;
    }

    public float GetZombieSearchRateMaxTime()
    {
        return zombieSearchRateMaxTime;
    }

    public float GetZombieDetectionRateMaxTime()
    {
        return zombieDetectionRateMaxTime;
    }

    public float GetZombieMaxSenseRange()
    {
        return zombieMaxSenseRange;
    }

    public void SetZombieHealth(float damage)
    {
        currentZombieHealth -= damage;
        
        print($"New health: {currentZombieHealth}");
        if (currentZombieHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
