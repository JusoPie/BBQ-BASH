using Fusion;
using UnityEngine;

public class EnemyController : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private int points = 10;

    private Vector2 movementDirection;
    private Transform playerTarget;

    private void Start()
    {
        currentHealth = maxHealth;
        SetRandomMovementDirection();
    }

    private void Update()
    {
        if (Object.HasStateAuthority)
        {
            MoveEnemy();
        }
    }

    private void SetRandomMovementDirection()
    {
        movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void MoveEnemy()
    {
        transform.Translate(movementDirection * moveSpeed * Time.deltaTime);

        // Example of basic movement, you can implement more complex AI here
        if (Vector2.Distance(transform.position, Vector2.zero) > 10f) // Example boundary check
        {
            SetRandomMovementDirection();
        }
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

    private void Die()
    {
        // Award points to players or update game state here
        // e.g., GameManager.Instance.AddPoints(points);

        Runner.Despawn(Object); // Despawn the enemy
    }

    // Optionally, you can add more methods to customize behavior for different enemies
}
