using UnityEngine;

public class Shield : MonoBehaviour
{
    public float duration = 2f; // Shield kaç saniye aktif kalacak

    private void Start()
    {
        Destroy(gameObject, duration); // duration saniye sonra kendini yok eder
    }
}
