using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Zombie type", menuName = "ScriptableObjects/Zombies")]
public class ScriptableZombie : ScriptableObject
{
    public string Designation;
    public string Description;

    public float MaxHealth;
    public float Speed;
    public float Damage;
    public float MaxAttackRange;

    public float EyeDetectionAngle;
    public float EyeDetectionMaxDistance;
    public float HearDetectionRadius;
}
