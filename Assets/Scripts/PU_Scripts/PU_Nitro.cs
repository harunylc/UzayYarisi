// using UnityEngine;
//
// public class PU_Nitro : MonoBehaviour
// {
//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player") || other.CompareTag("Player2"))
//         {
//             CarPowerUpHandler carHandler = other.GetComponent<CarPowerUpHandler>();
//             if (carHandler != null)
//             {
//                 // Arabanın "cüzdanına" Nitro güçlendirmesini ekle
//                 carHandler.GivePowerUp("Nitro");
//                 // Ve kendini yok et
//                 Destroy(gameObject);
//             }
//         }
//     }
// }
using UnityEngine;

public class PU_Nitro : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            CarPowerUpHandler carHandler = other.GetComponent<CarPowerUpHandler>();
            if (carHandler != null)
            {
                // Arabanın "cüzdanına" Nitro güçlendirmesini ekle
                carHandler.GivePowerUp("Nitro");
                // Ve kendini yok et
                Destroy(gameObject);
            }
        }
    }
}