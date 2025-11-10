using System;
using System.Collections;
using UnityEngine;

public class EnemySlowPU : MonoBehaviour
{
    public float slowAmount = 100f;
    public float duration = 10f;

    public IEnumerator SlowDown(MonoBehaviour target)
    {
        float originalSpeed = 0f;

        if (target is DriveMyCar p1)
        {
            originalSpeed = p1.speed;
            p1.speed -= slowAmount;
        }
        else if (target is DriveMyCar_Player2 p2)
        {
            originalSpeed = p2.speed;
            p2.speed -= slowAmount;
        }

        yield return new WaitForSeconds(duration);

        if (target is DriveMyCar_Player2 p1r)
        {
            p1r.speed = originalSpeed;
        }
        else if (target is DriveMyCar_Player2 p2r)
        {
            p2r.speed = originalSpeed;
        }
    }
}