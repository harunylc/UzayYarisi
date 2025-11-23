using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CarRespawnSystem car = other.GetComponentInParent<CarRespawnSystem>();

        if (car != null)
        {
            car.UpdateCheckpoint(this.transform);
        }
    }
}