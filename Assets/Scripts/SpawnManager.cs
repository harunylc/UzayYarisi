using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnManager : MonoBehaviour
{
    [Header("Car Prefabs")]
    public GameObject[] carPrefabsP1, carPrefabsP2;

    [Header("Spawn Points")]
    public Transform spawnPointP1;
    public Transform spawnPointP2;

    void Start()
    {
        int p1Index = PlayerSelectionData.player1CarIndex;
        int p2Index = PlayerSelectionData.player2CarIndex;

        GameObject player1 = Instantiate(carPrefabsP1[p1Index], spawnPointP1.position, Quaternion.identity);
        GameObject player2 = Instantiate(carPrefabsP2[p2Index], spawnPointP2.position, Quaternion.identity);
        player2.transform.rotation = Quaternion.Euler(0, 180, 0);

        PlayerInput input1 = player1.GetComponent<PlayerInput>();
        PlayerInput input2 = player2.GetComponent<PlayerInput>();

        if (Gamepad.all.Count >= 2)
        {
            if (input1 != null)
            {
                input1.SwitchCurrentControlScheme("Gamepad", Gamepad.all[0]);
            }

            if (input2 != null)
            {
                input2.SwitchCurrentControlScheme("Gamepad", Gamepad.all[1]);
            }
        }
    }
}