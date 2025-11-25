using UnityEngine;

public class CarRespawnSystem : MonoBehaviour
{
    
    public bool enableUpsideDownCheck = true; 
    public float timeToWaitBeforeRespawn = 3f; 
    public float upsideDownThreshold = 0.3f;
    public bool enableFallCheck = true;
    public float minHeightY = -10f;

    private Vector3 lastCheckpointPos;
    private Quaternion lastCheckpointRot;
    private Rigidbody rb;
    private float timer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (enableUpsideDownCheck)
        {
            HandleUpsideDownLogic();
        }

        if (enableFallCheck)
        {
            if (transform.position.y < minHeightY)
            {
                Respawn(); 
            }
        }
    }
    
    private void HandleUpsideDownLogic()
    {
        if (transform.up.y < upsideDownThreshold)
        {
            if (rb.linearVelocity.magnitude < 1f)
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            timer = 0f;
        }
        if (timer >= timeToWaitBeforeRespawn)
        {
            Respawn();
        }
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
        timer = 0f; 
    }
    public void Respawn()
    {
        Debug.Log($"{gameObject.name} Respawn oluyor...");

        transform.position = lastCheckpointPos;
        transform.rotation = lastCheckpointRot;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        timer = 0f; 
    }
}