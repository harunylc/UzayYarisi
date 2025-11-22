using System.Collections;
using UnityEngine;

public class ReverseControlsPU : MonoBehaviour
{
    public float duration = 3f;

    public IEnumerator ReverseControls(GameObject targetPlayer)
    {
        if (targetPlayer.TryGetComponent<DriveMyCar>(out var p1))
        {
            p1.SetReverseControls(true);
            yield return new WaitForSeconds(duration);
            p1.SetReverseControls(false);
        }
        else if (targetPlayer.TryGetComponent<DriveMyCar_Player2>(out var p2))
        {
            p2.SetReverseControls(true);
            yield return new WaitForSeconds(duration);
            p2.SetReverseControls(false);
        }
    }
}