using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Rigidbody rb;

    public float forwardForce;

    public float damage = 10;

    public float lifeSpan = 1f;

    private void Start()
    {
        Invoke("Die", lifeSpan);
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * forwardForce);
    }
    private void OnTriggerEnter(Collider other)
    {
        VikingUnit unit = other.GetComponent<VikingUnit>();
        if(unit != null)
        {
            unit.TakeDamage(damage, null);
            Die();
        }
    }

    void Die()
    {
        Destroy (gameObject);
    }
}
