using UnityEngine;

public class RandomPowerUpGiver : MonoBehaviour
{
    // Inspector'dan, bu kutudan çıkabilecek TÜM güçlerin isimlerini yazacağız.
    public string[] possiblePowerUps = { "Gravity", "DarkScreen", "Nitro" };

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Temas eden obje bir oyuncu mu?
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            // Evet. Arabanın üzerindeki "beyni" (CarPowerUpHandler) bul.
            CarPowerUpHandler carHandler = other.GetComponent<CarPowerUpHandler>();
            if (carHandler != null)
            {
                // 1. Rastgele bir güçlendirme ismi seç.
                int index = Random.Range(0, possiblePowerUps.Length);
                string selectedPowerUpName = possiblePowerUps[index];

                // 2. O ismi arabanın beynine "anons et".
                carHandler.GivePowerUp(selectedPowerUpName);

                // 3. Kutuyu yok et.
                Destroy(gameObject);
            }
        }
    }
}
