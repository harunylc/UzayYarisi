using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class PowerUpManager : MonoBehaviour
{
    [Header("UI References")] public Image P1_PowerUpImage;
    public Image P2_PowerUpImage;

    [Header("PowerUp Prefabs (6 farklı)")] public GameObject[] powerUps;

    private GameObject P1_currentPowerUp = null;
    private GameObject P2_currentPowerUp = null;

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

        groundPowerUp.SetActive(false);
    }

    public void OnP1PowerUp(InputAction.CallbackContext context)
    {
        if (context.performed && P1_currentPowerUp != null)
        {
            ActivatePowerUp(P1_currentPowerUp, "Player");
            P1_currentPowerUp = null;
            P1_PowerUpImage.enabled = false;
        }
    }

    public void OnP2PowerUp(InputAction.CallbackContext context)
    {
        if (context.performed && P2_currentPowerUp != null)
        {
            ActivatePowerUp(P2_currentPowerUp, "Player2");
            P2_currentPowerUp = null;
            P2_PowerUpImage.enabled = false;
        }
    }

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

        else if (powerUpPrefab.GetComponent<EnemySlowPU>() != null)
        {
            EnemySlowPU slow = powerUpPrefab.GetComponent<EnemySlowPU>();

            if (playerTag == "Player")
            {
                DriveMyCar_Player2 enemy = FindObjectOfType<DriveMyCar_Player2>();
                if (enemy != null)
                    enemy.StartCoroutine(slow.SlowDown(enemy));
            }
            else if (playerTag == "Player2")
            {
                DriveMyCar enemy = FindObjectOfType<DriveMyCar>();
                if (enemy != null)
                    enemy.StartCoroutine(slow.SlowDown(enemy));
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
        //yunus
        else if (powerUpPrefab.GetComponent<PU_Nitro>() != null)
        {
            PU_Nitro slow = powerUpPrefab.GetComponent<PU_Nitro>();

            if (playerTag == "Player")
            {
                DriveMyCar myCar = FindObjectOfType<DriveMyCar>();
                if (myCar != null)
                    myCar.StartCoroutine(slow.Nitro(myCar));
            }
            else if (playerTag == "Player2")
            {
                DriveMyCar_Player2 myCar = FindObjectOfType<DriveMyCar_Player2>();
                if (myCar != null)
                    myCar.StartCoroutine(slow.Nitro(myCar));
            }
        }
        else if (powerUpPrefab.GetComponent<PU_Gravity>() != null)
        {
            if (playerTag == "Player")
            {
                Rigidbody2D rb1 = opponent.GetComponent<Rigidbody2D>();
                rb1.mass *= 0.5f;
            }
            else if (playerTag == "Player2")
            {
                Rigidbody2D rb2 = opponent.GetComponent<Rigidbody2D>();
                rb2.mass *= 0.5f;
            }
            else
            {
                Debug.Log("Error tag bunulamadi gravity icin !!!!!!!!!!!");
            }
        }
        else if (powerUpPrefab.GetComponent<PU_DarkScreen>() != null)
        {
            // Hedef panelin etiketini (Tag) belirle
            string targetPanelTag = (playerTag == "Player") ? "P2_DarkPanel" : "P1_DarkPanel";

            // O etikete sahip paneli sahnede bul
            GameObject panelObject = GameObject.FindWithTag(targetPanelTag);

            if (panelObject != null)
            {
                // Panelin üzerindeki PU_DarkScreen script'ini al
                PU_DarkScreen panelScript = panelObject.GetComponent<PU_DarkScreen>();

                if (panelScript != null)
                {
                    // Coroutine'i, her zaman aktif olan PowerUpManager'ın kendisi başlatır,
                    // ama panelin planını (DarkenScreenRoutine) kullanır.
                    StartCoroutine(panelScript.DarkenScreenRoutine(5f));
                }
                else
                {
                    Debug.LogError(panelObject.name + " objesinin üzerinde PU_DarkScreen script'i bulunamadı!");
                }
            }
            else
            {
                Debug.LogError(targetPanelTag + " etiketine sahip bir panel bulunamadı!");
            }
        }
    }
}