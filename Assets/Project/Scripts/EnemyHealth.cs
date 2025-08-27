using UnityEngine;

public class EnemyHealth : MonoBehaviour 
{
    [SerializeField] private int startingHealth = 3;

    private int currentHealth;

    private void Start() 
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage) 
    {
        currentHealth -= damage;
        if (currentHealth <= 0) 
        {
            Destroy(gameObject);
        }
    }

    private void DetectDeath()
    {
        if (currentHealth <= 0) 
        {
            Destroy(gameObject);
        }
    }
}
