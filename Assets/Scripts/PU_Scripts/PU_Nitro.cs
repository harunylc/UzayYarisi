using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class PU_Nitro : MonoBehaviour
{
    /*
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
    */
    public float duration = 10f;
    public IEnumerator Nitro(MonoBehaviour target)
    {
        float originalNitro = 0f;

        if (target is DriveMyCar p1)
        {
            originalNitro = p1.nitroRechargeRate;
            p1.nitroRechargeRate *= (1.5f);
        }
        else if (target is DriveMyCar_Player2 p2)
        {
            originalNitro = p2.nitroRechargeRate;
            p2.nitroRechargeRate *= (1.5f);
        }

        yield return new WaitForSeconds(duration);

        if (target is DriveMyCar_Player2 p1r)
        {
            p1r.speed = originalNitro;
        }
        else if (target is DriveMyCar_Player2 p2r)
        {
            p2r.speed = originalNitro;
        }
    }
}