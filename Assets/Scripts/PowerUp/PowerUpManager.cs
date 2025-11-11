using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class PowerUpManager : MonoBehaviour
{
    [Header("UI References")] public Image P1_PowerUpImage;
    public Image P2_PowerUpImage;

    [Header("PowerUp Prefabs (6 farklı)")] public GameObject[] powerUps; // 6 farklı power-up prefab

    private GameObject P1_currentPowerUp = null;
    private GameObject P2_currentPowerUp = null;
    

    // Power-up alındığında çağrılır
    public void CollectPowerUp(GameObject player, GameObject groundPowerUp)
    {
        int index = Random.Range(0, powerUps.Length);
        GameObject selectedPowerUp = powerUps[index];

        if (player.CompareTag("Player"))
        {
            P1_currentPowerUp = selectedPowerUp;
            P1_PowerUpImage.sprite = selectedPowerUp.GetComponent<SpriteRenderer>().sprite;
            P1_PowerUpImage.enabled = true;
        }
        else if (player.CompareTag("Player2"))
        {
            P2_currentPowerUp = selectedPowerUp;
            P2_PowerUpImage.sprite = selectedPowerUp.GetComponent<SpriteRenderer>().sprite;
            P2_PowerUpImage.enabled = true;
        }

        // Sahnedeki ground power-up objesini gizle
        groundPowerUp.SetActive(false);
    }

    // Player1 LT tuşu ile kullanacak
    public void OnP1PowerUp(InputAction.CallbackContext context)
    {
        if (context.performed && P1_currentPowerUp != null)
        {
            ActivatePowerUp(P1_currentPowerUp, "Player");
            P1_currentPowerUp = null;
            P1_PowerUpImage.enabled = false;
        }
    }

    // Player2 LT tuşu ile kullanacak
    public void OnP2PowerUp(InputAction.CallbackContext context)
    {
        if (context.performed && P2_currentPowerUp != null)
        {
            ActivatePowerUp(P2_currentPowerUp, "Player2");
            P2_currentPowerUp = null;
            P2_PowerUpImage.enabled = false;
        }
    }

    // Power-up'u sahnede aktif et (tag ile oyuncu seçiliyor)
    private void ActivatePowerUp(GameObject powerUpPrefab, string playerTag)
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player == null) return;

        //yunus
        string opponentTag = (playerTag == "Player") ? "Player2" : "Player";
        GameObject opponent = GameObject.FindGameObjectWithTag(opponentTag);

        // 1️⃣ Eğer Shield power-up'ıysa
        if (powerUpPrefab.GetComponent<Shield>() != null)
        {
            GameObject shield = Instantiate(powerUpPrefab, player.transform.position, Quaternion.identity);
            shield.transform.SetParent(player.transform);
            shield.transform.localPosition = Vector3.zero;
        }

        // 2️⃣ Eğer Rakibi Yavaşlatma power-up'ıysa
        else if (powerUpPrefab.GetComponent<EnemySlowPU>() != null)
        {
            EnemySlowPU slow = powerUpPrefab.GetComponent<EnemySlowPU>();

            if (playerTag == "Player")
            {
                DriveMyCar_Player2 enemy = FindObjectOfType<DriveMyCar_Player2>();
                if (enemy != null)
                    enemy.StartCoroutine(slow.SlowDown(enemy)); // 🔥 coroutine'i aktif olan rakipte başlatıyoruz
            }
            else if (playerTag == "Player2")
            {
                DriveMyCar enemy = FindObjectOfType<DriveMyCar>();
                if (enemy != null)
                    enemy.StartCoroutine(slow.SlowDown(enemy)); // 🔥 burada da aynı
            }
        }
        else if (powerUpPrefab.GetComponent<ReverseControlsPU>() != null)
        {
            ReverseControlsPU reverse = powerUpPrefab.GetComponent<ReverseControlsPU>();

            if (playerTag == "Player")
            {
                DriveMyCar_Player2 enemy = FindObjectOfType<DriveMyCar_Player2>();
                if (enemy != null)
                    enemy.StartCoroutine(reverse.ReverseControls(enemy.gameObject));
            }
            else if (playerTag == "Player2")
            {
                DriveMyCar enemy = FindObjectOfType<DriveMyCar>();
                if (enemy != null)
                    enemy.StartCoroutine(reverse.ReverseControls(enemy.gameObject));
            }
        }
        // else if (powerUpPrefab.GetComponent<PU_GravityID>() != null)
        // {
        //     PU_GravityID gravity = powerUpPrefab.GetComponent<PU_GravityID>();
        //     
        //     if (playerTag == "Player")
        //     {
        //         gravity.ApplyEffectToPlayer1();
        //     }
        //     else if (playerTag == "Player2")
        //     {
        //         gravity.ApplyEffectToPlayer2();
        //
        //     }
        // }
        // else if (powerUpPrefab.GetComponent<PU_DarkScreenID>() != null)
        // {
        //     // Prefabın üzerindeki PU_DarkScreenID script'ini al
        //     PU_DarkScreenID darkScreen = powerUpPrefab.GetComponent<PU_DarkScreenID>();
        //
        //     if (playerTag == "Player")
        //     {
        //         StartCoroutine(darkScreen.DarkenPlayer2Screen(5f));
        //     }
        //     else if (playerTag == "Player2")
        //     {
        //         StartCoroutine(darkScreen.DarkenPlayer1Screen(5f));
        //     }
        // }
        // else if (powerUpPrefab.GetComponent<PU_NitroID>() != null)
        // {
        //     PU_NitroID nitro = powerUpPrefab.GetComponent<PU_NitroID>();
        //     
        //     if (playerTag == "Player")
        //     {
        //         StartCoroutine(nitro.BoostPlayer1NitroRecharge(10f)); 
        //     }
        //     else if (playerTag == "Player2")
        //     {
        //         StartCoroutine(nitro.BoostPlayer2NitroRecharge(10f));
        //
        //     }
        //
        // }
    }
}