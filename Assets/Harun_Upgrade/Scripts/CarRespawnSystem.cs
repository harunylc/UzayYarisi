using UnityEngine;

public class CarRespawnSystem : MonoBehaviour
{
    private Vector3 lastCheckpointPos;
    private Quaternion lastCheckpointRot;
    
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetInitialSpawnPoint(Transform spawnPoint)
    {
        lastCheckpointPos = spawnPoint.position;
        lastCheckpointRot = spawnPoint.rotation;
    }

    public void UpdateCheckpoint(Transform newCheckpoint)
    {
        lastCheckpointPos = newCheckpoint.position;
        lastCheckpointRot = newCheckpoint.rotation;
        Debug.Log("Checkpoint Kaydedildi!");
    }

    public void Respawn()
    {
        transform.position = lastCheckpointPos;
        transform.rotation = lastCheckpointRot;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;     
            rb.angularVelocity = 0f;         
        }
    }
}