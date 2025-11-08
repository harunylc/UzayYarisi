using System.Collections;
using UnityEngine;

public class EnemySlowPU : MonoBehaviour
{
    public float slowAmount = 100f;  // Ne kadar yavaşlatacak
    public float duration = 3f;      // Etki süresi

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            // Power-up'ı alan oyuncuya göre rakibi bul
            DriveMyCar player1 = FindObjectOfType<DriveMyCar>();
            DriveMyCar_Player2 player2 = FindObjectOfType<DriveMyCar_Player2>();

            if (other.CompareTag("Player"))
            {
                // Player 1 aldıysa Player2’yi yavaşlat
                if (player2 != null)
                    StartCoroutine(SlowDown(player2));
            }
            else if (other.CompareTag("Player2"))
            {
                // Player2 aldıysa Player1’i yavaşlat
                if (player1 != null)
                    StartCoroutine(SlowDown(player1));
            }

            Destroy(gameObject); // Power-up yok olur
        }
    }

    public IEnumerator SlowDown(MonoBehaviour target)
    {
        float originalSpeed = 0f;

        // Player tipine göre erişim
        if (target is DriveMyCar p1)
        {
            originalSpeed = p1.speed;
            p1.speed -= slowAmount;
        }
        else if (target is DriveMyCar_Player2 p2)
        {
            originalSpeed = p2.speed;
            p2.speed -= slowAmount;
        }

        // Etki süresi
        yield return new WaitForSeconds(duration);

        // Eski hıza dön
        if (target is DriveMyCar p1r)
        {
            p1r.speed = originalSpeed;
        }
        else if (target is DriveMyCar_Player2 p2r)
        {
            p2r.speed = originalSpeed;
        }
    }
}