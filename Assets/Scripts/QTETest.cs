using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class QTETest : MonoBehaviour
{
    [Header("QTE Buttons")]
    public GameObject AImage, BImage, XImage, YImage;

    [Header("Raycast Area")]
    public Vector2 raycastPosition;
    public Vector2 raycastSize = new Vector2(2f, 2f);
    public LayerMask playerLayer;

    [Header("Attention UI")]
    public GameObject attentionImage;

    [Header("Countdown Slider")]
    public Slider countdownSlider;
    public float countdownTime = 2f;

    [Header("QTE Settings")]
    // public float timeLimit = 2f; // artık slider bazlı olduğu için kullanmıyoruz

    [Header("Meteor Settings")]
    public GameObject meteorPrefab;        // meteor prefab
    public Transform meteorSpawnPoint;     // sahnedeki sabit spawn point
    public Transform player;               // takip edilecek player
    public float meteorSpeed = 6f;         // takip hızı

    private GameObject[] events;
    private string[] buttons = { "A", "B", "X", "Y" };
    private int currentIndex = 0;

    private bool countdownActive = false;
    private bool QTETrigger = false;
    private bool QTECompleted = false;
    private bool meteorSpawned = false;

    void Start()
    {
        events = new GameObject[] { AImage, BImage, XImage, YImage };
        HideAllKeys();

        if (attentionImage != null)
            attentionImage.SetActive(false);

        if (countdownSlider != null)
        {
            countdownSlider.maxValue = countdownTime;
            countdownSlider.value = countdownTime;
            countdownSlider.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (QTECompleted)
            return;

        if (!QTETrigger)
        {
            bool playerInArea = Physics2D.OverlapBox(raycastPosition, raycastSize, 0f, playerLayer) != null;
            if (attentionImage != null)
                attentionImage.SetActive(playerInArea);
        }

        // Sayaç azalışı: slider 0 olduğunda meteor spawn olacak
        if (countdownActive && countdownSlider != null)
        {
            countdownSlider.value -= Time.deltaTime;
            if (countdownSlider.value <= 0f)
            {
                countdownSlider.value = 0f;
                countdownActive = false;

                // Meteor sadece bir kere spawnlansın
                if (!meteorSpawned)
                {
                    meteorSpawned = true;
                    OnQTEFailedByTime();
                }
            }
        }

        // QTE aktifken buton input kontrolü (başarılı tamamlama)
        if (QTETrigger)
        {
            string currentButton = buttons[currentIndex];
            bool pressed = false;

            switch (currentButton)
            {
                case "A": pressed = Gamepad.current?.buttonSouth.wasPressedThisFrame ?? false; break;
                case "B": pressed = Gamepad.current?.buttonEast.wasPressedThisFrame ?? false; break;
                case "X": pressed = Gamepad.current?.buttonWest.wasPressedThisFrame ?? false; break;
                case "Y": pressed = Gamepad.current?.buttonNorth.wasPressedThisFrame ?? false; break;
            }

            if (pressed)
                NextKeys();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (QTECompleted)
            return;

        if (other.CompareTag("Player"))
        {
            if (attentionImage != null)
                attentionImage.SetActive(false);

            QTETrigger = true;
            currentIndex = 0;
            RandomKeys();
            ShowCurrentKeys();

            if (countdownSlider != null)
            {
                countdownSlider.gameObject.SetActive(true);
                countdownSlider.value = countdownTime;
                countdownActive = true;
            }

            // reset spawn flag (aynı QTE tekrar tetiklenirse)
            meteorSpawned = false;
        }
    }

    void ShowCurrentKeys()
    {
        HideAllKeys();
        events[currentIndex].SetActive(true);
    }

    void NextKeys()
    {
        currentIndex++;
        if (currentIndex < events.Length)
        {
            ShowCurrentKeys();
        }
        else
        {
            // QTE başarıyla tamamlandı — meteor spawn olmaz
            HideAllKeys();
            QTETrigger = false;
            QTECompleted = true;
            if (countdownSlider != null) countdownSlider.gameObject.SetActive(false);
            Debug.Log("✅ Başardın!");
        }
    }

    void HideAllKeys()
    {
        foreach (var img in events)
            if (img != null) img.SetActive(false);
    }

    void RandomKeys()
    {
        for (int i = 0; i < events.Length; i++)
        {
            int rand = Random.Range(i, events.Length);
            (events[i], events[rand]) = (events[rand], events[i]);
            (buttons[i], buttons[rand]) = (buttons[rand], buttons[i]);
        }
    }

    // Slider sıfırlandığında çağrılır
    void OnQTEFailedByTime()
    {
        Debug.Log("❌ Zaman doldu — meteor spawn ediliyor!");
        HideAllKeys();
        QTETrigger = false;
        QTECompleted = true;

        if (countdownSlider != null)
            countdownSlider.gameObject.SetActive(false);

        SpawnMeteor();
    }

    void SpawnMeteor()
    {
        if (meteorPrefab == null || meteorSpawnPoint == null || player == null)
        {
            Debug.LogWarning("Meteor spawn edilemiyor: prefab, spawnPoint veya player atanmamış.");
            return;
        }

        GameObject meteor = Instantiate(meteorPrefab, meteorSpawnPoint.position, Quaternion.identity);

        // MeteorFollow komponentini ekleyip hedefi ata
        MeteorFollow mf = meteor.GetComponent<MeteorFollow>();
        if (mf == null) mf = meteor.AddComponent<MeteorFollow>();
        mf.target = player;
        mf.speed = meteorSpeed;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(raycastPosition, raycastSize);
    }
}

public class MeteorFollow : MonoBehaviour
{
    public Transform target;
    public float speed = 6f;

    void Update()
    {
        if (target == null)
        {
            return;
        }
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("💥 Meteor player’a çarptı!");

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
