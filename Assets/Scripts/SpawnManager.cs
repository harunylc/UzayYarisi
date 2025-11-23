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
            // return; // Test için geçici kapattık
        }

        // ================= PLAYER 1 KISMI =================
        
        // 1. Aracı Yaratıyoruz (Senin eski kodun)
        PlayerInput player1Input = PlayerInput.Instantiate(
            carPrefabsP1[p1Index],
            controlScheme: "Gamepad",
            pairWithDevice: Gamepad.all.Count > 0 ? Gamepad.all[0] : null
        );
        player1Input.transform.position = spawnPointP1.position;
        player1Input.transform.rotation = Quaternion.identity;

        // 2. [YENİ KISIM] Aracı yarattıktan hemen sonra hafızasına ilk yeri kaydediyoruz
        // Yarattığımız aracın içindeki "CarRespawnSystem" scriptini buluyoruz.
        CarRespawnSystem p1Respawn = player1Input.GetComponent<CarRespawnSystem>();
        
        // Eğer scripti bulduysak (yani prefab'a eklemeyi unutmadıysan)
        if (p1Respawn != null)
        {
            // Araca "Senin ilk kayıt noktan spawnPointP1'dir" diyoruz.
            p1Respawn.SetInitialSpawnPoint(spawnPointP1);
        }


        // ================= PLAYER 2 KISMI =================
        
        if (Gamepad.all.Count > 1)
        {
            // 1. Aracı Yaratıyoruz (Senin eski kodun)
            PlayerInput player2Input = PlayerInput.Instantiate(
                carPrefabsP2[p2Index],
                controlScheme: "Gamepad",
                pairWithDevice: Gamepad.all[1]
            );
            player2Input.transform.position = spawnPointP2.position;
            player2Input.transform.rotation = Quaternion.identity;

            // Player 2 Ters Çevirme (Senin eski kodun)
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

            // 2. [YENİ KISIM] Player 2 için de aynısını yapıyoruz
            CarRespawnSystem p2Respawn = player2Input.GetComponent<CarRespawnSystem>();
            
            if (p2Respawn != null)
            {
                // Player 2'ye "Senin ilk kayıt noktan spawnPointP2'dir" diyoruz.
                p2Respawn.SetInitialSpawnPoint(spawnPointP2);
            }
        }
    }
}