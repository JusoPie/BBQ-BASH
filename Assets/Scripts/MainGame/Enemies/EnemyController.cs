using Fusion;
using UnityEngine;

public class EnemyController : NetworkBehaviour
{
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int points = 10;

    protected virtual void Start()
    {
        ResetState();
    }

    protected virtual void Update()
    {
        // Update logic if necessary
    }

    public virtual void TakeDamage(int damage)
    {
        if (Object.HasStateAuthority)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public virtual void ResetState()
    {
        currentHealth = maxHealth;
    }

    protected virtual void Die()
    {
        // Award points to players or update game state here
        // e.g., GameManager.Instance.AddPoints(points);

        Runner.Despawn(Object); // Despawn the enemy
    }

    // Optionally, you can add more methods to customize behavior for different enemies
}




