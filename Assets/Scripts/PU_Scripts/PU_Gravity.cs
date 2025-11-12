using UnityEngine;

public class PU_Gravity : MonoBehaviour
{
    public float duration = 5f; // Etki süresi

    public void ApplyEffectToPlayer1()
    {
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            Rigidbody2D rb = player1.GetComponentInChildren<Rigidbody2D>();
            if (rb != null)
                StartCoroutine(ReduceMassTemporarily(rb, "Player 1"));
        }
    }

    public void ApplyEffectToPlayer2()
    {
        GameObject player2 = GameObject.FindWithTag("Player2");
        if (player2 != null)
        {
            Rigidbody2D rb = player2.GetComponentInChildren<Rigidbody2D>();
            if (rb != null)
                StartCoroutine(ReduceMassTemporarily(rb, "Player 2"));
        }
    }

    private IEnumerator ReduceMassTemporarily(Rigidbody2D rb, string playerName)
    {
        float originalMass = rb.mass;
        rb.mass *= 0.5f;
        Debug.Log($"{playerName} kütlesi %50 azaltıldı. Yeni kütle: {rb.mass}");

        yield return new WaitForSeconds(duration);

        rb.mass = originalMass;
        Debug.Log($"{playerName} kütlesi normale döndü: {rb.mass}");
    }
}