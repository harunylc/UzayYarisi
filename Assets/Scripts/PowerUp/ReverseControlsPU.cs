using UnityEngine;
using System.Collections;

public class ReverseControlsPU : MonoBehaviour
{
    [SerializeField] private float duration = 5f;

    public IEnumerator ReverseControls(GameObject enemyPlayer)
    {
        if (enemyPlayer == null) yield break;

        var player1 = enemyPlayer.GetComponent<DriveMyCar>();
        var player2 = enemyPlayer.GetComponent<DriveMyCar_Player2>();

        if (player1 != null)
        {
            player1.InvertControls(true);
            yield return new WaitForSeconds(duration);
            player1.InvertControls(false);
        }
        else if (player2 != null)
        {
            player2.InvertControls(true);
            yield return new WaitForSeconds(duration);
            player2.InvertControls(false);
        }
    }
}