using UnityEngine;

public class HeadCollisionSensor : MonoBehaviour
{
    private CarRespawnSystem carRespawnSystem;

    void Start()
    {
        carRespawnSystem = GetComponentInParent<CarRespawnSystem>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground")) 
        {
            if (carRespawnSystem != null)
            {
                Debug.Log("Kafa çarptı! Respawn yapılıyor...");
                carRespawnSystem.Respawn();
            }
        }
    }
}