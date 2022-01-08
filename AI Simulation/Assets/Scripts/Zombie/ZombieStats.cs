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

    private float zombieEyeDetectionAngle;
    private float zombieEyeDetectionMaxDistance;
    private float zombieHearDetectionRadius;

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

        zombieEyeDetectionAngle = zombieData.EyeDetectionAngle;
        zombieEyeDetectionMaxDistance = zombieData.EyeDetectionMaxDistance;
        zombieHearDetectionRadius = zombieData.HearDetectionRadius;

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
}