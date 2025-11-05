using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnManager : MonoBehaviour
{
    [Header("Car Prefabs")]
    public GameObject[] carPrefabsP1;
    public GameObject[] carPrefabsP2;

    [Header("Spawn Points")]
    public Transform spawnPointP1;
    public Transform spawnPointP2;

    void Start()
    {
        int p1Index = PlayerSelectionData.player1CarIndex;
        int p2Index = PlayerSelectionData.player2CarIndex;

        if (Gamepad.all.Count < 2)
        {
            return;
        }

        PlayerInput player1Input = PlayerInput.Instantiate(
            carPrefabsP1[p1Index],
            controlScheme: "Gamepad",
            pairWithDevice: Gamepad.all[0]
        );
        player1Input.transform.position = spawnPointP1.position;
        player1Input.transform.rotation = Quaternion.identity;

        PlayerInput player2Input = PlayerInput.Instantiate(
            carPrefabsP2[p2Index],
            controlScheme: "Gamepad",
            pairWithDevice: Gamepad.all[1]
        );
        player2Input.transform.position = spawnPointP2.position;
        player2Input.transform.rotation = Quaternion.identity;

        player2Input.transform.localScale = new Vector3(-1, 1, 1);

        ParticleSystem[] particles = player2Input.GetComponentsInChildren<ParticleSystem>(true);
        foreach (var ps in particles)
        {
            Vector3 localPos = ps.transform.localPosition;
            localPos.x *= -1f;
            ps.transform.localPosition = localPos;

            Vector3 localRot = ps.transform.localEulerAngles;
            localRot.y += 180f;
            ps.transform.localEulerAngles = localRot;
        }
    }
}
