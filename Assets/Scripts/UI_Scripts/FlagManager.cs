using UnityEngine;

public class FlagManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameRoundManager.Instance == null)
        {
            Debug.LogError("GameRoundManager bulunamadı!");
            return;
        }

        if (other.CompareTag("P1"))
        {
            GameRoundManager.Instance.PlayerReachedFlag(1);
        }
        else if (other.CompareTag("P2"))
        {
            GameRoundManager.Instance.PlayerReachedFlag(2);
        }
    }
}