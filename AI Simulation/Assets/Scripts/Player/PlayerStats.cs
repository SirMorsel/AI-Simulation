using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private GameManager gameManager;
    private UIManager uiManager;
    [SerializeField] private float maxHealth = 250.0F;
    [SerializeField] private float walkSpeed = 5.0F;
    [SerializeField] private float runSpeed = 10.0F;
    [SerializeField] private float turnSpeed = 10.0F;

    private float currentHealth;
    // Start is called before the first frame update

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    void Start()
    {
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        uiManager.SetHealthTextFieldText(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        print($"Playerhealth: {GetCurrentHealth()}");
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetWalkSpeed()
    {
        return walkSpeed;
    }

    public float GetRunSpeed()
    {
        return runSpeed;
    }

    public float GetTurnSpeed()
    {
        return turnSpeed;
    }

    public void SetHealth(float damage)
    {
        currentHealth += damage;
        if (currentHealth <= 0)
        {
            gameManager.RestartScene();
        }
        uiManager.SetHealthTextFieldText(currentHealth, maxHealth);
    }
}
