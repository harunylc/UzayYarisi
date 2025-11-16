using UnityEngine;

public class PowerUpPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            PowerUpManager manager = FindObjectOfType<PowerUpManager>();
            manager.CollectPowerUp(other.gameObject, this.gameObject);
        }
    }
}
