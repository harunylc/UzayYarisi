using UnityEngine;

public class PU_Gravity : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            CarPowerUpHandler carHandler = other.GetComponent<CarPowerUpHandler>();
            if (carHandler != null)
            {
                // Artık güçlendirmenin adını bir string olarak veriyoruz.
                carHandler.GivePowerUp("Gravity");
                Destroy(gameObject);
            }
        }
    }
}