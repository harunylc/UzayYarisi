using System.Collections;
using UnityEngine;

public class ReverseControlsPU : MonoBehaviour
{
    public float duration = 5f;

    public IEnumerator ReverseControls(GameObject targetPlayer)
    {
        // DriveMyCar veya DriveMyCar_Player2 scriptlerini bul
        var drive1 = targetPlayer.GetComponent<DriveMyCar>();
        var drive2 = targetPlayer.GetComponent<DriveMyCar_Player2>();

        if (drive1 != null)
        {
            drive1.InvertControls(true);
            yield return new WaitForSeconds(duration);
            drive1.InvertControls(false);
        }
        else if (drive2 != null)
        {
            drive2.InvertControls(true);
            yield return new WaitForSeconds(duration);
            drive2.InvertControls(false);
        }
    }
}