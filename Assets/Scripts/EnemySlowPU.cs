using System.Collections;
using UnityEngine;

public class EnemySlowPU : MonoBehaviour
{
    /*[SerializeField] private float slowDuration = 3f;
    [SerializeField] private float slowPercent = 0.7f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player1 aldıysa Player2 yavaşlasın
        if (other.CompareTag("Player"))
        {
            Debug.Log("Power-up Player1 tarafından alındı!");

            Car_Driving car_driving = GameObject.FindWithTag("Player2")?.GetComponent<Car_Driving>();
            if (car_driving != null)
            {
                StartCoroutine(SlowTemporarily(car_driving));
                Debug.Log("Player2 yavaşlatıldı!");
            }

            Destroy(gameObject);
        }
        // Player2 aldıysa Player1 yavaşlasın
        else if (other.CompareTag("Player2"))
        {
            Debug.Log("Power-up Player2 tarafından alındı!");

            Car_Driving car_driving = GameObject.FindWithTag("Player")?.GetComponent<Car_Driving>();
            if (car_driving != null)
            {
                StartCoroutine(SlowTemporarily(car_driving));
                Debug.Log("Player1 yavaşlatıldı!");
            }

            Destroy(gameObject);
        }
    }

    private IEnumerator SlowTemporarily(Car_Driving target)
    {
        float originalSpeed = target.GetSpeed();
        target.SetSpeed(originalSpeed * (1f - slowPercent));

        yield return new WaitForSeconds(slowDuration);
        target.SetSpeed(originalSpeed);
        Debug.Log(target.name + " eski hızına döndü.");
    }*/
}