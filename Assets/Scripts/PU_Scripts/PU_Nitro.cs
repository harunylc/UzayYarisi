using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class PU_Nitro : MonoBehaviour
{
    public float duration = 10f;
    public IEnumerator Nitro(MonoBehaviour target)
    {
        float originalNitroRate = 0f; 

        if (target is DriveMyCar p1)
        {
            originalNitroRate = p1.nitroRechargeRate;
            p1.nitroRechargeRate *= 1.5f; 
        }
        else if (target is DriveMyCar_Player2 p2)
        {
            originalNitroRate = p2.nitroRechargeRate;
            p2.nitroRechargeRate *= 1.5f;
        }

        yield return new WaitForSeconds(duration);

        if (target is DriveMyCar p1_after) 
        {
            p1_after.nitroRechargeRate = originalNitroRate; 
        }
        else if (target is DriveMyCar_Player2 p2_after)
        {
            p2_after.nitroRechargeRate = originalNitroRate;
        }
    }
}