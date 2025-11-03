using UnityEngine;

public class CameraFollowP2 : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2f, -10f);
    private float fixedY;

    void LateUpdate()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player2");
            if (player != null)
            {
                target = player.transform;
                fixedY = target.position.y + offset.y;
            }
            else
            {
                return;
            }
        }
        Vector3 newPos = transform.position;
        newPos.x = target.position.x + offset.x;
        newPos.y = fixedY;
        newPos.z = offset.z;
        transform.position = newPos;
    }
}
