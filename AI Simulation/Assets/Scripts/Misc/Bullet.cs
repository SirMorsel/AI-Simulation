using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage = 50.0F;
    private float speed = 800.0F;
    private float maxLifeTime = 5.0F;

    private void Update()
    {
        Destroy(this.gameObject, maxLifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<ZombieStats>().SetZombieHealth(damage);
            }
            Destroy(this.gameObject);
        }
    }

    public float GetSpeed()
    {
        return speed;
    }
}
