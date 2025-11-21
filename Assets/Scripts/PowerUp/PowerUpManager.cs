using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [Header("UI References")] 
    public Image P1_PowerUpImage;
    public Image P2_PowerUpImage;

    [Header("PowerUp Prefabs")] public GameObject[] powerUps;
    private GameObject P1_currentPowerUp = null;
    private GameObject P2_currentPowerUp = null;

    [Header("Player UI Texts")] 
    public TextMeshProUGUI P1_Text;
    public TextMeshProUGUI P2_Text;

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        FindUIReferences();
    }

    private void FindUIReferences()
    {
        if (P1_Text == null)
        {
            GameObject t1 = GameObject.FindGameObjectWithTag("P1_Text");
            if (t1 != null)
                P1_Text = t1.GetComponent<TextMeshProUGUI>();
        }

        if (P2_Text == null)
        {
            GameObject t2 = GameObject.FindGameObjectWithTag("P2_Text");
            if (t2 != null)
                P2_Text = t2.GetComponent<TextMeshProUGUI>();
        }

        if (P1_PowerUpImage == null)
        {
            GameObject p1Obj = GameObject.FindGameObjectWithTag("P1_PowerUI");
            if (p1Obj != null)
                P1_PowerUpImage = p1Obj.GetComponent<Image>();
        }

        if (P2_PowerUpImage == null)
        {
            GameObject p2Obj = GameObject.FindGameObjectWithTag("P2_PowerUI");
            if (p2Obj != null)
                P2_PowerUpImage = p2Obj.GetComponent<Image>();
        }

        if (P1_currentPowerUp != null && P1_PowerUpImage != null)
            P1_PowerUpImage.sprite = P1_currentPowerUp.GetComponent<SpriteRenderer>().sprite;

        if (P2_currentPowerUp != null && P2_PowerUpImage != null)
            P2_PowerUpImage.sprite = P2_currentPowerUp.GetComponent<SpriteRenderer>().sprite;
    }

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

    public void UsePowerUp_P1()
    {
        if (P1_currentPowerUp == null) return;

        ActivatePowerUp(P1_currentPowerUp, "Player");
        P1_currentPowerUp = null;
        P1_PowerUpImage.enabled = false;
    }

    public void UsePowerUp_P2()
    {
        if (P2_currentPowerUp == null) return;

        ActivatePowerUp(P2_currentPowerUp, "Player2");
        P2_currentPowerUp = null;
        P2_PowerUpImage.enabled = false;
    }

    private void ActivatePowerUp(GameObject powerUpPrefab, string playerTag)
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player == null) return;

        TextMeshProUGUI playerText;
        TextMeshProUGUI enemyText;

        if (playerTag == "Player")
        {
            playerText = P1_Text;
            enemyText = P2_Text;
        }
        else if (playerTag == "Player2")
        {
            playerText = P2_Text;
            enemyText = P1_Text;
        }
        else
        {
            playerText = null;
            enemyText = null;
        }

        float displayDuration = 2f;

        if (powerUpPrefab.GetComponent<Shield>() != null)
        {
            GameObject shield = Instantiate(powerUpPrefab, player.transform.position, Quaternion.identity);
            shield.transform.SetParent(player.transform);
            shield.transform.localPosition = Vector3.zero;

            if (playerText != null)
                StartCoroutine(ShowText(playerText, "Kalkan Aktif Edildi!", displayDuration));
        }
        else if (powerUpPrefab.GetComponent<EnemySlowPU>() != null)
        {
            EnemySlowPU slow = powerUpPrefab.GetComponent<EnemySlowPU>();
            if (playerTag == "Player")
            {
                DriveMyCar_Player2 enemy = FindObjectOfType<DriveMyCar_Player2>();
                if (enemy != null)
                {
                    enemy.StartCoroutine(slow.SlowDown(enemy));
                    if (enemyText != null)
                        StartCoroutine(ShowText(enemyText, "Yavaşladın!", displayDuration));
                }
            }
            else if (playerTag == "Player2")
            {
                DriveMyCar enemy = FindObjectOfType<DriveMyCar>();
                if (enemy != null)
                {
                    enemy.StartCoroutine(slow.SlowDown(enemy));
                    if (enemyText != null)
                        StartCoroutine(ShowText(enemyText, "Yavaşladın!", displayDuration));
                }
            }
        }
        else if (powerUpPrefab.GetComponent<ReverseControlsPU>() != null)
        {
            ReverseControlsPU reverse = powerUpPrefab.GetComponent<ReverseControlsPU>();
            if (playerTag == "Player")
            {
                DriveMyCar_Player2 enemy = FindObjectOfType<DriveMyCar_Player2>();
                if (enemy != null)
                {
                    enemy.StartCoroutine(reverse.ReverseControls(enemy.gameObject));
                    if (enemyText != null)
                        StartCoroutine(ShowText(enemyText, "Kontroller Tersine Döndü!", displayDuration));
                }
            }
            else if (playerTag == "Player2")
            {
                DriveMyCar enemy = FindObjectOfType<DriveMyCar>();
                if (enemy != null)
                {
                    enemy.StartCoroutine(reverse.ReverseControls(enemy.gameObject));
                    if (enemyText != null)
                        StartCoroutine(ShowText(enemyText, "Kontroller Tersine Döndü!", displayDuration));
                }
            }
        }
        
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
                Rigidbody2D rb1 = player.GetComponent<Rigidbody2D>();
                rb1.mass *= 0.5f;
            }
            else if (playerTag == "Player2")
            {
                Rigidbody2D rb2 = player.GetComponent<Rigidbody2D>();
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

    private IEnumerator ShowText(TextMeshProUGUI textUI, string message, float duration)
    {
        textUI.text = message;
        textUI.enabled = true;
        yield return new WaitForSeconds(duration);
        textUI.enabled = false;
    }
}