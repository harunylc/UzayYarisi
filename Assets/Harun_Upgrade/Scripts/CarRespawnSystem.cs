using UnityEngine;

public class CarRespawnSystem : MonoBehaviour
{
    private Vector3 lastCheckpointPos;
    private Quaternion lastCheckpointRot;
    
    // DEĞİŞİKLİK: 3D Rigidbody yerine 2D kullanıyoruz
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // İlk doğduğu anı kaydetmek için (SpawnManager'dan çağrılır)
    public void SetInitialSpawnPoint(Transform spawnPoint)
    {
        lastCheckpointPos = spawnPoint.position;
        lastCheckpointRot = spawnPoint.rotation;
    }

    // Checkpoint'ten geçince çağrılacak metod (İleride kullanırsın)
    public void UpdateCheckpoint(Transform newCheckpoint)
    {
        lastCheckpointPos = newCheckpoint.position;
        lastCheckpointRot = newCheckpoint.rotation;
        Debug.Log("Checkpoint Kaydedildi!");
    }

    // Aracı en son noktaya ışınla
    public void Respawn()
    {
        // 1. Konumu sıfırla
        transform.position = lastCheckpointPos;
        transform.rotation = lastCheckpointRot;

        // 2. Fiziği (Hızı) sıfırla ki doğunca fırlamasın
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;       // 2D Hız sıfırlama
            rb.angularVelocity = 0f;          // 2D Dönüş hızı sıfırlama
        }
        
        Debug.Log("Araç Respawn Oldu!");
    }
}