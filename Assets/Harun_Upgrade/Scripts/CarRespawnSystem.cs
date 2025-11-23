using UnityEngine;

public class CarRespawnSystem : MonoBehaviour
{
    private Vector3 lastCheckpointPos;
    private Quaternion lastCheckpointRot;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // İlk doğduğu anı kaydetmek için
    public void SetInitialSpawnPoint(Transform spawnPoint)
    {
        lastCheckpointPos = spawnPoint.position;
        lastCheckpointRot = spawnPoint.rotation;
    }

    // Checkpoint'ten geçince çağrılacak metod
    public void UpdateCheckpoint(Transform newCheckpoint)
    {
        lastCheckpointPos = newCheckpoint.position;
        lastCheckpointRot = newCheckpoint.rotation;
        Debug.Log("Checkpoint Kaydedildi!");
    }

    // Aracı en son noktaya ışınla (Örn: "R" tuşuna basınca veya haritadan düşünce)
    public void Respawn()
    {
        transform.position = lastCheckpointPos;
        transform.rotation = lastCheckpointRot;

        // ÖNEMLİ: Işınlanınca aracın hızını sıfırla, yoksa uçmaya devam eder.
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}