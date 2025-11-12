using UnityEngine;

public class Shield : MonoBehaviour
{
    public float duration = 2f;

    private void Start()
    {
        Destroy(gameObject, duration);
    }
}
