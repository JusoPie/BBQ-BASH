using Fusion;
using UnityEngine;

public class EnemyController : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private int points = 10;

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        // Update logic if necessary
    }

    public void TakeDamage(int damage)
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

    public void ResetState() 
    {
        currentHealth = maxHealth;
    }

    private void Die()
    {
        // Award points to players or update game state here
        // e.g., GameManager.Instance.AddPoints(points);

        Runner.Despawn(Object); // Despawn the enemy
    }

    // Optionally, you can add more methods to customize behavior for different enemies
}



