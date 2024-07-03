using Fusion;
using UnityEngine;

public class EnemyController : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private int points = 10;

    private float movementDirection;
    private NetworkRigidbody2D networkRb;

    private void Awake()
    {
        networkRb = GetComponent<NetworkRigidbody2D>();
    }

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
        movementDirection = Random.Range(-1f, 1f) >= 0 ? 1 : -1; // Randomly sets to 1 or -1
        Debug.Log("New Movement Direction: " + movementDirection);
    }

    private void MoveEnemy()
    {
        networkRb.Rigidbody.velocity = new Vector2(movementDirection * moveSpeed, networkRb.Rigidbody.velocity.y);
        Debug.Log("Moving Enemy: " + networkRb.Rigidbody.velocity);

        // Example of basic movement, you can implement more complex AI here
        if (Mathf.Abs(transform.position.x) > 10f) // Example boundary check
        {
            SetRandomMovementDirection();
        }
    }

    public void TakeDamage(int damage)
    {
        if (Object.HasStateAuthority)
        {
            currentHealth -= damage;
            Debug.Log("Enemy took damage, current health: " + currentHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        // Award points to players or update game state here
        Debug.Log("Enemy died.");
        // e.g., GameManager.Instance.AddPoints(points);

        Runner.Despawn(Object); // Despawn the enemy
    }

    // Optionally, you can add more methods to customize behavior for different enemies
}
