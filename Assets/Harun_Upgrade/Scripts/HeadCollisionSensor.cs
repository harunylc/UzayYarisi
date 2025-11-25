using UnityEngine;

public class HeadCollisionSensor : MonoBehaviour
{
    private CarRespawnSystem carRespawnSystem;

    void Start()
    {
        // Bu script "Kafa" objesinde olacağı için, arabanın ana gövdesindeki
        // CarRespawnSystem scriptini bulmak için "InParent" kullanıyoruz.
        carRespawnSystem = GetComponentInParent<CarRespawnSystem>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eğer çarptığımız şey "Zemin" ise
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