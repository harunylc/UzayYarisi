using UnityEngine;

public class ShieldPU : MonoBehaviour
{
    public GameObject shieldPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            GameObject shield = Instantiate(shieldPrefab, other.transform.position + Vector3.up, Quaternion.identity);
            shield.transform.SetParent(other.transform);
            Destroy(gameObject);
        }
    }
}
